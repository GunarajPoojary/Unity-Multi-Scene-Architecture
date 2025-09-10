using UnityEngine;

public class UILoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;

    private void OnEnable()
    {
        _toggleLoadingScreen.OnEventRaised += ToggleLoadingScreen;
    }

    private void OnDisable()
    {
        _toggleLoadingScreen.OnEventRaised -= ToggleLoadingScreen;
    }

    private void ToggleLoadingScreen(bool toggle)
    {
        _container.SetActive(toggle);
    }
}