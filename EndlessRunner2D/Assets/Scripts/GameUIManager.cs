using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public GameObject newHighScoreText;
    public Button restartButton;
    public Button resetHighScoreButton;
    public GameObject resetConfirmPanel;
    public Button confirmResetButton;
    public Button cancelResetButton;

    private bool startScreenShown = true;
    private bool scoreShown = false;

    void Start()
    {
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        newHighScoreText.SetActive(false);

        restartButton.onClick.AddListener(RestartGame);
        resetHighScoreButton.onClick.AddListener(ResetHighScore);

        resetConfirmPanel.SetActive(false);
        confirmResetButton.onClick.AddListener(ConfirmReset);
        cancelResetButton.onClick.AddListener(CancelReset);

        // Disable reset button if score already 0
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        resetHighScoreButton.interactable = currentHighScore > 0;
    }

    void Update()
    {
        if (startScreenShown && Input.anyKeyDown)
        {
            GameManager.Instance.StartGame();
            startPanel.SetActive(false);
            startScreenShown = false;
        }

        if (GameManager.Instance.isGameOver)
        {
            gameOverPanel.SetActive(true);

            if (!scoreShown)
            {
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    finalScoreText.text = "Final Score: " + scoreManager.GetFinalScore();

                    if (scoreManager.IsNewHighScore())
                        newHighScoreText.SetActive(true);
                    else
                        newHighScoreText.SetActive(false);
                }

                scoreShown = true;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void ResetHighScore()
    {
        resetConfirmPanel.SetActive(true); // Show confirmation popup
    }

    void ConfirmReset()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
        Debug.Log("High Score Reset.");

        resetConfirmPanel.SetActive(false);
        resetHighScoreButton.interactable = false;
    }

    void CancelReset()
    {
        resetConfirmPanel.SetActive(false);
    }
}
