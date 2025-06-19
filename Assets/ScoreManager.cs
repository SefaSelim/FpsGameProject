using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private Image comboImage;

    private int score;
    private int Multiplier = 0;

    private float timer = 5f;
    public float ComboResetTime = 5f;
    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        
        score = 0;
        scoreText.text = score.ToString();
        comboText.text = "x " + Multiplier + 1;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (timer > ComboResetTime)
        {
            Multiplier = 0;
            comboText.text = "x " + (Multiplier + 1);
        }
        
        if (StaticSettings.GetScore)
        {
            Multiplier += 1;
            timer = 0f;
            StaticSettings.GetScore = false;
            
            comboText.text = "x " + (Multiplier + 1);
        }

        if (StaticSettings.GainedScore > 0)
        {
            score += StaticSettings.GainedScore * Multiplier;
            UpdateScore();
            StaticSettings.GainedScore = 0;
        }
        
        comboImage.fillAmount = 1 - (timer / ComboResetTime);
    }
    
    private void UpdateScore()
    {
        scoreText.text = score.ToString();
        StaticSettings.HighScore = Mathf.Max(StaticSettings.HighScore, score);
    }
}
