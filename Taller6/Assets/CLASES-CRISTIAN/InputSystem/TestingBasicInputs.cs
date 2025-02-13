using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class TestingBasicInputs : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Left Mouse Button was pressed");
        }

        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            Debug.Log("Space Key was released");
        }
    }
}
