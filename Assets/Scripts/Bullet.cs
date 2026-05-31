using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 25f;

    private void OnCollisionEnter(Collision collision)
    {
        Target target = collision.gameObject.GetComponent<Target>();

        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }

        // Destroy(gameObject);
    }
}