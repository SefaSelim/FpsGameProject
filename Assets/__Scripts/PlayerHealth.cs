using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;

    private void Start()
    {
        currentHealth = StaticSettings.PlayerMaxHealth;
    }

    private void Update()
    {
        if (StaticSettings.AddHealthAmount > 0)
        {
            Heal(StaticSettings.AddHealthAmount);
            StaticSettings.AddHealthAmount = 0;
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
    
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > StaticSettings.PlayerMaxHealth)
        {
            currentHealth = StaticSettings.PlayerMaxHealth;
        }
    }
    private void Die()
    {
        SceneManager.LoadScene("Retry");
    }
}
