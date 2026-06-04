using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public event Action<float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Player HP: " + currentHealth);

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player died!");
        SceneManager.LoadScene("LevelLoseMenu");
    }
}
