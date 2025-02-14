using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EntradaMobile : MonoBehaviour
{
    public static EntradaMobile Instance { get; private set; }
    private TouchInput _touchInput;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

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
        _touchInput.Mobile.TouchPress.started += ctx => startTouchPosition = GetTouchPosition();
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

    public Vector2 GetTouchMovement()
    {
        return _touchInput.Mobile.TouchMovement.ReadValue<Vector2>();
    }

    public void Swipe()
    {
        Vector2 direcion = _touchInput.Mobile.TouchMovement.ReadValue<Vector2>();
        Debug.Log(direcion);
        endTouchPosition = GetTouchPosition();
        Vector2 direction = endTouchPosition - startTouchPosition;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                Debug.Log("Swipe Right");
            }
            else
            {
                Debug.Log("Swipe Left");
            }
        }
        else
        {
            if (direction.y > 0)
            {
                Debug.Log("Swipe Up");
            }
            else
            {
                Debug.Log("Swipe Down");
            }
        }

    }
}
