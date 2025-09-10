using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the scene loading and unloading.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameSceneSO _gameplayScene = default;

    [Header("Listening to")]
    [SerializeField] private LoadSceneEventChannelSO _loadMapSceneChannel = default;
    [SerializeField] private LoadSceneEventChannelSO _loadMenuSceneChannel = default;
    [SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;

    private AsyncOperationHandle<SceneInstance> _sceneLoadingOperationHandle;
    private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

    private GameSceneSO _sceneToLoad;
    private GameSceneSO _currentlyLoadedScene;
    private bool _showLoadingScreen;
    private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();

    private readonly float _fadeDuration = 0.5f;
    private bool _isLoading = false;

    private void OnEnable()
    {
        _loadMapSceneChannel.OnLoadingRequested += LoadMap;
        _loadMenuSceneChannel.OnLoadingRequested += LoadMenu;
    }

    private void OnDisable()
    {
        _loadMapSceneChannel.OnLoadingRequested -= LoadMap;
        _loadMenuSceneChannel.OnLoadingRequested -= LoadMenu;
    }

    private void LoadMap(GameSceneSO sceneToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        StartSceneLoading(sceneToLoad, showLoadingScreen, fadeScreen, loadGameplayManager: true);
    }

    private void LoadMenu(GameSceneSO sceneToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        StartSceneLoading(sceneToLoad, showLoadingScreen, fadeScreen, loadGameplayManager: false);
    }

    private void StartSceneLoading(GameSceneSO sceneToLoad, bool showLoadingScreen, bool fadeScreen, bool loadGameplayManager)
    {
        if (_isLoading)
            return;

        _isLoading = true;
        _showLoadingScreen = showLoadingScreen;
        _sceneToLoad = sceneToLoad;

        if (loadGameplayManager)
        {
            if (_gameplayManagerSceneInstance.Scene == null || !_gameplayManagerSceneInstance.Scene.isLoaded)
            {
                _gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
                return;
            }
        }
        else
        {
            if (_gameplayManagerSceneInstance.Scene != null && _gameplayManagerSceneInstance.Scene.isLoaded)
            {
                Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);
            }
        }

        StartCoroutine(UnloadPreviousScene());
    }

    private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> operationHandle)
    {
        _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;
        StartCoroutine(UnloadPreviousScene());
    }

    private IEnumerator UnloadPreviousScene()
    {
        yield return new WaitForSeconds(_fadeDuration);

        if (_currentlyLoadedScene != null && _currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
        {
            _currentlyLoadedScene.sceneReference.UnLoadScene();
        }

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        if (_showLoadingScreen)
        {
            _toggleLoadingScreen.RaiseEvent(true);
        }

        _sceneLoadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
        _sceneLoadingOperationHandle.Completed += OnNewSceneLoaded;
    }

    private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> operationHandle)
    {
        _currentlyLoadedScene = _sceneToLoad;

        Scene loadedScene = operationHandle.Result.Scene;
        SceneManager.SetActiveScene(loadedScene);

        _isLoading = false;

        OnSceneLoad();
    }

    private void OnSceneLoad()
    {
        Debug.Log("Load new Scene");
    }
}