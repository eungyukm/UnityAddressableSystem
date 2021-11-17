using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;

public class AddressablesBundleDelete : MonoBehaviour
{
    [SerializeField] private bool playOnBundleDelete;
    [SerializeField] private AssetLabelReference[] labels;

    [Header("Event Channel")]
    public VoidEventChannelSO onDelete;

    [SerializeField] private bool DestroyBundle;

    private void OnEnable()
    {
        onDelete.OnEventRaised += BundleDelete;
    }

    private void OnDisable()
    {
        onDelete.OnEventRaised -= BundleDelete;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playOnBundleDelete)
        {
            BundleDelete();
        }
    }

    private void OnDestroy()
    {
        if(DestroyBundle)
        {
            foreach (var label in labels)
            {
                Addressables.ClearDependencyCacheAsync(label, true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BundleDelete()
    {
        Debug.Log("ClearDependencyCacheAsync");

        //StartCoroutine(BundleDeleteRoutine());
    }

    //private IEnumerator BundleDeleteRoutine()
    //{
    //    if(Application.platform == RuntimePlatform.WindowsEditor)
    //    {
    //        DeleteCache();
    //    }

    //    Addressables.CleanBundleCache();
    //    //AsyncOperationHandle<bool> handle = Addressables.ClearDependencyCacheAsync(labels[0], true);
    //    //yield return handle.WaitForCompletion();

    //    yield return null;
    //}

    //private static void DeleteCache()
    //{
    //    string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    //    Debug.Log("appDataPath : " + appDataPath);

    //    string localLow = @"AppData\LocalLow";

    //    string path = @"Unity\DefaultCompany_UnityAddressableSystem";

    //    string fullPath = Path.Combine(appDataPath, localLow, path);
    //    Debug.Log(fullPath);

    //    //"C:\Users\frontis\AppData\LocalLow\Unity\DefaultCompany_UnityAddressableSystem"

    //    bool isExist = Directory.Exists(fullPath);
    //    Debug.Log("경로 : " + isExist);
    //    string[] allFiles = Directory.GetFiles(fullPath);
    //    RemoveAllFile(allFiles);

    //    RootFolderSearch(fullPath);
    //    //Directory.Delete(fullPath);

    //    DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);

    //    // 1. 경로 내 파일이 있는지 체크
    //}

    ///// <summary>
    ///// 최상위 폴더 탐색
    ///// </summary>
    ///// <param name="fullPath"></param>
    //private static void RootFolderSearch(string fullPath)
    //{
    //    string[] allFiles = Directory.GetFiles(fullPath);
    //    RemoveAllFile(allFiles);

    //    string[] allDirectories = Directory.GetDirectories(fullPath);
    //    for (int i = 0; i < allDirectories.Length; i++)
    //    {
    //        Debug.Log("directory : " + allDirectories[i]);
    //    }
    //}

    //private static void RemoveAllFile(string[] allFiles)
    //{
    //    for (int i = 0; i < allFiles.Length; i++)
    //    {
    //        Debug.Log("file : " + allFiles[i]);
    //        File.Delete(allFiles[i]);
    //    }
    //}
}
