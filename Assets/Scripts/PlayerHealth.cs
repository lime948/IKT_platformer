using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player died!");
		Destroy(gameObject); // Zétény pls ide a lose menut vagy vmi mert most csak kitöröl mint Sztálin a politikai ellenfeleit
        // handle death here later
    }
}