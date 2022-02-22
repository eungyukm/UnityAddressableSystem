using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 새로운 게임을 시작하는 클래스
/// </summary>
public class StartGame : MonoBehaviour
{
    // 다음으로 로드할 Locatios SO
    [SerializeField] private LocationSO _locationsToLoad;

    [Header("Broadcasting on")] 
    [SerializeField] private LoadEventChannelSO _startGameEvent = default;
    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _startNewGameEvent = default;

    [SerializeField] private bool _showLoadScreen = default;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("StartGame Call!!");
        _startNewGameEvent.OnEventRaised += StartNewGame;   
    }

    private void OnDestroy()
    {
        _startNewGameEvent.OnEventRaised -= StartNewGame;
    }

    private void StartNewGame()
    {
        Debug.Log("StartNewGame Call!!");
        _startGameEvent.RaiseEvent(_locationsToLoad, _showLoadScreen);
    }
}
