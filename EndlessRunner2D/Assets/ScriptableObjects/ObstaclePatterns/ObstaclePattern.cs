using UnityEngine;

[CreateAssetMenu(fileName = "ObstaclePattern", menuName = "EndlessRunner/Obstacle Pattern")]
public class ObstaclePattern : ScriptableObject
{
    public GameObject[] sequence;       // Prefabs to spawn in order
    public float spacing = 3f;          // Spacing between elements
}
