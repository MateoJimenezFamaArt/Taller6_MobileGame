using UnityEngine;
using TMPro; // Don't forget to import TextMesh Pro!

public class PointsCounter : MonoBehaviour
{
    public float pointsPerSecond = 10f; // Base points per second.
    private float currentPoints = 0f;
    private float beatMultiplier = 1f; // Multiplier for points if the player is on beat.
    private float beatSustainTime = 0f; // Time player is on beat.
    private float sustainThreshold = 10f; // Time required to sustain the beat for a multiplier.

    [SerializeField] private TextMeshProUGUI pointsText; // Reference to the TextMeshProUGUI for points display.
    [SerializeField] private TextMeshProUGUI multiplierText; // Reference to the TextMeshProUGUI for multiplier display.

    private void OnEnable()
    {
        // Subscribe to the OnBeat and OutBeat events from the SingletonBeatManager
        SingletonBeatManager.Instance.OnBeat += HandleOnBeat;
        SingletonBeatManager.Instance.OutBeat += HandleOutBeat;
    }

    private void OnDisable()
    {
        // Unsubscribe from the events when this object is disabled or destroyed
        SingletonBeatManager.Instance.OnBeat -= HandleOnBeat;
        SingletonBeatManager.Instance.OutBeat -= HandleOutBeat;
    }

    void Update()
    {
        // Increase points based on time survived (add per second)
        currentPoints += pointsPerSecond * beatMultiplier * Time.deltaTime;

        // Update the UI to display the current points and multiplier
        UpdateUI();
    }

    // Event handler for when the beat is hit
    private void HandleOnBeat()
    {
        beatSustainTime += SingletonBeatManager.Instance.GetBeatInterval();

        // If the player sustains the beat for 10 seconds, increase multiplier
        if (beatSustainTime >= sustainThreshold)
        {
            beatMultiplier += 0.5f; // Adjust multiplier as needed
            beatSustainTime = 0f; // Reset sustain time after reaching the threshold
            Debug.Log("Multiplier increased! Current multiplier: " + beatMultiplier);
        }
    }

    // Event handler for when the player is off-beat
    private void HandleOutBeat()
    {
        beatSustainTime = 0f; // Reset sustain time if the player misses the beat
        beatMultiplier = 1f;  // Reset the multiplier to base value
    }

    // Method to update the points and multiplier text in the UI
    private void UpdateUI()
    {
        // Make sure the text fields are set before updating them
        if (pointsText != null)
        {
            pointsText.text = "Points: " + Mathf.FloorToInt(currentPoints);
        }

        if (multiplierText != null)
        {
            multiplierText.text = "Multiplier: x" + beatMultiplier.ToString("F1"); // Shows multiplier with 1 decimal
        }
    }

    public float GetCurrentPoints()
    {
        return currentPoints;
    }
}
