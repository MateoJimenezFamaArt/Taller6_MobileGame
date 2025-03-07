using UnityEngine;
using DG.Tweening;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public float moveDistance = 2f; // Distancia de movimiento en la cuadr�cula
    public float moveDuration = 0.2f; // Duraci�n del movimiento
    public float jumpPower = 1.5f; // Altura del salto
    public Renderer playerRenderer; // Material del jugador para color
    private bool isMoving = false;
    private Vector3 queuedMove;
    private bool hasQueuedMove = false;

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
        if (isMoving) return; // Ignorar input si ya est� movi�ndose

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
            if (isMoving) return;
            queuedMove = moveDirection * moveDistance;
            hasQueuedMove = true;
        }
        
        SetColor(Color.red); // Indicar que hay un movimiento en espera
    }

    void OnBeat()
    {
        if (isMoving) return;

        if (hasQueuedMove)
        {
            Vector3 targetPosition = transform.position + queuedMove;
            hasQueuedMove = false;
            StartCoroutine(JumpToPosition(targetPosition));
        }
        else
        {
            SetColor(Color.yellow);
        }
    }

    private void OutBeat()
    {
        SetColor(Color.white);
    }

    private IEnumerator JumpToPosition(Vector3 targetPosition)
    {
        targetPosition.y = transform.position.y; // Mantener la misma altura
        isMoving = true;
        SetColor(Color.green); // Indicar que el jugador se est� moviendo
        transform.DOJump(targetPosition, jumpPower, 1, 0.1f)
            .OnComplete(() => isMoving = false); // Volver a permitir movimiento al finalizar el salto

        yield return new WaitForSeconds(moveDuration);
    }

    private void SetColor(Color color)
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = color;
        }
    }
}
