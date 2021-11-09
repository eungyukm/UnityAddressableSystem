using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Load Event Channel")]
public class LoadEventChannelSO : DescriptionBaseSO
{
    public UnityAction<GameSceneSO, bool, bool> OnLoadingRequested;
    
    private const string className = "LoadEventChannelSO";

    public void RaiseEvent(GameSceneSO locationToLoad, bool showLoadingScene = false, bool fadeScreen = false)
    {
        if (OnLoadingRequested != null)
        {
            OnLoadingRequested.Invoke(locationToLoad, showLoadingScene, fadeScreen);          
        }
        else
        {
            DebugFro.LogWarning(className, "OnLoadingRequested UnityAction을 등록 하세요!");
        }
    }
}
