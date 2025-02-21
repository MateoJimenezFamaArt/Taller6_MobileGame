using UnityEngine;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public float moveDistance = 2f; // Step distance in grid
    public float moveDuration = 1f; // Smooth movement duration
    public Renderer playerRenderer; // Player's material for color feedback

    private bool isMoving = false;
    private bool hasQueuedMove = false; // If the player tried moving off-beat
    private Vector3 queuedMove = Vector3.zero; // Stores movement direction

    void OnEnable()
    {
        // Subscribe to beat event from BeatManager
        BeatManager.OnBeat += OnBeat;
        EntradaMobile.Instance.OnSwipe += HandleSwipeInput;
    }

    void Start()
    {
        if (playerRenderer == null)
        {
            Debug.LogError("GridMovement: No Renderer assigned for color feedback!");
            return;
        }
        SetColor(Color.yellow);
    }

    private void HandleSwipeInput(Vector2 direction)
    {
        if (isMoving) return; // Ignore input if currently moving

        Vector3 moveDirection = Vector3.zero;
        Debug.Log("Swipe Direction: " + direction);

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            moveDirection = direction.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            moveDirection = direction.y > 0 ? Vector3.forward : Vector3.back;
        }

        queuedMove = moveDirection * moveDistance;
        hasQueuedMove = true; // Store move for next beat
        SetColor(Color.red); // Indicate that movement is queued
    }

    void OnBeat()
    {
        if (isMoving) return;

        if (hasQueuedMove)
        {
            // Execute stored movement on beat
            StartCoroutine(MoveToPosition(transform.position + queuedMove));
            hasQueuedMove = false;
        }
        else
        {
            // If no movement is queued, set yellow
            SetColor(Color.yellow);
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        SetColor(Color.green); // Successful beat movement

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }

    private void SetColor(Color color)
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = color;
        }
    }

    void OnDisable()
    {
        BeatManager.OnBeat -= OnBeat;
    }
}
