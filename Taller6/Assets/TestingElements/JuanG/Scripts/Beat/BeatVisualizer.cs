using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using DG.Tweening;

public class BeatVisualizerUI : MonoBehaviour
{
    public RectTransform spriteCentral;
    public Image centralImage;
    public Color beatColor = Color.red;
    public Color normalColor = Color.white;
    public float beatScale = 1.5f;
    public float animationSpeed = 5f;
    private Vector3 initialScale;

    private void Start()
    {
        if (spriteCentral == null || centralImage == null)
        {
            Debug.LogError("BeatVisualizerUI: No se han asignado todos los elementos.");
            return;
        }
        initialScale = spriteCentral.localScale;
    }
    private void OnEnable()
    {
        SingletonBeatManager.Instance.OnBeat += OnBeat;
        SingletonBeatManager.Instance.OutBeat += OutBeat;
    }
    private void OnDisable()
    {
        SingletonBeatManager.Instance.OnBeat -= OnBeat;
        SingletonBeatManager.Instance.OutBeat -= OutBeat;
    }
    private void OnBeat()
    {
        centralImage.color = beatColor;
        spriteCentral.localScale = initialScale * beatScale;
    }

    private void OutBeat()
    {
        spriteCentral.localScale = Vector3.Lerp(spriteCentral.localScale, initialScale, animationSpeed * Time.deltaTime);
        centralImage.color = Color.Lerp(centralImage.color, normalColor, animationSpeed * Time.deltaTime);
    }
    
}
