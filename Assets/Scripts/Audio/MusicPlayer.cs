using System;
using UnityEngine;

/// <summary>
/// 전체적인 Music 관리하는 Player
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onSceneReady = default;
    [SerializeField] private AudioCueEventChannelSO playMusicOn = default;
    [SerializeField] private GameSceneSO thisSceneSO;
    [SerializeField] private AudioConfigurationSO audioConfig;
    
    [Header("Pause menu music")]
    [SerializeField] private AudioCueSO pauseMusic = default;
    [SerializeField] private BoolEventChannelSO onPauseOpened = default;
    
    private void OnEnable()
    {
        onSceneReady.OnEventRaised += PlayMusic;
        onPauseOpened.OnEventRaised += PlayPauseMusic;
    }

    private void OnDisable()
    {
        onSceneReady.OnEventRaised -= PlayMusic;
        onPauseOpened.OnEventRaised -= PlayPauseMusic;
    }

    private void PlayMusic()
    {
        playMusicOn.RaisePlayEvent(thisSceneSO.musicTrack, audioConfig);
    }
    
    private void PlayPauseMusic(bool open)
    {
        if (open)
        {
            // Pause 되었을 때
            playMusicOn.RaisePlayEvent(pauseMusic, audioConfig);
        }
        else
        {
            PlayMusic();
        }
    }
}
