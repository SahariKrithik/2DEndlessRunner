using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float destroyX = -20f;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameRunning)
        {
            transform.position += Vector3.left * GameManager.Instance.gameSpeed * Time.deltaTime;

            if (transform.position.x < destroyX)
            {
                Destroy(gameObject);
            }
        }
    }
}
