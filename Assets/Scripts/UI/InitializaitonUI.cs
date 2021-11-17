using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InitializaitonUI : MonoBehaviour
{
    [Header("Buton Demo")]
    [SerializeField] private Button bundleDownLoadButton;
    [SerializeField] private Button bundleDeleteButton;
    [SerializeField] private Button bundleSetListButton;
    [SerializeField] private Button bundleCheckSizeButton;

    [Header("Buton")]
    [SerializeField] private Button DownLoadButton;

    [Header("Event Channel")]
    [SerializeField] private VoidEventChannelSO OnDownLoad;
    [SerializeField] private VoidEventChannelSO DownLoadDone;
    [SerializeField] private VoidEventChannelSO OnDelete;
    [SerializeField] private VoidEventChannelSO OnSetList;
    [SerializeField] private VoidEventChannelSO OnCheckSize;
    [SerializeField] private FloatEventChannelSO sliderChangeValueEvent;

    [Header("GameObject")]
    [SerializeField] private GameObject downloadPopup;
    [SerializeField] private GameObject downloading;
    [SerializeField] private GameObject downBefore;
    [SerializeField] private GameObject downAfter;

    [Header("Text")]
    [SerializeField] private TMP_Text tmpPercent;

    [Header("Slider")]
    [SerializeField] private Slider percentSlider;

    private void OnEnable()
    {
        sliderChangeValueEvent.OnEventRaised += SetLoadingBar;
        DownLoadDone.OnEventRaised += SetDownloadDoneUI;
    }

    private void OnDisable()
    {
        sliderChangeValueEvent.OnEventRaised -= SetLoadingBar;
        DownLoadDone.OnEventRaised -= SetDownloadDoneUI;
    }

    private void SetDownloadDoneUI()
    {
        downloading.SetActive(false);
        downAfter.SetActive(true);
    }

    private void SetLoadingBar(float value)
    {
        percentSlider.value = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        bundleDownLoadButton.onClick.AddListener(() =>
        {
            downloading.SetActive(true);
            OnDownLoad.RaiseEvent();
        });

        bundleDeleteButton.onClick.AddListener(() =>
        {
            OnDelete.RaiseEvent();
        });

        bundleSetListButton.onClick.AddListener(() =>
        {
        });

        bundleCheckSizeButton.onClick.AddListener(() =>
        {
        });


        DownLoadButton.onClick.AddListener(() =>
        {
            OnDownLoad.RaiseEvent();
            downloading.SetActive(true);
        });

        Init();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            Debug.Log("!!");
            downBefore.SetActive(false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("@@");
            downBefore.SetActive(true);
        }
    }

    private void Init()
    {
        downloadPopup.SetActive(true);
        downloading.SetActive(false);
        downBefore.SetActive(true);
        downAfter.SetActive(false);
    }
}
