using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameSceneSO : DescriptionBaseSO
{
    public eGameSceneType sceneType;
    public AssetReference sceneReference;

    public enum eGameSceneType
    {
        Location,
        Menu,
        
        Initialisation,
        PersistenManagers,
        Gameplay,
        
        Art,
    }
}
