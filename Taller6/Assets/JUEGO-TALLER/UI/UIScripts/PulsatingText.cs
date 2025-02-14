using UnityEngine;
using TMPro;

public class PulsatingText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float pulseSpeed = 1.5f; // Speed of the pulse
    public float pulseStrength = 0.2f; // How much the text scales
    private Vector3 originalScale;

    void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }
        originalScale = textMeshPro.transform.localScale;
    }

    void Update()
    {
        float scaleFactor = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseStrength;
        textMeshPro.transform.localScale = originalScale * scaleFactor;
    }
}
