using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region Variables
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;
    #endregion
    #region Functions
    #region Builtin Functions
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isGamePaused)
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
    }
    #endregion
    #region Custom Functions

    public void Resume() 
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    public void OptionsMenu() 
    {
    
    }

    public void ReturnMainMenu() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit(); //exits the game
    }
    #endregion
    #endregion
}
