using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingsAudioComponent : MonoBehaviour
{
    [SerializeField] private FloatEventChannelSO sfxVolumeEventChannel;
    [SerializeField] private FloatEventChannelSO musicVolumeEventChannel;
    [SerializeField] private FloatEventChannelSO masterVolumeEventChannel;

    [SerializeField] private float sfxVolume = 1;
    [SerializeField] private float musicVolume = 1;
    [SerializeField] private float masterVolume = 1;

    public void Setup(float musicVolume, float sfxVolume, float masterVolume)
    {
        this.sfxVolume = sfxVolume;
        this.musicVolume = musicVolume;
        this.masterVolume = masterVolume;
    }

    private void SetMasterVolume()
    {
        masterVolumeEventChannel.RaiseEvent(masterVolume);
    }

    private void SetMusicVolume()
    {
        musicVolumeEventChannel.RaiseEvent(musicVolume);
    }

    private void SetSFXVolume()
    {
        sfxVolumeEventChannel.RaiseEvent(sfxVolume);
    }

    private void Start()
    {
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            SetMasterVolume();
            SetMusicVolume();
            SetSFXVolume();
        }
    }
}
