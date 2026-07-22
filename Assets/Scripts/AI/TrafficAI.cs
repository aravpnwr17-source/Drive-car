using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Manages AI traffic vehicle behavior
/// </summary>
public class TrafficAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float stoppingDistance = 5f;
    [SerializeField] private float acceleration = 10f;

    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int currentWaypointIndex = 0;

    [Header("Awareness")]
    [SerializeField] private float detectionRadius = 20f;
    [SerializeField] private float brakingDistance = 10f;

    private Rigidbody vehicleRigidbody;
    private float currentSpeed = 0f;
    private bool shouldBrake = false;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        vehicleRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        
        if (navMeshAgent != null && waypoints.Length > 0)
        {
            navMeshAgent.SetDestination(waypoints[0].position);
        }
    }

    private void Update()
    {
        CheckForObstacles();
        UpdateMovement();
    }

    /// <summary>
    /// Check for obstacles and vehicles ahead
    /// </summary>
    private void CheckForObstacles()
    {
        shouldBrake = false;

        // Cast ray ahead to detect obstacles
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, brakingDistance))
        {
            if (hit.collider.CompareTag("Vehicle") || hit.collider.CompareTag("Obstacle"))
            {
                shouldBrake = true;
            }
        }

        // Check for other traffic vehicles
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Vehicle") && collider.gameObject != gameObject)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < brakingDistance)
                {
                    shouldBrake = true;
                }
            }
        }
    }

    /// <summary>
    /// Update vehicle movement
    /// </summary>
    private void UpdateMovement()
    {
        if (waypoints.Length == 0) return;

        Transform currentWaypoint = waypoints[currentWaypointIndex];
        float distanceToWaypoint = Vector3.Distance(transform.position, currentWaypoint.position);

        // Check if reached waypoint
        if (distanceToWaypoint < stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        // Calculate desired speed
        float desiredSpeed = shouldBrake ? 0f : maxSpeed;

        // Smoothly adjust speed
        currentSpeed = Mathf.Lerp(currentSpeed, desiredSpeed, Time.deltaTime * acceleration);

        // Move towards waypoint
        Vector3 directionToWaypoint = (currentWaypoint.position - transform.position).normalized;
        vehicleRigidbody.velocity = directionToWaypoint * (currentSpeed / 3.6f);

        // Rotate towards waypoint
        if (currentSpeed > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    /// <summary>
    /// Set waypoints for this traffic vehicle
    /// </summary>
    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
    }

    /// <summary>
    /// Get current speed
    /// </summary>
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
