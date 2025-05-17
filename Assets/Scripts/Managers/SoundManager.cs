using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Configurações de Áudio")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] [Range(0f, 1f)] private float musicVolume = 1f;
    
    [Header("Referências UI")]
    [SerializeField] private Slider musicSlider;

    private AudioSource musicSource;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Configuração do AudioSource para música
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true; // 👈 Loop ativado
        musicSource.volume = musicVolume;
        musicSource.clip = backgroundMusic;
    }

    private void Start()
    {
        // Inicia a música
        musicSource.Play();

        // Configura o slider
        if(musicSlider != null) {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
    }

    // Método para ajustar volume da música
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }
}