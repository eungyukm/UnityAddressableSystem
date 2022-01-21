using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class AddressablesDownloader : MonoBehaviour
{
    private DownloadState downloadState = DownloadState.None;

    public float totalDownloadSize = 0f;
    public float persent = 0f;
    public float maxSize = 0f;

    public AssetLabelReference[] labelReferences;

    [Header("Event Channel")]
    public VoidEventChannelSO downloadDone;
    public VoidEventChannelSO onDownload;
    public VoidEventChannelSO onCheckSize;
    public VoidEventChannelSO onSetList;
    public FloatEventChannelSO downLoadBarEvent;

    [SerializeField] private bool playOnDownload;

    [SerializeField] private bool DestroyBundle;

    private void OnEnable()
    {
        onDownload.OnEventRaised += CheckTotalDownloadSize;
    }

    private void OnDisable()
    {
        onDownload.OnEventRaised -= CheckTotalDownloadSize;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(playOnDownload)
        {
            CheckTotalDownloadSize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckTotalDownloadSize()
    {
        StartCoroutine(WaitCheckTotalDownloadSizeRoutine(CheckDownloadSizeDone));
    }

    private void StartDownload()
    {
        StartCoroutine(StartDownloadRoutine(downloadDone.RaiseEvent));
    }

    /// <summary>
    /// 3. 번들 다운로드 시작
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartDownloadRoutine(UnityAction done)
    {
        for (int i = 0; i < labelReferences.Length; i++)
        {
            yield return CheckDownloadSizeRoutine(labelReferences[i].labelString);
            yield return DownloadRoutine(labelReferences[i].labelString);
        }

        done?.Invoke();
    }

    /// <summary>
    /// 5. 번들 개별 다운로드
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    private IEnumerator DownloadRoutine(string label)
    {
        var downloadHandle = Addressables.DownloadDependenciesAsync(label);

        downloadHandle.Completed += (handle) =>
        {

        };

        // 용량 출력
        while(downloadHandle.PercentComplete < 1 && !downloadHandle.IsDone)
        {
            persent = downloadHandle.PercentComplete;
            Debug.Log("label : " + label + " persent : " + persent);
            downLoadBarEvent.RaiseEvent(persent);
            yield return null;
        }
    }

    /// <summary>
    /// 2. 번들 전체 사이즈 측정 종료
    /// </summary>

    private void CheckDownloadSizeDone()
    {
        Debug.Log("다운로드 사이즈 측정 종료!");
        Debug.Log("다운로드 사이즈 : " + totalDownloadSize);

        StartDownload();
    }

    /// <summary>
    /// 1. 모든 에셋 번들 용량 체크
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator WaitCheckTotalDownloadSizeRoutine(UnityAction callback)
    {
        yield return CheckTotalDownloadSizeRoutine("PreLoad");
        callback?.Invoke();
    }

    /// <summary>
    /// 전체 용량 체크
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    private IEnumerator CheckTotalDownloadSizeRoutine(string label)
    {
        var sizeCheckHandle = Addressables.GetDownloadSizeAsync(label);

        downloadState = DownloadState.CheckDownloadSize;
        sizeCheckHandle.Completed += (handle) =>
        {
            if (totalDownloadSize < sizeCheckHandle.Result * 0.000001f)
            {
                totalDownloadSize = sizeCheckHandle.Result * 0.000001f;
            }
            else
            {
                Debug.LogWarning("다운로드 사이즈가 너무 작습니다.");
            }
            Addressables.Release(handle);
            downloadState = DownloadState.CheckDownloadSizeDone;
        };
        while (downloadState == DownloadState.CheckDownloadSize)
        {
            yield return null;
        }
    }


    /// <summary>
    /// 4. 개별 다운로드 사이즈 체크
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    private IEnumerator CheckDownloadSizeRoutine(string label)
    {
        var downloadHandle = Addressables.GetDownloadSizeAsync(label);

        downloadState = DownloadState.CheckDownloadSize;
        downloadHandle.Completed += (handle) =>
        {
            maxSize = handle.Result * 0.000001f;

            Addressables.Release(handle);
            downloadState = DownloadState.CheckDownloadSizeDone;
        };
        while (downloadState == DownloadState.CheckDownloadSizeDone)
        {
            yield return null;
        }
    }

    //private IEnumerator 

    public enum DownloadState
    {
        None,
        CheckTotalDownloadSize,
        CheckDownloadSize,
        CheckTotalDownloadSizeDone,
        CheckDownloadSizeDone,
        Downloading,
        Done
    }
}
