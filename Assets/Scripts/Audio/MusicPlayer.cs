using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전체적인 Music 관리하는 Player
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    // [SerializeField] private AudioCueEv
    
    // [Header("Pause menu music")]
    [SerializeField] private BoolEventChannelSO onPauseOpened = default;
    
    private void OnEnable()
    {
        onPauseOpened.OnEventRaised += PlayPauseMusic;
    }

    private void PlayPauseMusic(bool open)
    {
        if (open)
        {
            // Pause 되었을 때
            // playMusic
        }
        else
        {
            
        }
    }
}
