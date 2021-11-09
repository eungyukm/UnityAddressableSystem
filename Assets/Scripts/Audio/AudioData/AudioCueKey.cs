using UnityEngine;

// EGK : struct 정리하기
public struct AudioCueKey
{
    public static AudioCueKey Invalid = new AudioCueKey(-1, null);
    
    // EGK : internal 사용 이유 찾아보기
    internal int Value;
    internal AudioCueSO AudioCue;

    internal AudioCueKey(int value, AudioCueSO audioCue)
    {
        Value = value;
        AudioCue = audioCue;
    }
    
    // EGK : Equls 찾아보기
    public override bool Equals(object obj)
    {
        return obj is AudioCueKey x && Value == x.Value && AudioCue == x.AudioCue;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() ^ AudioCue.GetHashCode();
    }

    public static bool operator ==(AudioCueKey x, AudioCueKey y)
    {
        return x.Value == y.Value && x.AudioCue == y.AudioCue;
    }

    public static bool operator !=(AudioCueKey x, AudioCueKey y)
    {
        return !(x == y);
    }
}
