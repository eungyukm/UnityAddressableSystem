using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InitializationLoader : MonoBehaviour
{
    [SerializeField] private GameSceneSO managerScene = default;
    [SerializeField] private GameSceneSO menuToLoad = default;
    
    [SerializeField] private AssetReference menuLoadChannel = default;

    private ModeType updateMode;
    
    private int downloadCount = 0;

    [SerializeField] private AssetLabelReference[] labels;
    [SerializeField] private AssetLabelReference preLoad;

    [SerializeField] private bool _useClearCache;
    [SerializeField] private bool _useCatalog;

    [SerializeField] private Text log;

    [SerializeField] private VoidEventChannelSO downloadDone;

    [SerializeField] private bool _preventSceneLoad;

    [SerializeField] private bool _preLoad;
    private void Awake()
    {
        Debug.isLogVisable = true;

        updateMode = ModeType.None;

        foreach (var label in labels)
        {
            Debug.Log(label.labelString);
        }

        ClearCahe();
        ClearDependency();
    }

    private void OnEnable()
    {
        downloadDone.OnEventRaised += SceneLoad;
    }

    private void OnDisable()
    {
        downloadDone.OnEventRaised -= SceneLoad;
    }

    private void SceneLoad()
    {
        if(!_preventSceneLoad)
        {
            managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
        }
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
    
    public enum ModeType
    {
        None,
        DowaloadAsset,
        Wait,
        DownloadDone,
        DownloadAll,
    }

    private IEnumerator BundleDownload()
    {
        Debug.Log("다운로드 시작!");
        for (int i = 0; i < labels.Length; i++)
        {
            int capture = i;
            Addressables.GetDownloadSizeAsync(labels[capture]).Completed += (opSize) =>
            {
                Debug.Log("{labels[capture].labelString} Size : " + string.Concat(opSize.Result, " byte"));

                while (updateMode == ModeType.Wait && updateMode == ModeType.DowaloadAsset)
                {
                    new WaitForSeconds(1f);
                }
                
                
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
            updateMode = ModeType.Wait;

            yield return null;
        }
    }

    private void OnDownloadDone()
    {
        Debug.Log("다운로드 완료!!");
        updateMode = ModeType.DownloadDone;

        downloadCount++;

        if (downloadCount >= labels.Length)
        {
            updateMode = ModeType.DownloadAll;
            Debug.Log("모두 다운로드!!");
        }
    }

    private void ClearCahe()
    {
        if (_useClearCache)
        {
            Caching.ClearCache();
        }
    }

    private void ClearDependency()
    {
        if(_preLoad)
        {
            Addressables.ClearDependencyCacheAsync(preLoad);
        }
    }
}
