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

    private void OnEnable()
    {
        SFXVolumeSlider.value = _SFXVolume;
        MusicVolumeSlider.value = _MusicVolume * 10;
    }

    private void Update()
    {
        float convertedMusicVolume = MusicVolumeSlider.value / 10;
        SoundManager.Instance.SetMusicVolume(convertedMusicVolume);
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
        //Only save SFX and Music outside of normal save
    }
}
