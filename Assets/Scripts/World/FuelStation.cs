using UnityEngine;

/// <summary>
/// Fuel station system - Refuel vehicles at stations
/// </summary>
public class FuelStation : MonoBehaviour
{
    [SerializeField] private float refuelAmount = 30f;
    [SerializeField] private float refuelCost = 50f; // Currency units
    [SerializeField] private ParticleSystem refuelParticles;
    [SerializeField] private AudioClip refuelSound;
    [SerializeField] private float interactionRadius = 10f;

    private bool isRefueling = false;
    private CarController currentCar;

    private void Start()
    {
        gameObject.tag = "FuelStation";
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentCar = collision.GetComponent<CarController>();
            Debug.Log("Fuel station detected! Press 'E' to refuel");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopRefueling();
            currentCar = null;
        }
    }

    private void Update()
    {
        if (currentCar != null && Input.GetKeyDown(KeyCode.E))
        {
            if (!isRefueling)
                StartRefueling();
        }
    }

    /// <summary>
    /// Start refueling the car
    /// </summary>
    private void StartRefueling()
    {
        if (currentCar == null) return;

        isRefueling = true;
        
        if (refuelParticles != null)
            refuelParticles.Play();

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && refuelSound != null)
            audioSource.PlayOneShot(refuelSound);

        // Refuel the car
        currentCar.Refuel(refuelAmount);
        
        Debug.Log($"Refueled! Added {refuelAmount}L of fuel for {refuelCost} credits");
    }

    /// <summary>
    /// Stop refueling
    /// </summary>
    public void StopRefueling()
    {
        if (refuelParticles != null)
            refuelParticles.Stop();

        isRefueling = false;
    }

    /// <summary>
    /// Get refuel cost
    /// </summary>
    public float GetRefuelCost()
    {
        return refuelCost;
    }
}
