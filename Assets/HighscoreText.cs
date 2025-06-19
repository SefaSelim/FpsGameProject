using System;
using TMPro;
using UnityEngine;

public class HighscoreText : MonoBehaviour
{
    private TextMeshProUGUI highscore;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        highscore = GetComponent<TextMeshProUGUI>();
        highscore.text = StaticSettings.HighScore.ToString();
    }
}
