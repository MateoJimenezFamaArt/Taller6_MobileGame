using NUnit.Framework;
using System;
using UnityEngine;

public class CarruselManager : Singleton<CarruselManager>
{
    private InputManager inputManager;

    [SerializeField] private Cancion[] niveles;
    public Cancion nivelActual;

    void Awake()
    {
        inputManager = InputManager.Instanciar;
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
        if (nivelActual == niveles[niveles.Length - 1])
        {
            foreach(Cancion nivel in niveles)
            {
                nivel.bloquearIzquierda = true;
            }
        }
        else if (nivelActual.bloquearDerecha)
        {
            foreach (Cancion nivel in niveles)
            {
                nivel.bloquearDerecha = false;
            }
        }

        if(!nivelActual.bloquearIzquierda) nivelActual = niveles[Array.IndexOf(niveles, nivelActual) + 1];
    }

    private void SwipeDerecha()
    {
        if (nivelActual == niveles[0])
        {
            foreach (Cancion nivel in niveles)
            {
                nivel.bloquearDerecha = true;
            }
        }
        else if (nivelActual.bloquearIzquierda)
        {
            foreach (Cancion nivel in niveles)
            {
                nivel.bloquearIzquierda = false;
            }
        }

        if(!nivelActual.bloquearDerecha) nivelActual = niveles[Array.IndexOf(niveles, nivelActual) - 1];
    }
}
