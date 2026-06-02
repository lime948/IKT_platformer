using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Detection")]
    public float shootingRadius = 15f;       // Distance at which enemy starts shooting
    public LayerMask groundLayer;            // Set this to your ground/platform layer

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 50f;
    public float fireRate = 1f;              // Shots per second
    private float nextFireTime;

    [Header("Movement")]
    public float stoppingDistance = 5f;      // How close to get before stopping

    // References
    private NavMeshAgent agent;
    private Transform player;
    private bool playerInRange;

    private void Awake()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        bool sameGround = IsOnSamePlatform();
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (sameGround)
        {
            // Move toward player if outside stopping distance
            if (distToPlayer > stoppingDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            else
            {
                agent.isStopped = true;
            }

            // Shoot if within radius
            if (distToPlayer <= shootingRadius)
            {
                FacePlayer();

                if (Time.time >= nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        }
        else
        {
            // Player on different platform, stop moving
            agent.isStopped = true;
        }
    }

    private bool IsOnSamePlatform()
    {
        // Raycast straight down from both enemy and player, check if they hit the same collider
        RaycastHit enemyHit, playerHit;

        bool enemyGrounded = Physics.Raycast(transform.position, Vector3.down, out enemyHit, 5f, groundLayer);
        bool playerGrounded = Physics.Raycast(player.position, Vector3.down, out playerHit, 5f, groundLayer);

        if (!enemyGrounded || !playerGrounded) return false;

        return enemyHit.collider == playerHit.collider;
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // keep upright
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
    }

    private void Shoot()
    {
        if (bulletPrefab == null || bulletSpawn == null) return;

        Vector3 direction = (player.position - bulletSpawn.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.forward = direction;
        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletVelocity, ForceMode.Impulse);

        Destroy(bullet, 5f);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0f) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}