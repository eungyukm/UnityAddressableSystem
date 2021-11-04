using System;
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

    private eModeType updateMode;

    private void Awake()
    {
        // 냅다 다운로드
        
    }

    // Start is called before the first frame update
    void Start()
    {
        managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed
            += LoadEventChannel;
    }

    private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
    {
        menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
    }

    private void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
    {
        obj.Result.RaiseEvent(menuToLoad, true);
        
        SceneManager.UnloadSceneAsync(0);
    }
    
    public enum eModeType
    {
        DowaloadAsset,
        Wait,
    }

    private void BundleDownload()
    {
        string key = "";
        Addressables.GetDownloadSizeAsync(key).Completed += (opSize) =>
        {
            if (opSize.Status == AsyncOperationStatus.Succeeded && opSize.Result > 0)
            {
                Addressables.DownloadDependenciesAsync(key, true).Completed += (opDownload) =>
                {
                    if (opDownload.Status != AsyncOperationStatus.Succeeded)
                    {
                        return;
                    }
                    OnDownloadDone();
                };
            }
            else
            {
                OnDownloadDone();
            }
        };
        updateMode = eModeType.Wait;
    }

    private void OnDownloadDone()
    {
        
    }
}
