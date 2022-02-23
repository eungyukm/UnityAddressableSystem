using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

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

    [Header("preload with progress")]
    [SerializeField] private PreloadWithProgress preloadWithProgress;

    private void OnEnable()
    {
        sliderChangeValueEvent.OnEventRaised += SetLoadingBar;
        DownLoadDone.OnEventRaised += SetDownloadDoneUI;

        preloadWithProgress.preLoadComplete += SetUPDownloadUI;
        preloadWithProgress.progressAction += SetUPLoadingBar;
    }


    private void OnDisable()
    {
        sliderChangeValueEvent.OnEventRaised -= SetLoadingBar;
        DownLoadDone.OnEventRaised -= SetDownloadDoneUI;

        preloadWithProgress.preLoadComplete -= SetUPDownloadUI;
        preloadWithProgress.progressAction -= SetUPLoadingBar;
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

    private void SetUPLoadingBar(float value)
    {
        Debug.Log("value : " + value);
    }


    private void SetUPDownloadUI(long downloadBar)
    {
        Debug.Log("Bar : " + downloadBar);
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
            UnityEngine.Debug.Log("!!");
            downBefore.SetActive(false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            UnityEngine.Debug.Log("@@");
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
