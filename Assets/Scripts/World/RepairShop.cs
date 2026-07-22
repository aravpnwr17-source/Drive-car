using UnityEngine;

/// <summary>
/// Repair shop system - Repair vehicles
/// </summary>
public class RepairShop : MonoBehaviour
{
    [SerializeField] private float repairCost = 100f;
    [SerializeField] private float repairPercentage = 50f; // Repair 50% of damage
    [SerializeField] private ParticleSystem repairParticles;
    [SerializeField] private AudioClip repairSound;
    [SerializeField] private float interactionRadius = 10f;

    private bool isRepairing = false;
    private CarController currentCar;
    private AudioSource audioSource;

    private void Start()
    {
        gameObject.tag = "RepairShop";
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentCar = collision.GetComponent<CarController>();
            Debug.Log("Repair shop detected! Press 'E' to repair");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopRepairing();
            currentCar = null;
        }
    }

    private void Update()
    {
        if (currentCar != null && Input.GetKeyDown(KeyCode.E))
        {
            if (!isRepairing)
                StartRepairing();
        }
    }

    /// <summary>
    /// Start repairing the car
    /// </summary>
    private void StartRepairing()
    {
        if (currentCar == null) return;

        isRepairing = true;

        if (repairParticles != null)
            repairParticles.Play();

        if (audioSource != null && repairSound != null)
            audioSource.PlayOneShot(repairSound);

        // Repair the car
        float currentDamage = currentCar.GetDamage();
        float repairAmount = currentDamage * (repairPercentage / 100f);
        currentCar.ApplyDamage(-repairAmount);

        Debug.Log($"Car repaired! Cost: {repairCost} credits");
    }

    /// <summary>
    /// Stop repairing
    /// </summary>
    public void StopRepairing()
    {
        if (repairParticles != null)
            repairParticles.Stop();

        isRepairing = false;
    }

    /// <summary>
    /// Get repair cost
    /// </summary>
    public float GetRepairCost()
    {
        return repairCost;
    }
}
