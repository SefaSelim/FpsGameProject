using System;
using RaycastPro.RaySensors;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [Header("Sensors")]
    [SerializeField] RadialRay radialRaySensor;
    [SerializeField] private RadialRay attackZoneSensor;

    [Header("Player Health")]
    [SerializeField] PlayerHealth playerHealth;
    
    public bool isDead = false; 
    
    private bool isIdle = true;
    private bool isAggroed = false;
    
    private float attackTimer = 0f;
    private float zombieDamage;
    
    [SerializeField] private Transform playerTransform;
    NavMeshAgent navMeshAgent;
    private Animator zombieAnimator;

    private Vector3 movePosition;

    
    private Quaternion targetRotation;
    
    [SerializeField] Shooting shootingScript;

    private void Start()
    {
        playerHealth = GameManagerScript.Instance.playerHealthPrefab;
        playerTransform = GameManagerScript.Instance.playerTransformPrefab;
        shootingScript = GameManagerScript.Instance.shootingPrefab;
        
        shootingScript.OnShoot += Shooting_OnShoot;
        
        movePosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        
        radialRaySensor.SetDirection(new Vector3(0f,0f,StaticSettings.ZombieAgrooRange));
        zombieDamage = StaticSettings.ZombieDamage;
    }

    private void Update()
    {
        if (isDead)
        {
            navMeshAgent.enabled = false;
            return;
        }
        
        attackTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ZombieAttack();
        }
        
        if (isAggroed)
        {
            isIdle = false;
            navMeshAgent.enabled = true;
            OnAgroo(movePosition);

            
            Vector3 flatMovePos = new Vector3(movePosition.x, 0f, movePosition.z);
            Vector3 flatCurrentPos = new Vector3(transform.position.x, 0f, transform.position.z);

            if ((flatMovePos - flatCurrentPos).magnitude <= 1.5f && (playerTransform.position - transform.position).magnitude > 3f)
            {
                    isAggroed = false;
                    navMeshAgent.enabled = false;
                    isIdle = true;
            }
        }
        else
        {
            zombieAnimator.SetBool("IsAgroo", false);
            isIdle = true;
        }

        if (isIdle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
        }
    }

    public void SetIdleRotation()
    {
        targetRotation = Quaternion.Euler(0f, UnityEngine.Random.Range(transform.rotation.y - 90f, transform.rotation.y + 90f),0f);
    }
    
    
    // Ses duydugunda sesin sesin geldiği yere doğru hareket eder
    public void ShootSoundHeard()
    {
            movePosition = playerTransform.position;
            isAggroed = true;

    }
    
    public void ZombieAttack()
    {
        if (isDead)
        {
            return;
        }
        
        if (attackTimer >= 2f)
        {
            foreach (RaycastHit raySensor in attackZoneSensor.raycastHits)
            {
                if (raySensor.collider != null && raySensor.collider.CompareTag("Player"))
                {
                    zombieAnimator.SetTrigger("Attack");
                    attackTimer = 0f;
                    return;
                }
            }
        }
    }

    public void CheckZombieHit()
    {
        foreach (RaycastHit raySensor in attackZoneSensor.raycastHits)
        {
            if (raySensor.collider != null && raySensor.collider.CompareTag("Player"))
            {
                playerHealth.TakeDamage(zombieDamage);
                return;
            }
        }
    }
    
    
    
    // Agroo yu true yapmak için dışarıdan erişilebilecek bir fonksiyon
    public void SetAgroo()
    {
        foreach (RaycastHit raySensor in radialRaySensor.raycastHits)
        {
            if (raySensor.collider != null  && raySensor.collider.CompareTag("Player"))
            {
                isAggroed = true;
                movePosition = playerTransform.position;
                return;
            }
        }
    }

    
    
    
    // NavmeshAgent'ın destination'ını set eder
    private void OnAgroo(Vector3 agrooPosition)
    {
        zombieAnimator.SetBool("IsAgroo", true);
        navMeshAgent.SetDestination(agrooPosition);

    }
    
    public void Shooting_OnShoot(object sender, EventArgs e)
    {
        if ((transform.position - playerTransform.position).magnitude < StaticSettings.ZombieHearRadius)
        {
            ShootSoundHeard();
        }
    }

    private void OnDestroy()
    {
        shootingScript.OnShoot -= Shooting_OnShoot;
    }
}
