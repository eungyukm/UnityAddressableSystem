using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource audioSource;
    
    // Sound Play가 끝날을 때
    public event UnityAction<SoundEmitter> OnSoundFinishedPlying;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayAudioClip(AudioClip clip, AudioConfigurationSO settings, bool hasToLoop, Vector3 position = default)
    {
        audioSource.clip = clip;
        settings.ApplyTo(audioSource);
        audioSource.transform.position = position;
        audioSource.loop = hasToLoop;
        audioSource.time = 0f;
        audioSource.Play();

        if (!hasToLoop)
        {
            
        }
    }

    IEnumerator FinishedPlaying(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        
        NotifyBeingDone();
    }

    private void NotifyBeingDone()
    {
        
    }
}
