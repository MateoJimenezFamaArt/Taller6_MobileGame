using TMPro;
using UnityEngine;

public class DeteccionSwipe : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField]
    float distanciaMinima = 0.2f;
    [SerializeField]
    float tiempoMaximo = 1.0f;
    [SerializeField]
    float offsetMaximoSwipe = .2f;

    private Vector2 posicionInicial;
    private Vector2 posicionFinal;
    private float tiempoInicial;
    private float tiempoFinal;

    private void Awake()
    {
        inputManager = InputManager.Instanciar;
    }

    private void OnEnable()
    {
        inputManager.EnInicioContacto += InicioSwipe;
        inputManager.EnTerminoContacto += FinSwipe;
    }

    private void OnDisable()
    {
        inputManager.EnInicioContacto -= InicioSwipe;
        inputManager.EnTerminoContacto -= FinSwipe;
    }

    private void InicioSwipe(Vector2 posicion, float tiempo)
    {
        posicionInicial = posicion;
        tiempoInicial = tiempo;
    }

    private void FinSwipe(Vector2 posicion, float tiempo)
    {
        posicionFinal = posicion;
        tiempoFinal = tiempo;
        DetectarSwipe();
    }

    private void DetectarSwipe()
    {
        if (Vector2.Distance(posicionFinal,posicionInicial) >= distanciaMinima &&
           (tiempoFinal-tiempoInicial) <= tiempoMaximo)
        {
            Vector2 direccion = posicionFinal - posicionInicial;
            direccion.Normalize();
            DireccionSwipe(direccion);
        }
    }

    private void DireccionSwipe(Vector2 direccion)
    {
        float dirX = direccion.x;
        float dirY = direccion.y;

        float diferencia = Mathf.Abs(dirX) - Mathf.Abs(dirY);

        if (dirX >= 0 && diferencia >= offsetMaximoSwipe)
        {
            inputManager.SwipeDerecha();
            Debug.Log("Derecha");
        }
        else if (dirX <= 0 && diferencia >= offsetMaximoSwipe)
        {
            inputManager.SwipeIzquierda();
        }
        else if (dirY >= 0 && diferencia <= -offsetMaximoSwipe)
        {
            inputManager.SwipeArriba();
        }
        else if (dirY <= 0 && diferencia <= -offsetMaximoSwipe)
        {
            inputManager.SwipeAbajo();
        }
    }
}
