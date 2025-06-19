using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

public class Shooting : MonoBehaviour
{
    public event EventHandler OnShoot;
    
    [Header("Screen Shake")]
    [SerializeField] private ScreenShake screenShake;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    private AudioSource audioSource;
    
    [Header("Weapon Settings")]
    [SerializeField] private float maxBulletAmount = 30f;
    private float currentBulletAmount = 120f;
    private float currentBulletAmountInClip;
    public float damage = 10f;
    
    [SerializeField] private TextMeshProUGUI bulletAmountText;
    [SerializeField] private TextMeshProUGUI sideBulletAmountText;
    [SerializeField] private GameObject DamagePopupPrefab;
    
    public float shootDelay = 0.1f;
    bool isReloading = false;
    
    Animator WeaponAnimator;

    [SerializeField] private GameObject crossHair;
    
    [SerializeField] private GameObject LimbsVFXPrefab;
    [SerializeField] private GameObject BloodVFXPrefab;
    [SerializeField] private GameObject dustParticlePrefab;
    [SerializeField] private ParticleSystem muzzleFlashPrefab;
    
    Camera mainCamera;
    private float timer = 0f;
    private void Start()
    {
        WeaponAnimator = GetComponent<Animator>();
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        
        currentBulletAmountInClip = maxBulletAmount;
        bulletAmountText.text = currentBulletAmountInClip.ToString();
        sideBulletAmountText.text = currentBulletAmount.ToString();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (StaticSettings.AddBulletAmount > 0)
        {
            AddBullets();
        }
        
        if (Input.GetKeyDown(KeyCode.R) && currentBulletAmountInClip < maxBulletAmount && currentBulletAmount > 0)
        {
            isReloading = true;
            audioSource.PlayOneShot(reloadSound);
            WeaponAnimator.SetTrigger("Reload");
            Invoke("Reload",2.5f);
        }
        
        if (Input.GetMouseButton(0) && timer >= shootDelay && currentBulletAmountInClip > 0 && !isReloading )
        {
            currentBulletAmountInClip--;
            timer = 0f;
            Shoot();
            
            bulletAmountText.text = currentBulletAmountInClip.ToString();
            sideBulletAmountText.text = currentBulletAmount.ToString();
        }
        
        if (Input.GetMouseButton(1) && !isReloading)
        {
            WeaponAnimator.SetBool("IsScope",true);
            crossHair.SetActive(false);
        }
        else
        {
            WeaponAnimator.SetBool("IsScope",false);
            crossHair.SetActive(true);
        }
    }

    public void Shoot()
    {
        OnShoot?.Invoke(this, EventArgs.Empty);

        damage = UnityEngine.Random.Range(5f, 15f);
        
        screenShake.ShakeScreen();
        WeaponAnimator.SetTrigger("Shoot");
        audioSource.PlayOneShot(shootSound);
        RaycastHit hit;

            muzzleFlashPrefab.Play();
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100f))
            {
                
                if (hit.collider.transform.parent.CompareTag("Zombie"))
                {
                    Destroy(Instantiate(LimbsVFXPrefab, hit.point, Quaternion.LookRotation(transform.position - hit.point)), 1f);
                    Destroy(Instantiate(BloodVFXPrefab, hit.point, Quaternion.LookRotation(transform.position - hit.point)), 1f);
                    
                    if (hit.collider.CompareTag("ZombieHead"))
                    {
                        damage *= 3f;
                    }
                
                    GameObject popup = Instantiate(DamagePopupPrefab,hit.point - transform.forward ,Quaternion.LookRotation(hit.point - mainCamera.transform.position));
                    popup.TryGetComponent<Rigidbody>(out Rigidbody rb);
                    popup.TryGetComponent<TextMeshPro>(out TextMeshPro textMeshPro);
                    textMeshPro.text = Convert.ToInt16(damage).ToString();
                    
                    textMeshPro.fontSize = damage / 15 + 1f;
                    textMeshPro.color = new Color(1f, (180f - ((Mathf.Clamp(damage, 5f, 30f) - 5f) / 25f) * 180f) / 255f, 0f, 1f);

                
                    rb.AddForce(transform.up * 3f + transform.right * UnityEngine.Random.Range(-1f,1f) , ForceMode.Impulse);
                    Destroy(popup,1f);
                    
                
                    if (hit.collider.transform.parent.TryGetComponent<ZombieHeathManager>(out ZombieHeathManager healthManager))
                    {
                        healthManager.TakeDamage(damage);
                    }
                
                    if (hit.collider.CompareTag("ZombieHead"))
                    {
                        damage /= 3f;
                    }
                }
                else
                {
                    Destroy(Instantiate(dustParticlePrefab, hit.point, Quaternion.identity), 2f);
                }

            }
    }
    
    public void Reload()
    {

            if (currentBulletAmount + currentBulletAmountInClip <= maxBulletAmount)
            {
                currentBulletAmountInClip += currentBulletAmount;
                currentBulletAmount = 0f;
            }
            else
            {
                currentBulletAmount -= (maxBulletAmount - currentBulletAmountInClip);
                currentBulletAmountInClip = maxBulletAmount; 
            }
        isReloading = false;
        bulletAmountText.text = currentBulletAmountInClip.ToString();
        sideBulletAmountText.text = currentBulletAmount.ToString();

    }

    public void AddBullets()
    {
        currentBulletAmount += StaticSettings.AddBulletAmount;
        StaticSettings.AddBulletAmount = 0f;
        sideBulletAmountText.text = currentBulletAmount.ToString();
    }
}
