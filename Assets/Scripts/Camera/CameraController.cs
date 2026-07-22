using UnityEngine;

/// <summary>
/// Player camera controller - Third person follow camera
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float distance = 10f;
    [SerializeField] private float height = 5f;
    [SerializeField] private float smoothness = 5f;
    [SerializeField] private float rotationSpeed = 2f;

    private Vector3 targetPosition;
    private float currentYaw = 0f;

    private void Start()
    {
        if (carTransform == null)
            carTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void LateUpdate()
    {
        if (carTransform == null) return;

        // Mouse input for camera rotation
        float mouseX = Input.GetAxis("Mouse X");
        currentYaw += mouseX * rotationSpeed;

        // Calculate camera position behind the car
        Vector3 carPosition = carTransform.position;
        Vector3 carForward = carTransform.forward;
        Vector3 carRight = carTransform.right;

        // Position behind and above the car
        targetPosition = carPosition - carForward * distance + Vector3.up * height;

        // Smoothly move camera to target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothness);

        // Look at a point slightly ahead of the car
        Vector3 lookTarget = carPosition + carForward * 5f + Vector3.up * 2f;
        transform.LookAt(lookTarget);
    }

    /// <summary>
    /// Set camera distance from car
    /// </summary>
    public void SetDistance(float newDistance)
    {
        distance = newDistance;
    }

    /// <summary>
    /// Set camera height above car
    /// </summary>
    public void SetHeight(float newHeight)
    {
        height = newHeight;
    }
}
