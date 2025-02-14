using UnityEngine;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public float moveDistance = 2f; // Step distance in grid
    public float moveDuration = 0.2f; // Smooth movement duration
    public Renderer playerRenderer; // Player's material for color feedback

    private bool isMoving = false;
    private bool attemptedWrongMove = false; // Flag for incorrect move attempts
    private Vector3 queuedMove; // Stores a move attempt to execute on next beat
    private bool hasQueuedMove = false; // If the player tried moving off-beat
    [SerializeField] private bool isTouch = true;

    void OnEnable()
    {
        // Subscribe to beat event from BeatManager
        BeatManager.OnBeat += OnBeat;
    }

    void Start()
    {
        if (playerRenderer == null)
        {
            Debug.LogError("GridMovement: No Renderer assigned for color feedback!");
            return;
        }

        SetColor(Color.yellow); // Start in idle mode
    }

    void Update()
    {
        if (!isTouch)
        {
            if (isMoving) return;

            Vector3 moveDirection = Vector3.zero;
            moveDirection = EntradaMobile.Instance.GetTouchMovement();

            if (moveDirection != Vector3.zero)
            {
                if (isMoving) return; // Ignore movement while already moving

                hasQueuedMove = true;
                queuedMove = moveDirection;
            }
        }
        else
        {
            MovePlayerTouch();
        }
    }


    public void MovePlayerTouch()
    {
        if (isMoving) return;

        Vector3 moveDirection = Vector3.zero;
        moveDirection = EntradaMobile.Instance.GetTouchMovement();

        moveDirection.Normalize();

        if (moveDirection != Vector3.zero)
        {
            if (isMoving) return; // Ignore movement while already moving

            hasQueuedMove = true;
            queuedMove = new Vector3(moveDirection.x * moveDistance, 0, moveDirection.y * moveDistance);
        }
    }

    void OnBeat()
    {
        if (isMoving) return;

        if (hasQueuedMove)
        {
            // Execute stored movement
            StartCoroutine(MoveToPosition(transform.position + queuedMove));
            hasQueuedMove = false;
        }
        else
        {
            // If player is idle, set yellow
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
        // Unsubscribe from the beat event to prevent memory leaks
        BeatManager.OnBeat -= OnBeat;
    }
}
