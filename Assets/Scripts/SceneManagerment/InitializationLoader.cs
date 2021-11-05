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

    private eModeType updateMode;

    private const string className = "InitializationLoader";
    
    private int downloadCount = 0;

    [SerializeField] private AssetLabelReference[] labels;

    [SerializeField] private bool useDownload;
    [SerializeField] private bool useClearCache;
    [SerializeField] private bool useClearDependencyCache;
    [SerializeField] private bool useCatalog;

    [SerializeField] private Text log;
    private void Awake()
    {
        DebugFro.isLogVisable = true;

        updateMode = eModeType.None;

        foreach (var label in labels)
        {
            DebugFro.Log(label.labelString);
        }

        ClearCahe();

        if (useCatalog)
        {
            StartCoroutine(UpdateCatalogs());         
        }

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

                while (updateMode == eModeType.Wait && updateMode == eModeType.DowaloadAsset)
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

            if (useDownload)
            {
                managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed
                    += LoadEventChannel;
            }
            else
            {
                // Remote 인 경우 다운로드도 하는 기능을 가지고 있음
                managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed
                    += LoadEventChannel;
            }

        }
    }

    private void ClearCahe()
    {
        if (useClearCache)
        {
            Caching.ClearCache();
        }

        if (useClearDependencyCache)
        {
            foreach (var label in labels)
            {
                Addressables.ClearDependencyCacheAsync(label);
            }
        }
    }

    IEnumerator UpdateCatalogs()
    {
        List<string> catalogsToUpdate = new List<string>();
        AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates();
        checkForUpdateHandle.Completed += op =>
        {
            DebugFro.Log(className, op.Result.Capacity.ToString());
            catalogsToUpdate.AddRange(op.Result);
        };
        yield return checkForUpdateHandle;
        if (catalogsToUpdate.Count > 0)
        {
            DebugFro.Log(className, "Update 할 내역이 있습니다.");
            log.text = $"Update 할 내역이 있습니다.";
            AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate);
            yield return updateHandle;
        }
        else
        {
            log.text = "Update 할 내역이 없습니다.";
            DebugFro.Log(className, "Update 할 내역이 없습니다.");
        }
    }
}
