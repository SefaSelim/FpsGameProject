using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : MonoBehaviour
{
    [Header("Healthbar Settings")]
    [SerializeField] private Image healthbarImage;

    [SerializeField] private Image easeBar;
    
    [Header("Player Health")]
    [SerializeField] private PlayerHealth playerHealth;


    private void Update()
    {
        float targetFill = playerHealth.currentHealth / StaticSettings.PlayerMaxHealth;

        if (healthbarImage.fillAmount != targetFill)
            healthbarImage.fillAmount = targetFill;

        if (easeBar.fillAmount > targetFill)
        {
            easeBar.fillAmount = Mathf.Lerp(easeBar.fillAmount, targetFill, Time.deltaTime * 2f);
        }
        else
        {
            easeBar.fillAmount = targetFill; // Can arttıysa anında güncelle
        }
    }
}
