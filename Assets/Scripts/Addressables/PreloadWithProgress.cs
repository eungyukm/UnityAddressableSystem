using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PreloadWithProgress : MonoBehaviour
{
    public AssetLabelReference labelReference;
    public UnityAction<long> preLoadComplete;
    public UnityAction<float> progressAction;
    public UnityAction<bool> completionDownloadAction;
    private AsyncOperationHandle downloadHandle;

    private IEnumerator Start()
    {
        Coroutine preLoadRoutine = StartCoroutine(PreLoad(preLoadComplete));
        yield return preLoadRoutine;

        Debug.Log("PreLoad Complete!!");

        Coroutine downLoadRoutine = StartCoroutine(DownLoad(completionDownloadAction));
        yield return downLoadRoutine;
    }

    private IEnumerator PreLoad(UnityAction<long> callback)
    {
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(labelReference);
        yield return getDownloadSize;

        Debug.Log("Download Size : " + getDownloadSize.Result);

        callback?.Invoke(getDownloadSize.Result);
    }

    private IEnumerator DownLoad(UnityAction<bool> callback)
    {
        downloadHandle = Addressables.DownloadDependenciesAsync(labelReference, false);
        float progress = 0;

        while (downloadHandle.Status == AsyncOperationStatus.None)
        {
            float percentageComplete = downloadHandle.GetDownloadStatus().Percent;
            if (percentageComplete > progress * 1.1)
            {
                progress = percentageComplete;
                progressAction.Invoke(progress);
            }
            yield return null;
        }

        callback?.Invoke(downloadHandle.Status == AsyncOperationStatus.Succeeded);
        Addressables.Release(downloadHandle);
    }
}
