using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the scene loading and unloading.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] private LoadSceneEventChannelSO _loadSceneChannel = default;
    [SerializeField] private UILoadingScreen _loadingScreen;

    private AsyncOperationHandle<SceneInstance> _sceneLoadingOperationHandle;

    private GameSceneSO _sceneToLoad;
    private GameSceneSO _currentlyLoadedScene;

    private readonly float _fadeDuration = 0.5f;
    private bool _isLoading = false;

    private void OnEnable()
    {
        _loadSceneChannel.OnLoadingRequested += LoadScene;
    }

    private void OnDisable()
    {
        _loadSceneChannel.OnLoadingRequested -= LoadScene;
    }

    /// <summary>
    /// This function loads the scenes passed as array parameter
    /// </summary>
    private void LoadScene(GameSceneSO sceneToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        //Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
        if (_isLoading)
            return;

        _isLoading = true;

        if (showLoadingScreen)
            _loadingScreen.Show();

        _sceneToLoad = sceneToLoad;

        StartCoroutine(UnloadPreviousScene());
    }

    /// <summary>
    /// In both Map and Menu loading, this function takes care of removing previously loaded scenes.
    /// </summary>
    private IEnumerator UnloadPreviousScene()
    {
        yield return new WaitForSeconds(_fadeDuration);

        if (_currentlyLoadedScene != null) //would be null if the game was started in Initialisation
        {
            if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
            {
                //Unload the scene through its AssetReference, i.e. through the Addressable system
                _currentlyLoadedScene.sceneReference.UnLoadScene();
            }
        }

        LoadNewScene();
    }

    /// <summary>
    /// Kicks off the asynchronous loading of a scene, either menu or Map.
    /// </summary>
    private void LoadNewScene()
    {
        _sceneLoadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
        _sceneLoadingOperationHandle.Completed += OnNewSceneLoaded;
    }

    private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> operationHandle)
    {
        _loadingScreen.Hide();

        //Save loaded scenes (to be unloaded at next load request)
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