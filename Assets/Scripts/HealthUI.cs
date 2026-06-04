using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TextMeshProUGUI hpText;

    void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthText;
    }

    void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealthText;
    }

    void UpdateHealthText(float newHealthValue)
    {
        hpText.text = "HP: " + newHealthValue.ToString("F0");
    }
}

