using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressablesLoad : MonoBehaviour
{
    public VoidEventChannelSO downloadDone;

    private void OnEnable()
    {
        downloadDone.OnEventRaised += LoadStart;
    }

    /// <summary>
    /// 1. 로드 시작
    /// </summary>
    private void LoadStart()
    {
        Debug.Log("Load 시작!");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
