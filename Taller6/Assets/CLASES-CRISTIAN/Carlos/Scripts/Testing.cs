using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Testing : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("Presionado");
        }

        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            Debug.Log("EspacioSoltado");
        }
    }
}
