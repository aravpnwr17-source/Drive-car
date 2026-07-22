using UnityEngine;

/// <summary>
/// Manages dynamic weather and day/night cycles
/// </summary>
public class WeatherSystem : MonoBehaviour
{
    [Header("Lighting")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private float dayBrightness = 1f;
    [SerializeField] private float nightBrightness = 0.2f;
    [SerializeField] private float sunRotationSpeed = 0.1f;

    [Header("Weather")]
    [SerializeField] private ParticleSystem rainParticles;
    [SerializeField] private ParticleSystem snowParticles;
    [SerializeField] private ParticleSystem fogParticles;
    [SerializeField] private float weatherChangeInterval = 30f;

    [Header("Fog")]
    [SerializeField] private float dayFogDensity = 0.05f;
    [SerializeField] private float nightFogDensity = 0.1f;

    private float timeOfDay = 0f; // 0-1, where 0.5 is midnight
    private float weatherTimer = 0f;
    private WeatherType currentWeather = WeatherType.Clear;
    private enum WeatherType { Clear, Rain, Snow, Fog, Storm }

    private void Start()
    {
        SetupWeatherSystem();
    }

    private void Update()
    {
        UpdateTimeOfDay();
        UpdateLighting();
        UpdateWeather();
    }

    /// <summary>
    /// Update in-game time of day
    /// </summary>
    private void UpdateTimeOfDay()
    {
        timeOfDay += Time.deltaTime / (60f * 60f); // 1 hour = 60 seconds
        if (timeOfDay >= 1f)
            timeOfDay -= 1f;
    }

    /// <summary>
    /// Update lighting based on time of day
    /// </summary>
    private void UpdateLighting()
    {
        if (directionalLight == null) return;

        // Calculate sun rotation
        float sunAngle = timeOfDay * 360f;
        directionalLight.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0);

        // Calculate brightness based on time of day
        float brightness = CalculateBrightness();
        directionalLight.intensity = brightness;

        // Update ambient light
        RenderSettings.ambientIntensity = brightness;

        // Update fog
        UpdateFogDensity();
    }

    /// <summary>
    /// Calculate lighting brightness based on time of day
    /// </summary>
    private float CalculateBrightness()
    {
        // 0.25 = sunrise, 0.5 = noon, 0.75 = sunset, 0 or 1 = midnight
        if (timeOfDay < 0.25f)
            return Mathf.Lerp(nightBrightness, dayBrightness, timeOfDay / 0.25f);
        else if (timeOfDay < 0.75f)
            return dayBrightness;
        else
            return Mathf.Lerp(dayBrightness, nightBrightness, (timeOfDay - 0.75f) / 0.25f);
    }

    /// <summary>
    /// Update fog density based on time of day
    /// </summary>
    private void UpdateFogDensity()
    {
        float targetDensity = Mathf.Lerp(nightFogDensity, dayFogDensity, CalculateBrightness());
        RenderSettings.fogDensity = targetDensity;
    }

    /// <summary>
    /// Update weather effects
    /// </summary>
    private void UpdateWeather()
    {
        weatherTimer -= Time.deltaTime;

        if (weatherTimer <= 0)
        {
            ChangeWeather();
            weatherTimer = weatherChangeInterval;
        }
    }

    /// <summary>
    /// Randomly change weather
    /// </summary>
    private void ChangeWeather()
    {
        int weatherIndex = Random.Range(0, 5);
        currentWeather = (WeatherType)weatherIndex;

        DisableAllWeatherEffects();

        switch (currentWeather)
        {
            case WeatherType.Clear:
                // Clear weather - no effects
                break;
            case WeatherType.Rain:
                if (rainParticles != null)
                    rainParticles.Play();
                break;
            case WeatherType.Snow:
                if (snowParticles != null)
                    snowParticles.Play();
                break;
            case WeatherType.Fog:
                if (fogParticles != null)
                    fogParticles.Play();
                break;
            case WeatherType.Storm:
                if (rainParticles != null)
                    rainParticles.Play();
                // Add thunder effects
                break;
        }
    }

    /// <summary>
    /// Disable all weather particle effects
    /// </summary>
    private void DisableAllWeatherEffects()
    {
        if (rainParticles != null)
            rainParticles.Stop();
        if (snowParticles != null)
            snowParticles.Stop();
        if (fogParticles != null)
            fogParticles.Stop();
    }

    /// <summary>
    /// Setup initial weather system configuration
    /// </summary>
    private void SetupWeatherSystem()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = dayFogDensity;
        RenderSettings.fogColor = Color.gray;
    }

    /// <summary>
    /// Get current weather type
    /// </summary>
    public WeatherType GetCurrentWeather()
    {
        return currentWeather;
    }

    /// <summary>
    /// Get time of day (0-1)
    /// </summary>
    public float GetTimeOfDay()
    {
        return timeOfDay;
    }

    /// <summary>
    /// Get current brightness
    /// </summary>
    public float GetBrightness()
    {
        return CalculateBrightness();
    }

    /// <summary>
    /// Is it currently daytime?
    /// </summary>
    public bool IsDaytime()
    {
        return CalculateBrightness() > 0.5f;
    }
}
