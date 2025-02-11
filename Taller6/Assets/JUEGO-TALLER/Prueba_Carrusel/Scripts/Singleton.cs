using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : Component
{
    private static T instancia;
    public static T Instanciar{
        get{
            if(instancia == null){
                var objs = FindObjectsByType<T>(FindObjectsSortMode.None);

                if (objs.Length > 0)
                    instancia = objs[0];
                
                if(objs.Length > 1)
                    Debug.LogError("Hay más de un " + typeof(T).Name + " en la escena.");
                
                if(instancia == null){
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instancia = obj.AddComponent<T>();
                }
            }

            return instancia;
        }
    }
}