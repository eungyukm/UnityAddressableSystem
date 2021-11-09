using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameSceneSO : DescriptionBaseSO
{
    public eGameSceneType sceneType;
    public AssetReference sceneReference;
    public AudioCueSO musicTrack;

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
