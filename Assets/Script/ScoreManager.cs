using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager ScoreManagers; //どこからでも呼べるように

    [SerializeField] private Text scoreText;
    [SerializeField] private Text highText;

    private int score = 0;
    private int highScore = 0;

    void Awake()
    {
        if (ScoreManagers == null) ScoreManagers = this;
        else Destroy(gameObject);// ２個目を自動的に消す
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HIGH_SCORE", 0);
        UpdateUI();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("SCORE", score);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HIGH_SCORE", highScore);
            PlayerPrefs.Save();
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "SCORE : " + score.ToString();
        highText.text = "HIGH_SCORE : " + highScore.ToString();
    }
}
