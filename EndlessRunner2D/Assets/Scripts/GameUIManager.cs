using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;

    private bool startScreenShown = true;
    private bool scoreShown = false;

    void Start()
    {
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);

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
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
