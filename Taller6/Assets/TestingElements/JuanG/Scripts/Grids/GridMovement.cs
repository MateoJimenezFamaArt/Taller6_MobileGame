using UnityEngine;
using DG.Tweening;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public float moveDistance = 2f; // Distancia de movimiento en la cuadrícula
    public float jumpPower = 1.5f; // Altura del salto
    public Renderer playerRenderer; // Material del jugador para color
    private bool isOnBeat = false;
    WaitForSeconds wait = new WaitForSeconds(0.1f);

    void OnEnable()
    {
        EntradaMobile.Instance.OnSwipe += HandleSwipeInput;
        SingletonBeatManager.Instance.OnBeat += OnBeat;
        SingletonBeatManager.Instance.OutBeat += OutBeat;
    }
    void OnDisable()
    {
        EntradaMobile.Instance.OnSwipe -= HandleSwipeInput;
        SingletonBeatManager.Instance.OnBeat -= OnBeat;
        SingletonBeatManager.Instance.OutBeat -= OutBeat;
    }
    private void HandleSwipeInput(Vector2 direction)
    {

        Vector3 moveDirection = Vector3.zero;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            moveDirection = direction.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            moveDirection = direction.y > 0 ? Vector3.forward : Vector3.back;
        }
        if(moveDirection != Vector3.zero)
        {
            if (isOnBeat)
            {
                Vector3 fixedDistance = moveDirection * moveDistance;
                Vector3 targetPosition = transform.position + fixedDistance;
                StartCoroutine(JumpToPosition(targetPosition));
            }
            else
            {
                Debug.Log("No está en el beat");
            }
        }
    }
    void OnBeat()
    {
        isOnBeat = true;
        SetColor(Color.green);
    }
    private void OutBeat()
    {
        isOnBeat = false;
        SetColor(Color.red);
    }
    private IEnumerator JumpToPosition(Vector3 targetPosition)
    {
        targetPosition.y = transform.position.y;
        SetColor(Color.green);
        transform.DOJump(targetPosition, jumpPower, 1, 0.1f);
        yield return wait;
    }
    private void SetColor(Color color)
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = color;
        }
    }
}
