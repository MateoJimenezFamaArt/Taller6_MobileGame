using UnityEngine;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public float moveDistance = 2f; // Step distance in grid
    public float moveDuration = 1f; // Smooth movement duration
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
        EntradaMobile.Instance.OnSwipe += context => { StartCoroutine(MovePlayerTouch(context)); };
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

   private IEnumerator MovePlayerTouch(Vector2 direction)
    {
        Vector3 moveDirection = Vector3.zero;
        Debug.Log("direction" + direction);
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            moveDirection = direction.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            moveDirection = direction.y > 0 ? Vector3.forward : Vector3.back; // Cambio de up/down a forward/back para un movimiento más estándar en 3D.
        }

        Vector3 targetPosition = transform.position + moveDirection * moveDistance;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
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
        hasQueuedMove = false;
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
