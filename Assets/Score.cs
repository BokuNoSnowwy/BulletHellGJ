using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTextIG;

    public void Update()
    {
        Debug.Log("score : " + score);

        scoreText.text = "Score: " + score.ToString();
        scoreTextIG.text = "Score: " + score.ToString();
    }
}
