using UnityEngine;

public class ZombieHeathManager : MonoBehaviour
{
    AudioSource audioSource;
    [Header("Audio Settings")]
    [SerializeField] AudioClip headExplosionSound;
    
    [SerializeField] private float maxHealth = 100;
    private float currentHealth;

    private Animator zombieAnimator;
    
    ZombieController zombieController;
    

    private void Start()
    {
        currentHealth = maxHealth;
        zombieAnimator = GetComponent<Animator>();
        zombieController = GetComponent<ZombieController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!zombieController.isDead)
        {
            // head explosion sound
            foreach (Collider head in GetComponentsInChildren<Collider>())
            {
                if (head.CompareTag("ZombieHead"))
                {
                    head.gameObject.SetActive(false);
                }
            }
            audioSource.PlayOneShot(headExplosionSound);
            
            zombieController.isDead = true;
            zombieAnimator.SetTrigger("Die");
            Destroy(gameObject, 2f);
            StaticSettings.GainedScore = StaticSettings.GainedScorePerKill;
            StaticSettings.GetScore = true;
        }

    }

}
