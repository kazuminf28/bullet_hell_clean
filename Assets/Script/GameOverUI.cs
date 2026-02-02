using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text highScoreText;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("SCORE", 0);
        int highScore  = PlayerPrefs.GetInt("HIGH_SCORE", 0);

        finalScoreText.text = "Final_Score : " + finalScore.ToString();
        highScoreText.text = "High_Score : " + highScore.ToString();
    }
}
