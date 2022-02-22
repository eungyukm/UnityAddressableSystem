using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneLoader : MonoBehaviour
{
    [FormerlySerializedAs("gameSceneScene")] [SerializeField] private GameSceneSO gamePlayScene;
    
    [Header("Load Events")]
    [SerializeField] private LoadEventChannelSO _loadLocation = default;
    [SerializeField] private LoadEventChannelSO _loadMenu = default;
    
    // Demo
    [SerializeField] private LoadEventChannelSO _loadDemo = default;

    [Header("Broadcasting on")] 
    [SerializeField] private VoidEventChannelSO onSceneReady = default;
    
    // 로딩하는 Scene의 핸들러
    private AsyncOperationHandle<SceneInstance> loadingOperationHandle;
    private AsyncOperationHandle<SceneInstance> gameplayManagerLoadingOpHandle;

    private GameSceneSO _sceneToLoad;
    private GameSceneSO currentlyLoadScene;
    private bool _showLoadingScene;

    private SceneInstance gameplayManagerSceneInstance = new SceneInstance();
    private float fadeDuration = .5f;
    // 새로운 로딩 요청을 막기 위해 존재
    private bool _isLoading = false;

    [SerializeField] private GameObject[] objs;

#if UNITY_EDITOR
    private void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen, bool fadeSceen)
    {
        Debug.Log("[SceneLoader] OnLoader StartUP Call");
        currentlyLoadScene = currentlyOpenedLocation;

        if (currentlyLoadScene.sceneType == GameSceneSO.eGameSceneType.Location)
        {
            gameplayManagerLoadingOpHandle = gamePlayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
            gameplayManagerLoadingOpHandle.WaitForCompletion();
            gameplayManagerSceneInstance = gameplayManagerLoadingOpHandle.Result;
            
            StartGameplay();
        }
    }
    #endif

    private void OnEnable()
    {
        Debug.isLogVisable = true;
        
        _loadLocation.OnLoadingRequested += LoadLocation;
        _loadMenu.OnLoadingRequested += LoadMenu;
        _loadDemo.OnLoadingRequested += LoadDemo;
    }

    private void OnDisable()
    {
        _loadLocation.OnLoadingRequested -= LoadLocation;
        _loadMenu.OnLoadingRequested -= LoadMenu;
        _loadDemo.OnLoadingRequested -= LoadDemo;
    }

    private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        Debug.Log("LoadLocation Call!!");
        // 이중 로딩 방지
        if (_isLoading)
        {
            return;
        }

        _sceneToLoad = locationToLoad;
        _showLoadingScene = showLoadingScreen;
        // 현재 로딩중
        _isLoading = true;

        if (gameplayManagerSceneInstance.Scene == null ||
            !gameplayManagerSceneInstance.Scene.isLoaded)
        {
            gameplayManagerLoadingOpHandle = gamePlayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
            gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
        }
        else
        {
            StartCoroutine(UnloadPreviousScene());
        }
    }
    
    private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        Debug.Log("LoadMenu Call!!");
        
        //Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
        if (_isLoading)
            return;

        _sceneToLoad = menuToLoad;
        showLoadingScreen = showLoadingScreen;
        _isLoading = true;


        if (gameplayManagerSceneInstance.Scene != null
            && gameplayManagerSceneInstance.Scene.isLoaded)
        {
            Addressables.UnloadSceneAsync(gameplayManagerLoadingOpHandle, true);           
        }


        StartCoroutine(UnloadPreviousScene());
    }
    
    // Demo
    private void LoadDemo(GameSceneSO demoToLoad, bool showLoadingScreen, bool fadeScreen)
    {        
        //Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
        if (_isLoading)
            return;

        _sceneToLoad = demoToLoad;
        showLoadingScreen = showLoadingScreen;
        _isLoading = true;


        if (gameplayManagerSceneInstance.Scene != null
            && gameplayManagerSceneInstance.Scene.isLoaded)
        {
            Addressables.UnloadSceneAsync(gameplayManagerLoadingOpHandle, true);           
        }


        StartCoroutine(UnloadPreviousScene());
    }

    private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        gameplayManagerSceneInstance = gameplayManagerLoadingOpHandle.Result;

        StartCoroutine(UnloadPreviousScene());
    }

    private IEnumerator UnloadPreviousScene()
    {
        DestroyDemo();
        // TODO : fadeRequestChannel 하기

        yield return new WaitForSeconds(fadeDuration);

        if (currentlyLoadScene != null)
        {
            if (currentlyLoadScene.sceneReference.OperationHandle.IsValid())
            {
                UnityEngine.Debug.Log("[SceneLoader] UnLoadScene");
                currentlyLoadScene.sceneReference.UnLoadScene();
            }
        }

        LoadNewScene();
    }

    /// <summary>
    /// 데모 오브젝트 제거
    /// </summary>
    private void DestroyDemo()
    {
        if (objs.Length > 0)
        {
            foreach (var o in objs)
            {
                Destroy(o);
            }
        }
    }

    /// <summary>
    /// 비동기로 Scene을 로드
    /// </summary>
    private void LoadNewScene()
    {
        Debug.Log( "LoadNewScene Call!!");
        // TODO : showLoadingScreen 찾아보기

        loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
        loadingOperationHandle.Completed += OnNewSceneLoaded;
    }

    private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        Debug.Log("OnNewSceneLoaded Call!!");
        // 현재 로딩 Scene 저장
        currentlyLoadScene = _sceneToLoad;

        Scene scene = obj.Result.Scene;
        // Scene 활성화
        SceneManager.SetActiveScene(scene);

        _isLoading = false;

        if (_showLoadingScene)
        {
            
        }
        
        // TODO : fadeRequestChannel 뭔지 찾아보기
        StartGameplay();
    }
    
    private void StartGameplay()
    {
        Debug.Log("StartGameplay Call!!");
        onSceneReady.RaiseEvent();
    }
}
