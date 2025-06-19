using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomInitialition : MonoBehaviour
{
    [SerializeField] private GameObject ZombiePrefab;

    private float timer = 0;
    private int count;
    
    float spawnTime = 6f;
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 5f)
        {
            count++;
            InitZombie();
            timer = 0f; 
        }

        if (count >= 10)
        {
            spawnTime = spawnTime * 9 / 10;
            count = 0;
        }
    }


    public void InitZombie()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-60f, 60f),
            0f,
            Random.Range(-60f, 100f)
        );
        
        Instantiate(ZombiePrefab, randomPosition, Quaternion.identity);
    }
    
}
