using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject controlsPanel;
    public GameObject pauseMenu;
    private bool isMainMenuActive = true;

    void Start()
    {
        controlsPanel.SetActive(false);
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }


    public void Play()
    {
        isMainMenuActive = false;
        Debug.Log("launching : Playscene");
        SceneManager.LoadScene("PlayScene");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        if (!isMainMenuActive)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
    }

    public void MainMenu()
    {
        Debug.Log("launching : MainMenuScene");
        SceneManager.LoadScene("MainMenuScene");
        isMainMenuActive = true;
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
        Debug.Log("Quit");
        Application.Quit();
    }
}