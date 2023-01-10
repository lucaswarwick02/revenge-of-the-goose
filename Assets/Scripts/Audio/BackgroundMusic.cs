using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private const float MUSIC_FADE_IN_TIME = 1f;
    private const float MUSIC_FADE_OUT_TIME = 2f;

    static BackgroundMusic INSTANCE;

    [SerializeField] private AudioClip MetalMusic;
    [SerializeField] private AudioClip NeutralMusic;

    private AudioClip currentPlaying;

    AudioSource audioSource;

    private void Awake() {
        INSTANCE = this;

        MainMenu.OnMainMenuAnimationSoftComplete += StartMainMenuMusic;
        GameHandler.OnNeutralModeChange += OnNeutralModeChanged;
        Converser.OnStartConversation += ForceNeutralMusic;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        MainMenu.OnMainMenuAnimationSoftComplete -= StartMainMenuMusic;
        GameHandler.OnNeutralModeChange -= OnNeutralModeChanged;
        Converser.OnStartConversation -= ForceNeutralMusic;
    }

    private void StartMainMenuMusic()
    {
        PlayMusic(MetalMusic);
    }

    public static void ForceNeutralMusic () {
        INSTANCE.OnNeutralModeChanged(true);
    }

    public static void StopMusic()
    {
        INSTANCE.PlayMusic(null);
    }

    public static void ForceNeutralMusic(Transform converser)
    {
        ForceNeutralMusic();
    }

    private void OnNeutralModeChanged(bool inNeutralMode)
    {
        PlayMusic(inNeutralMode ? NeutralMusic : MetalMusic);
    }

    private void PlayMusic(AudioClip audioClip, float newVolume = 1)
    {
        if (audioClip != currentPlaying)
        {
            StartCoroutine(PlayNewTrack(audioClip, newVolume));
            currentPlaying = audioClip;
        }
    }

    private IEnumerator PlayNewTrack(AudioClip audioClip, float newVolume = 1)
    {
        float timePassed = 0;

        // If a track is already playing, fade it out first
        if (INSTANCE.audioSource.volume > 0)
        {
            float initialVolume = INSTANCE.audioSource.volume;
            while (INSTANCE.audioSource.volume != 0)
            {
                INSTANCE.audioSource.volume = Mathf.Lerp(initialVolume, 0, timePassed / MUSIC_FADE_OUT_TIME);
                timePassed += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        // Pause audio source and switch track
        INSTANCE.audioSource.Stop();
        INSTANCE.audioSource.clip = audioClip;

        // If new track is not null, start it and fade it in
        if (audioClip != null)
        {
            timePassed = 0;
            INSTANCE.audioSource.Play();

            while (INSTANCE.audioSource.volume != newVolume)
            {
                INSTANCE.audioSource.volume = Mathf.Lerp(0, newVolume, timePassed / MUSIC_FADE_IN_TIME);
                timePassed += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
