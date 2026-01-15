using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using DataPersistence;

public class SoundManager : MonoBehaviour, IDataPersistence
{
    public static SoundManager Instance;

    [Header("Música")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField][Range(0f, 1f)] public float musicVolume = 1f;
    
    [Header("Efeitos Sonoros")]
    [SerializeField] private AudioClip[] soundEffects;
    [SerializeField][Range(0f, 1f)] public float sfxVolume = 1f;
    
    [SerializeField] private AudioClip[] meowSounds;

    private AudioSource musicSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();
    private Dictionary<string, AudioClip> soundDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (AudioClip clip in soundEffects)
        {
            clip.LoadAudioData();  //Força carregamento na memória
        }
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.clip = backgroundMusic;

        // Configura SFX
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in soundEffects)
        {
            if (!soundDictionary.ContainsKey(clip.name))
            {
                soundDictionary.Add(clip.name, clip);
            }
        }
        
        // Cria pool de fontes de áudio
        for (int i = 0; i < 10; i++) // 10 fontes iniciais
        {
            CreateNewSFXSource();
        }
    }

    private AudioSource CreateNewSFXSource()
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        sfxSources.Add(newSource);
        return newSource;
    }

    private void Start()
    {
        SetSFXVolume(sfxVolume);
        SetMusicVolume(musicVolume);
        musicSource.Play();
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void PlaySFX(string soundName, float customVolume = 1f)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            AudioSource source = GetAvailableSFXSource();
            source.PlayOneShot(clip, sfxVolume * customVolume);
        }
        else
        {
            Debug.LogWarning($"SFX não encontrado: {soundName}");
        }
    }

    public void PlayRandomMeow()
    {
        AudioClip meow = meowSounds[Random.Range(0, meowSounds.Length)];
        AudioSource source = GetAvailableSFXSource();
        source.PlayOneShot(meow, sfxVolume);
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
    
    public void StopMusic()
    {
        musicSource.Pause();
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying) return source;
        }
        return CreateNewSFXSource(); // Cria nova se todas ocupadas
    }

    public void LoadData(GameData data)
    {
        this.sfxVolume = data.SFXVolume;
        this.musicVolume = data.MusicVolume;
    }

    public void SaveData(ref GameData data)
    {
    }
}