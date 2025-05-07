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

    private bool startScreenShown = true;
    private bool scoreShown = false;

    void Start()
    {
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        newHighScoreText.SetActive(false); // hide initially

        restartButton.onClick.AddListener(RestartGame);
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
                    {
                        newHighScoreText.SetActive(true);
                    }
                    else
                    {
                        newHighScoreText.SetActive(false);
                    }
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
}
