using UnityEngine;

/// <summary>
/// Minimap system - Show player position on map
/// </summary>
public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float mapHeight = 50f;
    [SerializeField] private float mapZoom = 1f;

    private Camera minimapCamera;

    private void Start()
    {
        if (carTransform == null)
            carTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        minimapCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (carTransform == null) return;

        // Follow car position from above
        Vector3 newPosition = carTransform.position;
        newPosition.y = mapHeight;
        transform.position = newPosition;

        // Set orthographic size for zoom
        if (minimapCamera != null)
            minimapCamera.orthographicSize = mapZoom;
    }

    /// <summary>
    /// Set minimap zoom level
    /// </summary>
    public void SetZoom(float zoom)
    {
        mapZoom = Mathf.Clamp(zoom, 5f, 100f);
    }
}
