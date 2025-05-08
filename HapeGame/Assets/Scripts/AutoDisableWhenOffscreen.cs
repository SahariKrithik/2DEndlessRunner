using UnityEngine;

public class AutoDisableWhenOffscreen : MonoBehaviour
{
    public float despawnX = -20f; // X position threshold to disable

    void Update()
    {
        if (transform.position.x < despawnX)
        {
            gameObject.SetActive(false);
        }
    }
}
