using UnityEngine;

/// <summary>
/// Checkpoint system - Mark checkpoints for missions
/// </summary>
public class CheckpointSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem checkpointParticles;
    [SerializeField] private AudioClip checkpointSound;
    [SerializeField] private float detectionRadius = 5f;

    private bool isReached = false;
    private CarController carController;

    private void Start()
    {
        gameObject.tag = "Checkpoint";
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !isReached)
        {
            ReachCheckpoint();
            carController = collision.GetComponent<CarController>();
        }
    }

    /// <summary>
    /// Mark checkpoint as reached
    /// </summary>
    private void ReachCheckpoint()
    {
        isReached = true;

        if (checkpointParticles != null)
            Instantiate(checkpointParticles, transform.position, Quaternion.identity);

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && checkpointSound != null)
            audioSource.PlayOneShot(checkpointSound);

        Debug.Log("Checkpoint reached!");
    }

    /// <summary>
    /// Reset checkpoint
    /// </summary>
    public void ResetCheckpoint()
    {
        isReached = false;
    }

    /// <summary>
    /// Check if checkpoint is reached
    /// </summary>
    public bool IsReached()
    {
        return isReached;
    }
}
