using System.Collections;
using TMPro;
using UnityEngine;

public class FlashingText : MonoBehaviour
{
    public TMP_Text flashingText;
    public float fadeDuration = 1.0f; // Time for a full fade cycle

    private void Start()
    {
        if (flashingText == null)
        {
            flashingText = GetComponent<TMP_Text>();
        }
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(0f, 1f)); // Fade in
            yield return StartCoroutine(Fade(1f, 0f)); // Fade out
        }
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        Color textColor = flashingText.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            flashingText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }

        // Ensure exact final alpha value
        flashingText.color = new Color(textColor.r, textColor.g, textColor.b, targetAlpha);
    }
}
