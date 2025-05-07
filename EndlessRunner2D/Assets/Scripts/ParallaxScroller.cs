using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    public float scrollMultiplier = 0.3f;
    public float tileWidth = 20f;

    private Transform[] tiles;
    private float cameraWidth;

    void Start()
    {
        tiles = new Transform[transform.childCount];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = transform.GetChild(i);
        }

        // Calculate camera width in world units
        cameraWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameRunning)
            return;

        float scrollSpeed = GameManager.Instance.gameSpeed * scrollMultiplier;
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        foreach (Transform tile in tiles)
        {
            // Check if tile is far offscreen to the left
            if (tile.position.x + tileWidth < Camera.main.transform.position.x - cameraWidth / 2f)
            {
                float rightMostX = GetRightmostTileX();
                tile.position = new Vector3(rightMostX + tileWidth, tile.position.y, tile.position.z);
            }
        }
    }

    float GetRightmostTileX()
    {
        float maxX = float.MinValue;
        foreach (Transform tile in tiles)
        {
            if (tile.position.x > maxX)
                maxX = tile.position.x;
        }
        return maxX;
    }
}
