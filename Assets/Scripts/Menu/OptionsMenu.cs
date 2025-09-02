using System;
using DataPersistence;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour, IDataPersistence
{
    [SerializeField] private CircularSlider SFXVolumeSlider;
    [SerializeField] private CircularSlider MusicVolumeSlider;
    
    [SerializeField] private Slider MusicVolumeNormalSlider;
    [SerializeField] private Slider SFXVolumeNormalSlider;
    
    [SerializeField] private Image background;
    [SerializeField] private Sprite mainMenuImage;
    
    private float _SFXVolume;
    private float _MusicVolume;

    private void Start()
    {
        ApplySettings();
    }
    
    public void OnSFXVolumeChanged(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume);
        _SFXVolume = volume;
    }

    public void OnMusicVolumeChanged(float volume)
    {
        SoundManager.Instance.SetMusicVolume(volume);
        
        _MusicVolume = volume;
    }
    
    public void BackButton()
    {
        if (mainMenuImage != null) background.sprite = mainMenuImage;
    }

    public void PlaySound()
    {
        SoundManager.Instance.PlaySFX("Button");
    }

    private void ApplySettings()
    {
        if (SFXVolumeSlider != null && MusicVolumeSlider != null)
        {
            SFXVolumeSlider.sliderValue = _SFXVolume;
            MusicVolumeSlider.sliderValue = _MusicVolume;
        }
        else
        {
            SFXVolumeNormalSlider.value = _SFXVolume;
            MusicVolumeNormalSlider.value = _MusicVolume;
        }
        
        
        
        SoundManager.Instance.SetMusicVolume(_MusicVolume);
        SoundManager.Instance.SetSFXVolume(_SFXVolume);
    }

    public void LoadData(GameData data)
    {
        this._SFXVolume = data.SFXVolume;
        this._MusicVolume = data.MusicVolume;
    }

    public void SaveData(ref GameData data)
    {
        data.SFXVolume = this._SFXVolume;
        data.MusicVolume = this._MusicVolume;
    }
    
}
