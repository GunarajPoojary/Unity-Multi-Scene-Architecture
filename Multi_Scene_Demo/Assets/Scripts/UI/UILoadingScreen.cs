using UnityEngine;

public class UILoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _container;

    public void Show()
    {
        _container.SetActive(true);
    }

    public void Hide()
    {
        _container.SetActive(false);
    }
}