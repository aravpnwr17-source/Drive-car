using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages all HUD (Heads-Up Display) elements
/// </summary>
public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private Image fuelBar;
    [SerializeField] private Image damageBar;
    [SerializeField] private Image minimap;
    [SerializeField] private RectTransform compassNeedle;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI weatherText;

    private CarController carController;
    private float fuelMaxValue;
    private float gameTime = 0f;

    private void Start()
    {
        carController = FindObjectOfType<CarController>();
        if (carController != null)
            fuelMaxValue = carController.GetMaxFuel();
    }

    private void Update()
    {
        if (carController == null) return;

        UpdateSpeedometer();
        UpdateFuelBar();
        UpdateDamageBar();
        UpdateCompass();
        UpdateTime();
    }

    /// <summary>
    /// Update speedometer display
    /// </summary>
    private void UpdateSpeedometer()
    {
        float speed = carController.GetCurrentSpeed();
        if (speedText != null)
            speedText.text = $"<size=150%>{speed:F0}</size>\nkm/h";
    }

    /// <summary>
    /// Update fuel bar and text
    /// </summary>
    private void UpdateFuelBar()
    {
        float fuel = carController.GetFuel();
        float fuelPercentage = fuel / fuelMaxValue;

        if (fuelText != null)
            fuelText.text = $"Fuel: {fuel:F1}L";

        if (fuelBar != null)
        {
            fuelBar.fillAmount = fuelPercentage;
            
            // Change color based on fuel level
            if (fuelPercentage > 0.5f)
                fuelBar.color = Color.green;
            else if (fuelPercentage > 0.25f)
                fuelBar.color = Color.yellow;
            else
                fuelBar.color = Color.red;
        }
    }

    /// <summary>
    /// Update damage bar and text
    /// </summary>
    private void UpdateDamageBar()
    {
        float damage = carController.GetDamage();
        float damagePercentage = damage / 100f;

        if (damageText != null)
            damageText.text = $"Damage: {damage:F0}%";

        if (damageBar != null)
        {
            damageBar.fillAmount = damagePercentage;
            
            // Change color based on damage level
            if (damagePercentage < 0.3f)
                damageBar.color = Color.green;
            else if (damagePercentage < 0.7f)
                damageBar.color = Color.yellow;
            else
                damageBar.color = Color.red;
        }
    }

    /// <summary>
    /// Update compass needle rotation
    /// </summary>
    private void UpdateCompass()
    {
        if (compassNeedle != null && carController != null)
        {
            float carYRotation = carController.transform.eulerAngles.y;
            compassNeedle.rotation = Quaternion.Euler(0, 0, -carYRotation);
        }
    }

    /// <summary>
    /// Update in-game time display
    /// </summary>
    private void UpdateTime()
    {
        gameTime += Time.deltaTime;
        
        // Calculate hour and minute (1 real second = 1 game minute)
        int totalMinutes = (int)gameTime % (24 * 60);
        int hour = totalMinutes / 60;
        int minute = totalMinutes % 60;

        if (timeText != null)
            timeText.text = $"{hour:D2}:{minute:D2}";
    }

    /// <summary>
    /// Update mission objective text
    /// </summary>
    public void UpdateMission(string missionText)
    {
        if (this.missionText != null)
            this.missionText.text = missionText;
    }

    /// <summary>
    /// Update weather display
    /// </summary>
    public void UpdateWeather(string weatherType)
    {
        if (weatherText != null)
            weatherText.text = $"Weather: {weatherType}";
    }

    /// <summary>
    /// Show damage effect (screen shake, red overlay)
    /// </summary>
    public void ShowDamageEffect()
    {
        // Implement screen shake or red flash effect
        StartCoroutine(DamageFlash());
    }

    /// <summary>
    /// Flash effect when taking damage
    /// </summary>
    private System.Collections.IEnumerator DamageFlash()
    {
        Image damageOverlay = gameObject.AddComponent<Image>();
        damageOverlay.color = new Color(1, 0, 0, 0.3f);
        yield return new WaitForSeconds(0.1f);
        Destroy(damageOverlay);
    }

    /// <summary>
    /// Show notification popup
    /// </summary>
    public void ShowNotification(string message, float duration = 3f)
    {
        // Implement notification popup system
        Debug.Log($"Notification: {message}");
    }
}
