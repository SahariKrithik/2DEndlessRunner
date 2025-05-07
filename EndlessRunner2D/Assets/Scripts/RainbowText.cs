using TMPro;
using UnityEngine;

public class RainbowText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float speed = 1f; // Speed of the color change

    private float hue = 0f;

    void Update()
    {
        hue += speed * Time.deltaTime;
        if (hue > 1f) hue -= 1f;

        Color color = Color.HSVToRGB(hue, 1f, 1f);
        text.color = color;
    }
}
