using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Events/Void Event Channel")]
public class VoidEventChannelSO : DescriptionBaseSO
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke();
        }
        else
        {
            Debug.LogWarning("[VoidEventChannelSO] 이벤트를 등록 해주세요!");
        }
            
    }
}
