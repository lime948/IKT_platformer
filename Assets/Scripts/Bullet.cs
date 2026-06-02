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
			return;
		}

		PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
		if (player != null)
		{
			player.TakeDamage(damage);
			Destroy(gameObject);
			return;
		}

		// Destroy(gameObject); // hit a wall or something else
    }
}