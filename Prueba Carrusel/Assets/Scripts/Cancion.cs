using DG.Tweening;
using TMPro;
using UnityEngine;

public class Cancion : MonoBehaviour
{
    private InputManager inputManager;
    private CarruselManager carruselManager;

    [SerializeField]
    private float separacion; // Debería estar un en manager general para no tener que revisar cada prefab, tal vez¿?
    [SerializeField] private float tiempoSwipe;

    [SerializeField]
    TextMeshProUGUI nombreCancion, dificultadCancion;
    private RectTransform rectTransform;

    public bool bloquearIzquierda, bloquearDerecha;

    void Awake()
    {
        inputManager = InputManager.Instanciar;
        carruselManager = CarruselManager.Instanciar;
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform.position.x == 0) carruselManager.nivelActual = this;
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
        if (!bloquearIzquierda) rectTransform.DOAnchorPosX(rectTransform.localPosition.x - separacion, tiempoSwipe);
    }

    private void SwipeDerecha()
    {
        if (!bloquearDerecha) rectTransform.DOAnchorPosX(rectTransform.localPosition.x + separacion, tiempoSwipe);
    }
}
