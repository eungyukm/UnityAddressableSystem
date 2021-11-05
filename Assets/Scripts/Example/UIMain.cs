using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField] private Button startBtn = default;

    [SerializeField] private GameplaySO demoToLoad = default;
    
    [Header("Broadcasting on")] 
    [SerializeField] private LoadEventChannelSO loadDemo = default;
     
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            loadDemo.RaiseEvent(demoToLoad, true);
        });
    }

    private void NextScene()
    {
        
    }
}
