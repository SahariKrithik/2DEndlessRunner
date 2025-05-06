using UnityEngine;

public class BirdAnimatorScaler : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameRunning)
        {
            float speedFactor = GameManager.Instance.gameSpeed / GameManager.Instance.initialGameSpeed;
            animator.speed = 0.5f * speedFactor; // Adjust as needed
        }
    }
}
