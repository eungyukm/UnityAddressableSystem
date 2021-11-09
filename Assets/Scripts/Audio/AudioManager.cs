using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // EGK : soundEmitterPoolSO
    // [Header("SoundEmitters pool")]
    // [SerializeField] private Soun
    [SerializeField] private int initialSize = 10;
    
    [Header("Listening on channels")]
    [Tooltip("SFX Channel")]
    [SerializeField] private AudioCueEventChannelSO sfxEventChannel = default;

    [Tooltip("Music Event Channel")] 
    [SerializeField] private AudioCueEventChannelSO musicEventChannel = default;

    [Tooltip("SFX Volume")] 
    [SerializeField] private FloatEventChannelSO sfxVolumeEventChannel = default;
    
    [Tooltip("Music Volume")] 
    [SerializeField] private FloatEventChannelSO musicVolumeEventChannel = default;
    
    [Tooltip("Master Volume")] 
    [SerializeField] private FloatEventChannelSO masterVolumeEventChannel = default;

    [Header("Audio control")] 
    [SerializeField] private AudioMixer audioMixer = default;

    [Range(0f, 1f)] [SerializeField] private float masterVolume = 1f;
    
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 1f;
    
    [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1f;

    // private SoundEmitterVault 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
