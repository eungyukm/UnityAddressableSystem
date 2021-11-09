using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/AudioCue Event Channel")]
public class AudioCueEventChannelSO : DescriptionBaseSO
{
    public AudioCuePlayAction OnAudioCuePlayRequested;
    // Stop 신호 호출
    public AudioCueStopAction OnAudioCueStopRequested;
    // Finish 신호 호출
    public AudioCueFinishAction OnAudioCueFinishRequested;

    public AudioCueKey RaisePlayEvent(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
    {
        AudioCueKey audioCueKey = AudioCueKey.Invalid;

        if (OnAudioCuePlayRequested != null)
        {
            audioCueKey = OnAudioCuePlayRequested.Invoke(audioCue, audioConfiguration, positionInSpace);
        }
        else
        {
            Debug.LogWarning("[AudioCueEventChannelSO] OnAudioCuePlayRequested를 체크해주세요!");
        }

        return audioCueKey;
    }

    public bool RaiseStopEvent(AudioCueKey audioCueKey)
    {
        bool requestSucced = false;

        if (OnAudioCueStopRequested != null)
        {
            requestSucced = OnAudioCueStopRequested.Invoke(audioCueKey);
        }
        else
        {
            Debug.LogWarning("[AudioCueEventChannelSO] OnAudioCueStopRequested를 체크해주세요!");
        }

        return requestSucced;
    }

    public bool RaiseFinishEvent(AudioCueKey audioCueKey)
    {
        bool requestSucced = false;

        if (OnAudioCueStopRequested != null)
        {
            requestSucced = OnAudioCueFinishRequested.Invoke(audioCueKey);
        }
        else
        {
            Debug.LogWarning("[AudioCueEventChannelSO] OnAudioCueStopRequested를 체크해주세요!");
        }

        return requestSucced;
    }
}

public delegate AudioCueKey AudioCuePlayAction(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace);

// Audio Cue Stop 되었을 때
public delegate bool AudioCueStopAction(AudioCueKey emitterKey);

// Audio Cue가 Fish 되었을 때
public delegate bool AudioCueFinishAction(AudioCueKey emitterKey);