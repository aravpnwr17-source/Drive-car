using UnityEngine;

/// <summary>
/// Manages vehicle customization and configuration
/// </summary>
public class VehicleCustomizer : MonoBehaviour
{
    [Header("Paint")]
    [SerializeField] private Material paintMaterial;
    [SerializeField] private Color[] availableColors = new Color[10];

    [Header("Wheels")]
    [SerializeField] private Mesh[] wheelModels = new Mesh[10];
    [SerializeField] private Material wheelMaterial;

    [Header("Body")]
    [SerializeField] private Mesh[] bodyKits = new Mesh[5];
    [SerializeField] private GameObject[] spoilers = new GameObject[5];

    [Header("Performance")]
    [SerializeField] private CarController carController;

    private int currentPaintColorIndex = 0;
    private int currentWheelIndex = 0;
    private int currentBodyKitIndex = 0;
    private int currentSpoilerIndex = 0;

    /// <summary>
    /// Apply paint color to vehicle
    /// </summary>
    public void SetPaintColor(int colorIndex)
    {
        if (colorIndex < 0 || colorIndex >= availableColors.Length) return;

        currentPaintColorIndex = colorIndex;
        
        if (paintMaterial != null)
        {
            paintMaterial.color = availableColors[colorIndex];
        }

        Debug.Log($"Paint color changed to index: {colorIndex}");
    }

    /// <summary>
    /// Change wheel style
    /// </summary>
    public void SetWheelStyle(int wheelIndex)
    {
        if (wheelIndex < 0 || wheelIndex >= wheelModels.Length) return;

        currentWheelIndex = wheelIndex;
        
        // Apply wheel mesh to all wheel colliders
        // This would require wheel mesh references in the scene
        Debug.Log($"Wheel style changed to index: {wheelIndex}");
    }

    /// <summary>
    /// Apply body kit
    /// </summary>
    public void SetBodyKit(int bodyKitIndex)
    {
        if (bodyKitIndex < 0 || bodyKitIndex >= bodyKits.Length) return;

        currentBodyKitIndex = bodyKitIndex;
        Debug.Log($"Body kit changed to index: {bodyKitIndex}");
    }

    /// <summary>
    /// Toggle spoiler
    /// </summary>
    public void SetSpoiler(int spoilerIndex)
    {
        // Disable all spoilers
        foreach (GameObject spoiler in spoilers)
        {
            if (spoiler != null)
                spoiler.SetActive(false);
        }

        // Enable selected spoiler
        if (spoilerIndex >= 0 && spoilerIndex < spoilers.Length && spoilers[spoilerIndex] != null)
        {
            spoilers[spoilerIndex].SetActive(true);
            currentSpoilerIndex = spoilerIndex;
        }

        Debug.Log($"Spoiler changed to index: {spoilerIndex}");
    }

    /// <summary>
    /// Upgrade performance stats
    /// </summary>
    public void UpgradePerformance(string stat, float improvement)
    {
        if (carController == null) return;

        switch (stat.ToLower())
        {
            case "acceleration":
                // Increase acceleration
                Debug.Log($"Acceleration upgraded by {improvement}%");
                break;
            case "topspeed":
                // Increase top speed
                Debug.Log($"Top speed upgraded by {improvement}%");
                break;
            case "handling":
                // Improve handling
                Debug.Log($"Handling improved");
                break;
            case "braking":
                // Improve braking
                Debug.Log($"Braking upgraded by {improvement}%");
                break;
        }
    }

    /// <summary>
    /// Get current customization values
    /// </summary>
    public void LogCustomizationStatus()
    {
        Debug.Log($"Current Customization Status:\n" +
                  $"Paint Color: {currentPaintColorIndex}\n" +
                  $"Wheel Style: {currentWheelIndex}\n" +
                  $"Body Kit: {currentBodyKitIndex}\n" +
                  $"Spoiler: {currentSpoilerIndex}");
    }
}
