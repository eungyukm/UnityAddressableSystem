using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button _NewGameButton = default;

    public UnityAction NewGameButtonAction;

    public void NewGameButton()
    {
        NewGameButtonAction.Invoke();
    }
}
