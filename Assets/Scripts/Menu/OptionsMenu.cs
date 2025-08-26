using System;
using DataPersistence;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Slider MusicVolumeSlider;
    
    private float _SFXVolume;
    private float _MusicVolume;

    private void Start()
    {
        ApplySettings();
    }
    
    public void OnSFXVolumeChanged(float volume)
    {
        SoundManager.Instance.SetSFXVolume(volume / 10);
        _SFXVolume = volume / 10;
    }

    public void OnMusicVolumeChanged(float volume)
    {
        SoundManager.Instance.SetMusicVolume(volume / 10);
        
        _MusicVolume = volume / 10;
    }

    private void ApplySettings()
    {
        MusicVolumeSlider.value = _MusicVolume * 10;
        SFXVolumeSlider.value = _SFXVolume * 10;
        
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

    public void BackButton()
    {
        SoundManager.Instance.PlaySFX("Button");
    }
}
