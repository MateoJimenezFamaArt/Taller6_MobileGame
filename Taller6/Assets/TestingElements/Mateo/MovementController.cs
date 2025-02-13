using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float moveDistance = 2f; // Distance per movement step
    private bool isMoving = false; // Prevents overlapping movements

    void Update()
    {
        if (!isMoving)
        {
            Vector3 targetPosition = transform.position;

            if (Input.GetKeyDown(KeyCode.W))
                targetPosition += new Vector3(0, 0, moveDistance);
            else if (Input.GetKeyDown(KeyCode.S))
                targetPosition += new Vector3(0, 0, -moveDistance);
            else if (Input.GetKeyDown(KeyCode.A))
                targetPosition += new Vector3(-moveDistance, 0, 0);
            else if (Input.GetKeyDown(KeyCode.D))
                targetPosition += new Vector3(moveDistance, 0, 0);

            if (targetPosition != transform.position)
                StartCoroutine(MoveToPosition(targetPosition));
        }
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        float elapsedTime = 0f;
        float moveDuration = 0.2f; // Adjust movement speed
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
}