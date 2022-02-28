using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private UIMainMenu _mainMenuPanel = default;

    [Header("Broadcasting on")]
    [SerializeField]
    private VoidEventChannelSO _startNewGameEvent = default;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.4f);
        _mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
    }

    private void ButtonStartNewGameClicked()
    {
        Debug.Log("MainMenu Call!!");
        _startNewGameEvent.RaiseEvent();
    }
}
