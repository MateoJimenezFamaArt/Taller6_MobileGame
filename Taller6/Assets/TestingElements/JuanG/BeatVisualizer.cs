using UnityEngine;
using UnityEngine.UI;

public class BeatVisualizerUI : MonoBehaviour
{
    public RectTransform spriteCentral; // El que crece y cambia de color
    public RectTransform spriteIzquierdo, spriteDerecho; // Los que se mueven hacia el centro
    public Image centralImage; // Para cambiar el color del sprite central
    public Color beatColor = Color.red; // Color cuando hay beat
    public Color normalColor = Color.white; // Color normal
    public float beatScale = 1.5f; // Cuánto crece el sprite central
    public float moveDistance = 370f; // Distancia de los sprites laterales
    public float animationSpeed = 5f; // Velocidad de interpolación

    private Vector2 leftStartPos, rightStartPos;
    private Vector2 leftEndPos, rightEndPos;
    private Vector3 initialScale;

    private void Start()
    {
        if (spriteCentral == null || spriteIzquierdo == null || spriteDerecho == null || centralImage == null)
        {
            Debug.LogError("BeatVisualizerUI: No se han asignado todos los elementos.");
            return;
        }

        // Configurar posiciones iniciales y finales
        initialScale = spriteCentral.localScale;
        leftStartPos = new Vector2(-moveDistance, -200);
        rightStartPos = new Vector2(moveDistance, -200);
        leftEndPos = new Vector2(0, -200);
        rightEndPos = new Vector2(0, -200);

        // Asignar posiciones iniciales
        spriteIzquierdo.anchoredPosition = leftStartPos;
        spriteDerecho.anchoredPosition = rightStartPos;
    }

    private void OnEnable()
    {
        BeatManager.OnBeat += OnBeat;
    }

    private void OnDisable()
    {
        BeatManager.OnBeat -= OnBeat;
    }

    private void Update()
    {
        if (BeatManager.IsOnBeat()) return;
        // Volver al tamaño normal
        spriteCentral.localScale = Vector3.Lerp(spriteCentral.localScale, initialScale, animationSpeed * Time.deltaTime);

        // Volver al color normal
        centralImage.color = Color.Lerp(centralImage.color, normalColor, animationSpeed * Time.deltaTime);

        // Regresar sprites laterales a su posición original
        spriteIzquierdo.anchoredPosition = Vector2.Lerp(spriteIzquierdo.anchoredPosition, leftStartPos, animationSpeed * Time.deltaTime);
        spriteDerecho.anchoredPosition = Vector2.Lerp(spriteDerecho.anchoredPosition, rightStartPos, animationSpeed * Time.deltaTime);
    }

    private void OnBeat()
    {
        // Cambiar color
        centralImage.color = beatColor;

        // Hacer crecer el sprite central
        spriteCentral.localScale = initialScale * beatScale;

        // Mover sprites laterales al centro
        spriteIzquierdo.anchoredPosition = leftEndPos;
        spriteDerecho.anchoredPosition = rightEndPos;
    }
}
