using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Button resumeButton;
    public Button restartButton;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        resumeButton.onClick.AddListener(TogglePause);
        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameRunning)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        // Optional: also pause player animation
        Animator playerAnim = FindObjectOfType<PlayerController>()?.GetComponent<Animator>();
        if (playerAnim != null)
            playerAnim.speed = isPaused ? 0f : 1f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
