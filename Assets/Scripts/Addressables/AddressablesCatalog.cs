using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesCatalog : MonoBehaviour
{
    //[SerializeField] private ContentCatalogData catalogData;
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(UpdateCatalogs());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UpdateCatalogs()
    {
        yield return Addressables.InitializeAsync();
        List<string> catalogsToUpdate = new List<string>();
        AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates();
        checkForUpdateHandle.Completed += op =>
        {
            UnityEngine.Debug.Log(op.Result.Capacity.ToString());
            catalogsToUpdate.AddRange(op.Result);

            if (catalogsToUpdate.Count > 0)
            {
                UnityEngine.Debug.Log("Update 할 내역이 있습니다.");
            }
        };
        yield return checkForUpdateHandle;
        if (catalogsToUpdate.Count > 0)
        {
            UnityEngine.Debug.Log("Update 할 내역이 있습니다.");
            AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate);
            yield return updateHandle;
        }
        else
        {
            UnityEngine.Debug.Log("Update 할 내역이 없습니다.");
        }
    }
}
