using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]private FloatEventChannelSO masterVolumeEventChannel;
    [SerializeField]private FloatEventChannelSO musicVolumeEventChannel;
    [SerializeField]private FloatEventChannelSO sfxVolumeEventChannel;

    [SerializeField] private AudioMixer audioMixer;

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    private void OnEnable()
    {
        masterVolumeEventChannel.OnEventRaised += ChangeMasterVolume;
        musicVolumeEventChannel.OnEventRaised += ChangeMusicVolume;
        sfxVolumeEventChannel.OnEventRaised += ChangeSFXVolume;
    }

    private void OnDisable()
    {
        masterVolumeEventChannel.OnEventRaised -= ChangeMasterVolume;
        musicVolumeEventChannel.OnEventRaised -= ChangeMusicVolume;
        sfxVolumeEventChannel.OnEventRaised -= ChangeSFXVolume;
    }

    private void ChangeMasterVolume(float newVolume)
    {
        masterVolume = newVolume;
        SetGroupVolume("MasterVolume", masterVolume);
    }

    private void ChangeMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        SetGroupVolume("MusicVolume", musicVolume);
    }

    private void ChangeSFXVolume(float newVolume)
    {
        sfxVolume = newVolume;
        SetGroupVolume("SFXVolume", sfxVolume);
    }

    public void SetGroupVolume(string parameterName, float normalizedVolume)
    {
        bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
        if(!volumeSet)
        {
            Debug.LogError("AudioMixer parameter를 찾을 수 없습니다.");
        }
    }

    // [0 ~ 1] 사이값을 [-80dB ~ 0dB]로 변경
    private float NormalizedToMixerValue(float normalizedValue)
    {
        return (normalizedValue - 1f) * 80f;
    }
}
