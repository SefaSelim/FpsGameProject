using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public enum ButtonType
{
    Start,
    Settings,
    Quit,
    Retry
}

public class ButtonsManager : MonoBehaviour
{
    public ButtonType buttonType;
    
    private Button button;
    bool isClicked = false;
    private Vector3 lerpPosition;

    private void Start()
    {
        button = GetComponent<Button>();
        lerpPosition = transform.position - new Vector3(0f, 5f, 0f);
    }

    private void Update()
    {
        if (isClicked)
        {
            transform.position = Vector3.Lerp(transform.position, lerpPosition, 10f * Time.deltaTime);

            if (transform.position.y <= lerpPosition.y + 0.1f)
            {
                isClicked = false;
                transform.position = lerpPosition;
                
                ButtonEvent(0);
            }
        }
        
    }

    public void OnButtonClick()
    {
        isClicked = true;
    }
    
    private void ButtonEvent(int Index)
    {
        switch (Index)
        {
            case 0:
                SceneManager.LoadScene("Gameplay");
                break;
            case 1:
                break;
            case 2:
                Application.Quit();
                break;
            case 3:
                //ResetGame();
                SceneManager.LoadScene("Gameplay");
                break;
            default:
                break;
        }
    }

    private void ResetGame()
    {
        StaticSettings.HighScore = 0;
    }
}
