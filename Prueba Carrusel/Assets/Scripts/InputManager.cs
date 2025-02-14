using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private InputControls controlesInput;

    #region Eventos
    public delegate void EventoInicioContacto(Vector2 position, float tiempo);
    public event EventoInicioContacto EnInicioContacto;
    public delegate void EventoTerminoContacto(Vector2 position, float tiempo);
    public event EventoTerminoContacto EnTerminoContacto;
    public delegate void EventoSwipeArriba();
    public event EventoSwipeArriba EnSwipeArriba;
    public delegate void EventoSwipeAbajo();
    public event EventoSwipeAbajo EnSwipeAbajo;
    public delegate void EventoSwipeDerecha();
    public event EventoSwipeDerecha EnSwipeDerecha;
    public delegate void EventoSwipeIzquierda();
    public event EventoSwipeIzquierda EnSwipeIzquierda;
    #endregion

    Camera camaraPrincipal;

    private void Awake()
    {
        controlesInput = new InputControls();
        camaraPrincipal = Camera.main;
    }
    private void OnEnable()
    {
        controlesInput.Enable();
    }
    private void OnDisable()
    {
        controlesInput.Disable();
    }
    void Start()
    {
        controlesInput.Tocar.ContactoPrimario.started += ctx => InicioContacto(ctx);
        controlesInput.Tocar.ContactoPrimario.canceled += ctx => TerminoContacto(ctx);
    }

    private void InicioContacto(InputAction.CallbackContext contexto)
    {
        if (EnInicioContacto != null) EnInicioContacto(posicionPrimaria(), (float)contexto.startTime);
    }

    private void TerminoContacto(InputAction.CallbackContext contexto)
    {
        if(EnTerminoContacto != null) EnTerminoContacto(posicionPrimaria(), (float)contexto.time);
    }

    #region funciones para llamar eventos
    public void SwipeArriba()
    {
        if(EnSwipeArriba != null) EnSwipeArriba();
    }

    public void SwipeAbajo()
    {
        if (EnSwipeAbajo != null) EnSwipeAbajo();
    }

    public void SwipeDerecha()
    {
        if (EnSwipeDerecha != null) EnSwipeDerecha();
    }

    public void SwipeIzquierda()
    {
        if (EnSwipeIzquierda != null) EnSwipeIzquierda();
    }
    #endregion

    public Vector2 posicionPrimaria()
    {
        return Utilidades.PantallaAMundo(camaraPrincipal, controlesInput.Tocar.PosiciónContacto.ReadValue<Vector2>());
    }
}
