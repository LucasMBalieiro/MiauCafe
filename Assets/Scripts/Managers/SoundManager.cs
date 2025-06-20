using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Música")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 1f;
    
    [Header("Efeitos Sonoros")]
    [SerializeField] private AudioClip[] soundEffects;
    [SerializeField][Range(0f, 1f)] public float sfxVolume = 1f;
    
    [Header("Referências UI")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

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
        // Configura música
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
        musicSource.Play();
        
        if(musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        
        if(sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(v => sfxVolume = v);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
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

    private AudioSource GetAvailableSFXSource()
    {
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying) return source;
        }
        return CreateNewSFXSource(); // Cria nova se todas ocupadas
    }
}