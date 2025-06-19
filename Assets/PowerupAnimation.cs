using System;
using UnityEngine;

public class PowerupAnimation : MonoBehaviour
{
    private void Update()
    {
        // Rotate the powerup object around its Y-axis
        transform.Rotate(Vector3.up, 50 * Time.deltaTime);
        
        // Optionally, you can also add a slight vertical bobbing effect
        float bobbingSpeed = 2f;
        float bobbingHeight = 0.002f;
        Vector3 position = transform.position;
        position.y += Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            if (gameObject.CompareTag("Ammo"))
            {
                StaticSettings.AddBulletAmount = 20;
            }
            
            if (gameObject.CompareTag("Heal"))
            {
                StaticSettings.AddHealthAmount = 20;
            }
            
            
            Invoke("Reinitilize", 40f);
        }
    }

    private void Reinitilize()
    {
        gameObject.SetActive(true);
    }
}
