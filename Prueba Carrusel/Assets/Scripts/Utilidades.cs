using UnityEngine;

namespace Assets.Scripts
{
    public class Utilidades : MonoBehaviour
    {
        public static Vector3 PantallaAMundo(Camera camara, Vector3 posicion)  
        {
            posicion.z = camara.nearClipPlane;
            return camara.ScreenToWorldPoint(posicion);
        }
    }
}