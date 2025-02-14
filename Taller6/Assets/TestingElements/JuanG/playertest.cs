using UnityEngine;

public class playertest : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        
        Vector2 direccion = EntradaMobile.Instance.GetTouchMovement();
    }
}
