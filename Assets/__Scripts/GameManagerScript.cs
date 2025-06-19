using System;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [Header("Zombie Settings")]
    [SerializeField] private float zombieHearRadius = 30f;
    [SerializeField] private float zombieAgrooRange = 10f;
    [SerializeField] private float zombieDamage = 10f;
    [SerializeField] private int gainedScorePerKill = 10;
        
    [Header("Player Settings")]
    [SerializeField] private float playerMaxHealth = 100f;
    
    [Header("Initialization Prefabs")]
    public PlayerHealth playerHealthPrefab;
    public Transform playerTransformPrefab;
    public Shooting shootingPrefab;
    
    public static GameManagerScript Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        
        StaticSettings.ZombieHearRadius = zombieHearRadius;
        StaticSettings.ZombieAgrooRange = zombieAgrooRange;
        StaticSettings.PlayerMaxHealth = playerMaxHealth;
        StaticSettings.ZombieDamage = zombieDamage;
        StaticSettings.GainedScorePerKill = gainedScorePerKill;
    }
}
