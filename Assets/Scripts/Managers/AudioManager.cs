using UnityEngine;

/// <summary>
/// Audio manager - Centralized audio handling
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private float masterVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Play background music
    /// </summary>
    public void PlayMusic(AudioClip clip, float volume = 0.5f)
    {
        if (musicSource == null) return;
        musicSource.clip = clip;
        musicSource.volume = volume * masterVolume;
        musicSource.Play();
    }

    /// <summary>
    /// Play sound effect
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume * masterVolume);
    }

    /// <summary>
    /// Set master volume
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        AudioListener.volume = masterVolume;
    }

    /// <summary>
    /// Stop all music
    /// </summary>
    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }
}
