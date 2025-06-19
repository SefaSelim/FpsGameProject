using Unity.Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [Header("ScreenShake Settings")]
    [SerializeField] private CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer = 0f;
    private float shakeTimerTotal = 0f;
    private float startingIntensity = 0f;
    
    [SerializeField] private float shakeIntensity = 0.5f;
    [SerializeField] private float shakeDuration = 0.2f;
    
    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            noise.AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1f - (shakeTimer / shakeTimerTotal));
        }
    }

    public void ShakeScreen()
    {
        noise.AmplitudeGain = shakeIntensity;
        
        shakeTimer = shakeDuration;
        shakeTimerTotal = shakeDuration;
        startingIntensity = shakeIntensity;
        
    }
    
}
