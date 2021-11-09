using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/Audio Configuration")]
public class AudioConfigurationSO : ScriptableObject
{
    public AudioMixerGroup OutputAudioMixerGroup = null;

    [SerializeField] private PriorityLevel priorityLevel = PriorityLevel.Standard;

    [HideInInspector]
    public int Priority
    {
        get { return (int) priorityLevel; }
        set { priorityLevel = (PriorityLevel) value; }
    }

    [Header("Sound properties")] 
    public bool Mute = false;
    [Range(0f, 1f)] public float Volume = 1f;
    [Range(-3f, 3f)] public float Pitch = 1f;
    [Range(-1f, 1f)] public float PanSereo = 0f;
    [Range(0f, 1.1f)] public float ReverbZoneMix = 1f;

    [Header("Spatialisation")] 
    [Range(0f, 1f)] public float SpatialBlend = 1f;
    // 3d sound
    public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
    [Range(0.01f, 5f)] public float MinDistance = 0.1f;
    [Range(5f, 100f)] public float MaxDistance = 50f;
    [Range(0, 360)] public int Spread = 0;
    [Range(0f, 5f)] public float DopplerLevel = 1f;

    [Header("Ignores")] 
    public bool BypassEffects = false;
    public bool BypassListenerEffects = false;
    public bool BypassReverbZones = false;
    public bool IgnoreListenerVolume = false;
    public bool IgnoreListenerPause = false;
    
    private enum PriorityLevel
    {
        Highest = 0,
        High = 64,
        Standard = 128,
        Low = 194,
        VeryLow = 256,
    }

    public void ApplyTo(AudioSource audioSource)
    {
        audioSource.outputAudioMixerGroup = OutputAudioMixerGroup;
        audioSource.mute = Mute;
        audioSource.bypassEffects = BypassEffects;
        audioSource.bypassReverbZones = BypassReverbZones;
        audioSource.priority = Priority;
        audioSource.volume = Volume;
        audioSource.pitch = Pitch;
        audioSource.panStereo = PanSereo;
        audioSource.spatialBlend = SpatialBlend;
        audioSource.reverbZoneMix = ReverbZoneMix;
        audioSource.dopplerLevel = DopplerLevel;
        audioSource.spread = Spread;
        audioSource.rolloffMode = RolloffMode;
        audioSource.minDistance = MaxDistance;
        audioSource.maxDistance = MaxDistance;
        audioSource.ignoreListenerVolume = IgnoreListenerVolume;
        audioSource.ignoreListenerPause = IgnoreListenerPause;
    }
}
