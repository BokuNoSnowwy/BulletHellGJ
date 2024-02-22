using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject controlsPanel;
    public GameObject pauseMenu;

    private bool isPaused = false;

    void Start()
    {
        controlsPanel.SetActive(false);
        pauseMenu.SetActive(false);
    }


    public void Play()
    {
        //Debug.Log("launching : GameLoop");
        SceneManager.LoadScene("GameLoop");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    public void MainMenu()
    {
        //Debug.Log("launching : MainMenuScene");
        SceneManager.LoadScene("MainMenuV2");
    }

    public void Controls()
    {
        if(controlsPanel.activeSelf)
        {
            controlsPanel.SetActive(false);
        }
        else
        {
            controlsPanel.SetActive(true);
        }
    }

    public void Quit()
    {
        //Debug.Log("Quit");
        Application.Quit();
    }
}
