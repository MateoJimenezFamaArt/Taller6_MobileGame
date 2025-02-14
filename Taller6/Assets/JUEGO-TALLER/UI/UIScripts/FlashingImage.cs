using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashingImage : MonoBehaviour
{
    public Image flashingImage;
    public float fadeDuration = 1.0f; // Time for a full fade cycle

    private void Start()
    {
        if (flashingImage == null)
        {
            flashingImage = GetComponent<Image>();
        }
        StartCoroutine(FadeImage());
    }

    private IEnumerator FadeImage()
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
        Color imageColor = flashingImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            flashingImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
            yield return null;
        }

        // Ensure exact final alpha value
        flashingImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, targetAlpha);
    }
}
