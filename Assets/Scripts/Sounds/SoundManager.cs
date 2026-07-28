using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    // Music
    Theme,
    GameplayTheme,

    // UI
    ButtonClick,
    ButtonHover,

    // Footsteps
    Run,

    // Ambience
    Wind,

    // Lose
    Lose,

    // Win
    Win,

    // Countdown
    Countdown,
    CountdownStart,

    // Gold square bonus
    GoalBonus
}

[System.Serializable]
public class Sound
{
    public SoundType type;

    public AudioClip[] clips;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(.5f, 2f)]
    public float pitch = 1f;

    [Range(0f, 1f)]
    public float volumeVariance = 0.1f;

    [Range(0f, 1f)]
    public float pitchVariance = 0.1f;

    public bool loop;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Mixer")]
    [SerializeField] private AudioMixerGroup mixerGroup;

    [Header("Library")]
    [SerializeField] private Sound[] sounds;

    [Header("Pool")]
    [SerializeField] private int sfxPoolSize = 15;

    [Header("Music Intensity")]
    [SerializeField] private float musicPitchSmoothTime = 0.9f;
    [SerializeField] private float maxMusicPitchMultiplier = 1.2f;

    private AudioSource musicSource;
    private AudioSource uiSource;
    // private AudioSource ambienceSource;

    private AudioSource[] sfxPool;
    private SoundType? currentMusic = null;

    private float musicBasePitch = 1f;
    private float musicPitchMultiplier = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateSources();
    }

    void Update()
    {
        if (musicSource == null || musicSource.clip == null) return;

        float targetPitch = musicBasePitch * musicPitchMultiplier;
        float alpha = 1f - Mathf.Exp(-Time.deltaTime / musicPitchSmoothTime);
        musicSource.pitch = Mathf.Lerp(musicSource.pitch, targetPitch, alpha);
    }

    public void SetMusicIntensity(float multiplier)
    {
        musicPitchMultiplier = Mathf.Clamp(multiplier, 0.5f, maxMusicPitchMultiplier);
    }

    void CreateSources()
    {
        musicSource = CreateSource("Music");
        uiSource = CreateSource("UI");
        // ambienceSource = CreateSource("Ambience");

        uiSource.ignoreListenerPause = true;

        sfxPool = new AudioSource[sfxPoolSize];

        for (int i = 0; i < sfxPoolSize; i++)
        {
            sfxPool[i] = CreateSource("SFX_" + i);
        }
    }

    AudioSource CreateSource(string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;

        AudioSource source = obj.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = mixerGroup;

        return source;
    }

    Sound GetSound(SoundType type)
    {
        return sounds.FirstOrDefault(s => s.type == type);
    }

    AudioClip GetRandomClip(Sound sound)
    {
        if (sound.clips.Length == 1)
            return sound.clips[0];

        return sound.clips[Random.Range(0, sound.clips.Length)];
    }

    AudioSource GetFreeSFXSource()
    {
        foreach (AudioSource source in sfxPool)
        {
            if (!source.isPlaying)
                return source;
        }

        return sfxPool[0];
    }

    public void PlaySFX(SoundType type)
    {
        Sound sound = GetSound(type);

        if (sound == null)
        {
            Debug.LogWarning($"Missing sound: {type}");
            return;
        }

        AudioSource source = GetFreeSFXSource();

        source.pitch =
            sound.pitch *
            (1 + Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));

        source.PlayOneShot(
            GetRandomClip(sound),
            sound.volume *
            (1 + Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f))
        );
    }

    public void PlayUI(SoundType type)
    {
        Sound sound = GetSound(type);

        if (sound == null)
            return;

        uiSource.pitch =
            sound.pitch *
            (1 + Random.Range(-sound.pitchVariance / 2f, sound.pitchVariance / 2f));

        uiSource.PlayOneShot(
            GetRandomClip(sound),
            sound.volume *
            (1 + Random.Range(-sound.volumeVariance / 2f, sound.volumeVariance / 2f))
        );
    }

    public void PlayMusic(SoundType type)
    {
        if (currentMusic == type && musicSource.isPlaying)
            return;

        currentMusic = type;
        
        Sound sound = GetSound(type);

        if (sound == null)
            return;

        musicSource.clip = GetRandomClip(sound);
        musicSource.volume = sound.volume;
        musicSource.loop = true;
        musicSource.pitch = sound.pitch;
        musicSource.Play();

        musicBasePitch = sound.pitch;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void CrossFadeMusic(SoundType nextSong, float duration)
    {
        if (currentMusic == nextSong)
            return;

        currentMusic = nextSong;
        StartCoroutine(CrossFade(nextSong, duration));
    }

    IEnumerator CrossFade(SoundType nextSong, float duration)
    {
        Sound sound = GetSound(nextSong);

        if (sound == null)
            yield break;

        AudioSource newSource = CreateSource("Crossfade");

        newSource.clip = GetRandomClip(sound);
        newSource.loop = true;
        newSource.pitch = sound.pitch;
        newSource.volume = 0;
        newSource.Play();

        float timer = 0;

        float oldVolume = musicSource.volume;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            musicSource.volume = Mathf.Lerp(oldVolume, 0, t);
            newSource.volume = Mathf.Lerp(0, sound.volume, t);

            yield return null;
        }

        Destroy(musicSource.gameObject);

        musicSource = newSource;
        musicBasePitch = sound.pitch;
    }
}