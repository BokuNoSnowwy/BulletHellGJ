using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName;
    
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
