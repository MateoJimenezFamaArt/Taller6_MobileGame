using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Carrusel : MonoBehaviour
{
    InputManager inputManager;

    [SerializeField] RectTransform[] objetosCarrusel;
    [SerializeField] Carrusel carruselCanciones;
    RectTransform objetoActual;

    [SerializeField] float separacion;
    [SerializeField] float tiempoSwipe;
    [SerializeField] bool esVertical;

    bool estaEnPrimero;
    bool estaEnUltimo;

    private void Awake()
    {
        inputManager = InputManager.Instanciar;

        objetoActual = objetosCarrusel[0];
        carruselCanciones.enabled = false;

        estaEnPrimero = true;
        estaEnUltimo = false;
    }

    private void OnEnable()
    {
        inputManager.EnSwipeIzquierda += SwipeIzquierda;
        inputManager.EnSwipeDerecha += SwipeDerecha;
        inputManager.EnSwipeArriba += SwipeArriba;
        inputManager.EnSwipeAbajo += SwipeAbajo;
    }

    private void OnDisable()
    {
        inputManager.EnSwipeIzquierda -= SwipeIzquierda;
        inputManager.EnSwipeDerecha -= SwipeDerecha;
        inputManager.EnSwipeArriba -= SwipeArriba;
        inputManager.EnSwipeAbajo -= SwipeAbajo;
    }

    void SwipeIzquierda()
    {
        if (esVertical || estaEnUltimo) return;
        estaEnPrimero = false;

        foreach (RectTransform objeto in objetosCarrusel)
        {
            objeto.DOAnchorPosX(objeto.localPosition.x - separacion, tiempoSwipe);
        }

        objetoActual = objetosCarrusel[Array.IndexOf(objetosCarrusel, objetoActual) + 1];
        if (objetoActual == objetosCarrusel[^1]) estaEnUltimo = true;
    }

    void SwipeDerecha()
    {
        if(esVertical || estaEnPrimero) return;
        estaEnUltimo = false;

        foreach(RectTransform objeto in objetosCarrusel)
        {
            objeto.DOAnchorPosX(objeto.localPosition.x + separacion, tiempoSwipe);
        }

        objetoActual = objetosCarrusel[Array.IndexOf(objetosCarrusel, objetoActual) - 1];
        if (objetoActual == objetosCarrusel[0]) estaEnPrimero = true;
    }

    void SwipeArriba()
    {
        if (!esVertical || estaEnUltimo) return;
        estaEnPrimero = false;

        foreach (RectTransform objeto in objetosCarrusel)
        {
            objeto.DOAnchorPosY(objeto.localPosition.y + separacion, tiempoSwipe, true);
        }

        objetoActual = objetosCarrusel[Array.IndexOf(objetosCarrusel, objetoActual) + 1];

        if (objetoActual == carruselCanciones.gameObject.GetComponent<RectTransform>()) carruselCanciones.enabled = true;
        else carruselCanciones.enabled = false;

        if (objetoActual == objetosCarrusel[^1]) estaEnUltimo = true;
    }

    void SwipeAbajo()
    {
        if (!esVertical || estaEnPrimero) return;
        estaEnUltimo = false;

        foreach (RectTransform objeto in objetosCarrusel)
        {
            objeto.DOAnchorPosY(objeto.localPosition.y - separacion, tiempoSwipe);
        }

        objetoActual = objetosCarrusel[Array.IndexOf(objetosCarrusel, objetoActual) - 1];

        if (objetoActual == carruselCanciones.gameObject.GetComponent<RectTransform>()) carruselCanciones.enabled = true;
        else carruselCanciones.enabled = false;

        if (objetoActual == objetosCarrusel[0]) estaEnPrimero = true;
    }
}


