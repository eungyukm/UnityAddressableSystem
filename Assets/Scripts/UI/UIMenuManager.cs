using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private Button startBtn = default;

    [SerializeField] private GameplaySO demoToLoad = default;

    [SerializeField] private UIMainMenu _mainMenuPanel = default;

    [Header("Broadcasting on")]
    [SerializeField] private LoadEventChannelSO _startNewGameEvent = default;


    private IEnumerator Start()
    {
        yield return null;
        _mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
    }

    private void ButtonStartNewGameClicked()
    {
        throw new NotImplementedException();
    }

    private void NextScene()
    {

    }
}
