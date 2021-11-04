using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetReferenceExample : MonoBehaviour
{
    [SerializeField] private AssetReference assetReference;
    
    // Start is called before the first frame update
    void Start()
    {
        assetReference.LoadAssetAsync<GameObject>();
        assetReference.InstantiateAsync(Vector3.zero, Quaternion.identity);
    }
}
