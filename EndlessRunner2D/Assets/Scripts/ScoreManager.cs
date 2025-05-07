using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public float scoreMultiplier = 1f;

    private float currentScore = 0f;
    private int highScore = 0;
    private bool hasRecordedHighScore = false;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    void Update()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.isGameRunning)
            {
                currentScore += GameManager.Instance.gameSpeed * scoreMultiplier * Time.deltaTime;
                UpdateUI();
                hasRecordedHighScore = false;
            }
            else if (GameManager.Instance.isGameOver && !hasRecordedHighScore)
            {
                TryUpdateHighScore();
            }
        }
    }

    void UpdateUI()
    {
        int scoreInt = Mathf.FloorToInt(currentScore);
        scoreText.text = $"Score: {scoreInt}";
        highScoreText.text = $"Best: {highScore}";
    }

    void TryUpdateHighScore()
    {
        int finalScore = Mathf.FloorToInt(currentScore);
        if (finalScore > highScore)
        {
            highScore = finalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        hasRecordedHighScore = true;
    }

    public void ResetScore()
    {
        currentScore = 0f;
        hasRecordedHighScore = false;
        UpdateUI();
    }

    // ✅ Add this method to expose the score to GameUIManager
    public int GetFinalScore()
    {
        return Mathf.FloorToInt(currentScore);
    }

    public bool IsNewHighScore()
    {
        return Mathf.FloorToInt(currentScore) > highScore;
    }

}
