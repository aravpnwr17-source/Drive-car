using UnityEngine;

/// <summary>
/// Speedometer and performance tracker
/// </summary>
public class PerformanceTracker : MonoBehaviour
{
    [SerializeField] private CarController carController;

    private float maxSpeedReached = 0f;
    private float totalDistanceTraveled = 0f;
    private float totalTimeInGame = 0f;
    private int collisionsCount = 0;
    private Vector3 lastPosition;

    private void Start()
    {
        if (carController == null)
            carController = GetComponent<CarController>();

        lastPosition = transform.position;
    }

    private void Update()
    {
        if (carController == null) return;

        totalTimeInGame += Time.deltaTime;

        // Track max speed
        float currentSpeed = carController.GetCurrentSpeed();
        if (currentSpeed > maxSpeedReached)
            maxSpeedReached = currentSpeed;

        // Track distance traveled
        float distance = Vector3.Distance(transform.position, lastPosition);
        totalDistanceTraveled += distance;
        lastPosition = transform.position;
    }

    /// <summary>
    /// Record collision
    /// </summary>
    public void RecordCollision()
    {
        collisionsCount++;
    }

    /// <summary>
    /// Get performance stats
    /// </summary>
    public void DisplayStats()
    {
        Debug.Log($"=== Performance Stats ===");
        Debug.Log($"Max Speed: {maxSpeedReached:F2} km/h");
        Debug.Log($"Distance: {totalDistanceTraveled:F2}m");
        Debug.Log($"Time: {totalTimeInGame:F2}s");
        Debug.Log($"Collisions: {collisionsCount}");
    }
}
