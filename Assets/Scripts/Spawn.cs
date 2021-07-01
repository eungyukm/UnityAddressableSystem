using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Spawn : MonoBehaviour
{
    public AssetReference AssetReference;
    // Start is called before the first frame update
    void Start()
    {
        var async = Addressables.InitializeAsync();
        async.Completed += (op) =>
        {
            Addressables.Release(async);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            AssetReference.Instantiate(Vector3.zero, Quaternion.identity, null);
        }
    }
}
