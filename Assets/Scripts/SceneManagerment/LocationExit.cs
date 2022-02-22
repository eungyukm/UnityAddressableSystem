using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationExit : MonoBehaviour
{
    [SerializeField] private GameSceneSO _locationToLoad = default;
    //[SerializeField] private PathSO

    [Header("Broadcasting on")]
    [SerializeField] private LoadEventChannelSO _locationExitLoadChannel = default;

    private void OnTriggerEnter(Collider other)
    {
        _locationExitLoadChannel.RaiseEvent(_locationToLoad, false, true);
    }
}
