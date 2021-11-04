using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesExample : MonoBehaviour
{
    [SerializeField]private GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        Addressables.LoadAssetAsync<GameObject>("Cube").Completed += OnLoadDone;
    }

    private void OnLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        cube = obj.Result;
    }
}
