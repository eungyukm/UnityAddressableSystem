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
    
    [Header("Listening to")]
    [SerializeField] private LoadEventChannelSO loadLocation = default;
    [SerializeField] private LoadEventChannelSO loadMenu = default;

    [Header("Broadcasting on")] 
    [SerializeField] private VoidEventChannelSO onSceneReady = default;

    // TODO : AsyncOperationHandle 찾아보기
    // TODO : SceneInstantce 찾아보기
    // 로딩하는 Scene의 핸들러
    private AsyncOperationHandle<SceneInstance> loadingOperationHandle;
    private AsyncOperationHandle<SceneInstance> gameplayManagerLoadingOpHandle;

    private GameSceneSO sceneToLoad;
    private GameSceneSO currentlyLoadScene;
    private bool showLoadingScene;

    private SceneInstance gameplayManagerSceneInstance = new SceneInstance();
    private float fadeDuration = .5f;
    // 새로운 로딩 요청을 막기 위해 존재
    private bool isLoading = false;
    
    #if UNITY_EDITOR
    private void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen, bool fadeSceen)
    {
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
        loadLocation.OnLoadingRequested += LoadLocation;
        loadMenu.OnLoadingRequested += LoadMenu;
    }

    private void OnDisable()
    {
        loadLocation.OnLoadingRequested -= LoadLocation;
        loadMenu.OnLoadingRequested -= LoadMenu;
    }

    private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        Debug.Log("[SceneLoader] LoadLocation Call!!");
        // 이중 로딩 방지
        if (isLoading)
        {
            return;
        }

        sceneToLoad = locationToLoad;
        showLoadingScene = showLoadingScreen;
        // 현재 로딩중
        isLoading = true;

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
        //Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
        if (isLoading)
            return;

        sceneToLoad = menuToLoad;
        showLoadingScreen = showLoadingScreen;
        isLoading = true;


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
        // TODO : fadeRequestChannel 알아 보기

        yield return new WaitForSeconds(fadeDuration);

        if (currentlyLoadScene != null)
        {
            if (currentlyLoadScene.sceneReference.OperationHandle.IsValid())
            {
                Debug.Log("[SceneLoader] UnLoadScene");
                currentlyLoadScene.sceneReference.UnLoadScene();
            }
        }

        LoadNewScene();
    }
    
    /// <summary>
    /// 비동기로 Scene을 로드
    /// </summary>
    private void LoadNewScene()
    {
        Debug.Log("[SceneLoader] 새로운 씬 로드 시작!");
        // TODO : showLoadingScreen 찾아보기

        loadingOperationHandle = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
        loadingOperationHandle.Completed += OnNewSceneLoaded;
    }

    private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        // 현재 로딩 Scene 저장
        currentlyLoadScene = sceneToLoad;

        Scene scene = obj.Result.Scene;
        // TODO : SceneManager.SetActiveScene 찾아보기
        SceneManager.SetActiveScene(scene);

        isLoading = false;

        if (showLoadingScene)
        {
            
        }
        
        // TODO : fadeRequestChannel 뭔지 찾아보기
        StartGameplay();
    }
    
    private void StartGameplay()
    {
        onSceneReady.RaiseEvent();
    }
}
