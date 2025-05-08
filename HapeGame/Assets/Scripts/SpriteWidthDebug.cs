using UnityEngine;

public class SpriteWidthDebug : MonoBehaviour
{
    void Start()
    {
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log("Ground Sprite Width: " + width);
    }
}
