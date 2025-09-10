using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for starting the game by loading the persistent managers scene 
/// and raising the event to load the Main Menu
/// </summary>
public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameSceneSO _persistentManagerScene = default;
	[SerializeField] private GameSceneSO _menuScene = default;

	[Header("Broadcasting on")]
	[SerializeField] private AssetReferenceScriptableObject _loadMenuSceneChannel = default;

    private const int FIRST_SCENE_INDEX = 0;

	private void Start()
	{
		//Load the persistent managers scene
		_persistentManagerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
	}

	private void LoadEventChannel(AsyncOperationHandle<SceneInstance> operationHandle)
	{
		_loadMenuSceneChannel.LoadAssetAsync<LoadSceneEventChannelSO>().Completed += LoadMainMenu;
	}

	private void LoadMainMenu(AsyncOperationHandle<LoadSceneEventChannelSO> operationHandle)
	{
		operationHandle.Result.RaiseEvent(_menuScene, true);

		SceneManager.UnloadSceneAsync(FIRST_SCENE_INDEX); //Bootsrtap is the only scene in BuildSettings, thus it has index 0
	}
}