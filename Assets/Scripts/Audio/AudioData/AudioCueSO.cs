using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 병렬 재생 및 랜덤화 지원
/// </summary>
[CreateAssetMenu(fileName = "newAudioCue", menuName = "Audio/Audio Cue")]
public class AudioCueSO : ScriptableObject
{
    public bool looping = false;
    [SerializeField] private AudioClipsGroup[] audioClipGroups = default;

    public AudioClip[] GetClips()
    {
        // 오디오 그룹의 수 가져오기
        int numberOfClips = audioClipGroups.Length;
        AudioClip[] resultingClips = new AudioClip[numberOfClips];

        for (int i = 0; i < numberOfClips; i++)
        {
            resultingClips[i] = audioClipGroups[i].GetNextClip();
        }

        return resultingClips;
    }

    /// <summary>
    /// 오디오 클립 그룹 나타내기 위한 클래스
    /// </summary>
    [Serializable]
    public class AudioClipsGroup
    {
        public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
        public AudioClip[] audioClips;

        private int nextClipToPlay = -1;
        private int lastClipPlayed = -1;

        /// <summary>
        /// 순서를 따르거나, 랜덤 재생
        /// </summary>
        /// <returns></returns>
        public AudioClip GetNextClip()
        {
            if (audioClips.Length == 1)
            {
                return audioClips[0];
            }

            if (nextClipToPlay == -1)
            {
                nextClipToPlay = (sequenceMode == SequenceMode.Sequential
                    ? 0
                    : UnityEngine.Random.Range(0, audioClips.Length));
            }
            else
            {
                switch (sequenceMode)
                {
                    case SequenceMode.Random:
                        break;

                    case SequenceMode.RandomNoImmediateRepeat:
                        do
                        {
                            nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        } while (nextClipToPlay == lastClipPlayed);

                        break;

                    case SequenceMode.Sequential:
                        nextClipToPlay = (int) Mathf.Repeat(++nextClipToPlay, audioClips.Length);
                        break;
                }
            }

            lastClipPlayed = nextClipToPlay;

            return audioClips[nextClipToPlay];
        }

        public enum SequenceMode
        {
            Random,
            RandomNoImmediateRepeat,
            Sequential,
        }
    }
}



