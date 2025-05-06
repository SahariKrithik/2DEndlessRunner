using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialGameSpeed = 6f;
    public float gameSpeedIncrease = 0.1f;
    public float maxGameSpeed = 12f;
    public float gameSpeed { get; private set; }

    public bool isGameRunning { get; private set; } = false;
    public bool isGameOver { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);
    }

    private void Start()
    {
        SetIdleState();
    }

    private void Update()
    {
        if (isGameRunning)
        {
            gameSpeed += gameSpeedIncrease * Time.deltaTime;
            gameSpeed = Mathf.Min(gameSpeed, maxGameSpeed);
        }

        // Start game on any jump key
        if (!isGameRunning && !isGameOver &&
            (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            StartGame();
        }

        // Restart game
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        // Quit (Editor safe)
        if (isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void StartGame()
    {
        isGameRunning = true;
        isGameOver = false;
        gameSpeed = initialGameSpeed;
        FindObjectOfType<ScoreManager>()?.ResetScore();

    }

    public void GameOver()
    {
        isGameRunning = false;
        isGameOver = true;
        gameSpeed = 0f;
    }

    public void RestartGame()
    {
        // Reload current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetIdleState()
    {
        isGameRunning = false;
        isGameOver = false;
        gameSpeed = 0f;
    }

    public bool IsGameRunning => isGameRunning;
}
