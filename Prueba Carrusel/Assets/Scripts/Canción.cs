using DG.Tweening;
using TMPro;
using UnityEngine;

public class Canción : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField]
    private float separacion; // Debería estar un en manager general para no tener que revisar cada prefab, tal vez¿?

    [SerializeField]
    TextMeshProUGUI nombreCancion, dificultadCancion;
    private RectTransform rectTransform;

    void Awake()
    {
        inputManager = InputManager.Instanciar;
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        inputManager.EnSwipeIzquierda += SwipeIzquierda;
        inputManager.EnSwipeDerecha += SwipeDerecha;
    }

    private void OnDisable()
    {
        inputManager.EnSwipeIzquierda -= SwipeIzquierda;
        inputManager.EnSwipeDerecha -= SwipeDerecha;
    }

    private void SwipeIzquierda()
    {
        Debug.Log("Me muevo a la izq");
        rectTransform.DOAnchorPosX(rectTransform.position.x - separacion, 1.0f);
    }

    private void SwipeDerecha()
    {
        Debug.Log("Me muevo a la der");
        rectTransform.DOAnchorPosX(rectTransform.position.x + separacion, 1.0f);
    }
}
