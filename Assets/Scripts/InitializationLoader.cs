using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


public class InitializationLoader : MonoBehaviour
{
    [SerializeField] private GameSceneSO managerScene = default;
    [SerializeField] private GameSceneSO menuToLoad = default;
    
    [SerializeField] private AssetReference menuLoadChannel = default;
    // Start is called before the first frame update
    void Start()
    {
        managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed
            += LoadEventChannel;
    }

    private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
    {
        // TODO : LoadAssetAsync 찾아보기
        menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
    }

    private void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
    {
        obj.Result.RaiseEvent(menuToLoad, true);
        
        // TODO : UnloadSceneAsync 찾아보기
        SceneManager.UnloadSceneAsync(0);
    }
}
