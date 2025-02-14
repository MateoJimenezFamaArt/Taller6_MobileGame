using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class NewTest : MonoBehaviour
{
    private Vector2 direction;
    public void Jumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("Presionado JUMP");
        }
        
    }

    public void Moving(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();

        if (ctx.performed)
        {
            Debug.Log(direction);
        }
    }
}
