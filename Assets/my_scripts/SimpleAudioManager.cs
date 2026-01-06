using UnityEngine;

public class SimpleAudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip shootSound;

    // These are now private because we create them in code!
    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        // 1. Create the Music Source automatically
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.5f; // Optional: lower music volume slightly

        // 2. Create the SFX Source automatically
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
    }

    void Start()
    {
        // Play Music immediately if a clip is assigned
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void PlayShootSound()
    {
        if (shootSound != null)
        {
            // PlayOneShot allows sounds to overlap
            sfxSource.PlayOneShot(shootSound);
        }
    }
}