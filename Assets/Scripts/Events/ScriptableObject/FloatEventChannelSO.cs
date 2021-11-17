using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/FloatEventChannel", fileName = "New FloatEventChannelSO")]
public class FloatEventChannelSO : DescriptionBaseSO
{
    public UnityAction<float> OnEventRaised;

    public void RaiseEvent(float value)
    {
        if(OnEventRaised != null)
        {
            OnEventRaised.Invoke(value);
        }
        else
        {
            Debug.LogWarning("[FloatEventChannelSO] OnEventRaised를 등록해주세요!");
        }
    }
}
