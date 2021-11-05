using System;
using System.Collections;
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

    private const string className = "InitializationLoader";
    
    private int downloadCount = 0;

    [SerializeField] private AssetLabelReference[] labels;

    [SerializeField] private bool useDownload;
    private void Awake()
    {
        DebugFro.isLogVisable = true;

        updateMode = eModeType.None;

        foreach (var label in labels)
        {
            DebugFro.Log(label.labelString);
        }

        Caching.ClearCache();
        
        // 냅다 다운로드
        if (useDownload)
        {
            StartCoroutine(BundleDownload());            
        }
        else
        {
            managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed
                += LoadEventChannel;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

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
        None,
        DowaloadAsset,
        Wait,
        DownloadDone,
        DownloadAll,
    }

    private IEnumerator BundleDownload()
    {
        DebugFro.Log(className,"다운로드 시작!");
        for (int i = 0; i < labels.Length; i++)
        {
            int capture = i;
            Addressables.GetDownloadSizeAsync(labels[capture]).Completed += (opSize) =>
            {
                DebugFro.Log(className, $"{labels[capture].labelString} Size : " + string.Concat(opSize.Result, " byte"));
                
                
                if (opSize.Status == AsyncOperationStatus.Succeeded && opSize.Result > 0)
                {
                    Addressables.DownloadDependenciesAsync(labels[capture], true).Completed += (opDownload) =>
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

            yield return null;
        }
    }

    private void OnDownloadDone()
    {
        DebugFro.Log(className, "다운로드 완료!!");
        updateMode = eModeType.DownloadDone;

        downloadCount++;

        if (downloadCount >= labels.Length)
        {
            updateMode = eModeType.DownloadAll;
            DebugFro.Log(className, "모두 다운로드!!");
            
            managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed
                += LoadEventChannel;
        }
    }
}
