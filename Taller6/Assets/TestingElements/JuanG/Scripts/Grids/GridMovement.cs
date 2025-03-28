using UnityEngine;
using DG.Tweening;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public float moveDistance = 2f; // Distancia de movimiento en la cuadrícula
    public float jumpPower = 1.5f; // Altura del salto
    [SerializeField] private Animator playerAnimator;
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
                playerAnimator.SetTrigger("isHitting");
            }
        }
    }
    void OnBeat()
    {
        isOnBeat = true;
    }
    private void OutBeat()
    {
        isOnBeat = false;
    }
    private IEnumerator JumpToPosition(Vector3 targetPosition)
    {
        playerAnimator.SetTrigger("isJumping");
        targetPosition.y = transform.position.y;
        transform.DOJump(targetPosition, jumpPower, 1, 0.5f);
        yield return wait;
    }
}
