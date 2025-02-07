using System.Collections;
using UnityEngine;

public class ItemGrow : MonoBehaviour
{
    private float beatInterval;
    private int beatCount = 0;
    private System.Action<GameObject> returnToPool;

    public void Initialize(System.Action<GameObject> returnToPoolAction, float interval)
    {
        returnToPool = returnToPoolAction;
        beatInterval = interval;
        beatCount = 0;
        transform.localScale = Vector3.one;
        StartCoroutine(GrowAndReturn());
    }

    private IEnumerator GrowAndReturn()
    {
        while (beatCount < 3)
        {
            yield return new WaitForSeconds(beatInterval);
            transform.localScale += Vector3.one;
            beatCount++;
        }

        returnToPool?.Invoke(gameObject);
    }
}