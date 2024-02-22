using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTextIG;

    
    public static Score Instance { get; private set;}

    public void Update()
    {
        scoreText.text = "Score: " + score.ToString();
        scoreTextIG.text = "Score: " + score.ToString();
    } 

    public void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
