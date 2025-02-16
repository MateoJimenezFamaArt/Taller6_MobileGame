using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class EntradaMobile : MonoBehaviour
{
    public static EntradaMobile Instance { get; private set; }
    private TouchInput _touchInput;
    private Vector2 CurrentPos => _touchInput.Mobile.TouchPosition.ReadValue<Vector2>();
    private Vector2 initialPos;
    private float swipeThreshold = 100f;
    public delegate void Swipe(Vector2 direction);
    public event Swipe OnSwipe;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one instance of InputManager");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("InputManager created");
        _touchInput = new TouchInput();
        _touchInput.Mobile.TouchPress.performed += _ => { initialPos = CurrentPos; };
        _touchInput.Mobile.TouchPress.canceled += ctx => DetectSwipe();
    }

    private void OnEnable()
    {
        _touchInput.Mobile.Enable();
    }

    private void OnDisable()
    {
        _touchInput.Mobile.Disable();
    }


    public Vector2 GetTouchPosition()
    {
        return Touchscreen.current.primaryTouch.position.ReadValue();
    }

    public bool IsTouchPress()
    {
        Debug.Log("IsTouchPress");
        return _touchInput.Mobile.TouchPress.IsPressed();
    }

    public void DetectSwipe()
    {
        Vector2 delta = CurrentPos - initialPos;
        Vector2 swipeDirection = Vector2.zero;

        if (Mathf.Abs(delta.x) > swipeThreshold)
        {
            swipeDirection.x = Mathf.Clamp(delta.x, -1, 1);
        }
        if (Mathf.Abs(delta.y) > swipeThreshold)
        {
            swipeDirection.y = Mathf.Clamp(delta.y, -1, 1);
        }
        if(swipeDirection != Vector2.zero && OnSwipe != null)
        {
            OnSwipe?.Invoke(swipeDirection);
        }
    }
}
