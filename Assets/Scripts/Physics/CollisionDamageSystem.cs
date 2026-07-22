using UnityEngine;

/// <summary>
/// Handles collision detection and damage system
/// </summary>
public class CollisionDamageSystem : MonoBehaviour
{
    [SerializeField] private CarController carController;
    [SerializeField] private float collisionDamageMultiplier = 1f;
    [SerializeField] private float minImpactForce = 5f;
    [SerializeField] private ParticleSystem impactParticles;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioSource audioSource;

    private Rigidbody vehicleRigidbody;
    private float lastCollisionTime = 0f;
    private float collisionCooldown = 0.5f;

    private void Start()
    {
        vehicleRigidbody = GetComponent<Rigidbody>();
        if (carController == null)
            carController = GetComponent<CarController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Prevent multiple collision triggers too quickly
        if (Time.time - lastCollisionTime < collisionCooldown)
            return;

        lastCollisionTime = Time.time;

        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce < minImpactForce)
            return;

        // Calculate damage based on impact force
        float damageAmount = impactForce * collisionDamageMultiplier;
        carController.ApplyDamage(damageAmount);

        // Play crash sound
        if (audioSource != null && crashSound != null)
            audioSource.PlayOneShot(crashSound);

        // Spawn impact particles
        if (impactParticles != null)
        {
            Instantiate(impactParticles, collision.contacts[0].point, Quaternion.identity);
        }

        // Log collision info
        Debug.Log($"Collision with {collision.gameObject.name}! Damage: {damageAmount:F2}");

        // Handle different collision types
        if (collision.gameObject.CompareTag("Vehicle"))
        {
            HandleVehicleCollision(collision.gameObject, impactForce);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(collision.gameObject, impactForce);
        }
        else if (collision.gameObject.CompareTag("Building"))
        {
            HandleBuildingCollision(impactForce);
        }
    }

    /// <summary>
    /// Handle collision with another vehicle
    /// </summary>
    private void HandleVehicleCollision(GameObject otherVehicle, float impactForce)
    {
        // Both vehicles take damage
        CarController otherCarController = otherVehicle.GetComponent<CarController>();
        if (otherCarController != null)
        {
            otherCarController.ApplyDamage(impactForce * collisionDamageMultiplier * 0.7f);
        }

        Debug.Log($"Vehicle collision with {otherVehicle.name} at force {impactForce}");
    }

    /// <summary>
    /// Handle collision with obstacle
    /// </summary>
    private void HandleObstacleCollision(GameObject obstacle, float impactForce)
    {
        // Destroy light obstacles
        if (obstacle.CompareTag("Obstacle"))
        {
            if (impactForce > 20f)
            {
                Destroy(obstacle);
                Debug.Log("Obstacle destroyed!");
            }
        }
    }

    /// <summary>
    /// Handle collision with building
    /// </summary>
    private void HandleBuildingCollision(float impactForce)
    {
        // Buildings take minimal damage, car takes full damage
        if (impactForce > 50f)
        {
            Debug.Log("Heavy impact with building!");
        }
    }

    /// <summary>
    /// Get impact force from collision
    /// </summary>
    public float GetImpactForce(Collision collision)
    {
        return collision.relativeVelocity.magnitude;
    }
}
