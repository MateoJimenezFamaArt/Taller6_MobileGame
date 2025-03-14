using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class EntradaMobile : MonoBehaviour
{
    public static EntradaMobile Instance { get; private set; }
    private TouchInput _touchInput;
    private Vector2 currentPos => _touchInput.Mobile.TouchPosition.ReadValue<Vector2>();
    private Vector2 initialPos;
    private readonly float swipeThreshold = 100f;
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
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    private void OnEnable()
    {
        _touchInput = new TouchInput();
        _touchInput.Mobile.Enable();
        _touchInput.Mobile.TouchPress.started += ctx => SetInitialPos(ctx.ReadValue<Vector2>());
        _touchInput.Mobile.TouchPress.canceled += _ => DetectSwipe();
    }
   private void OnDisable()
    {
        _touchInput.Mobile.Disable();
    }
    private void SetInitialPos(Vector2 touchInput)
    {
        initialPos = touchInput;
    }
    public void DetectSwipe()
    {
        Vector2 delta = currentPos - initialPos;
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
