using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _mapSelectPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource _buttonClickSound;
    [SerializeField] private AudioSource _backgroundMusic;

    private void Start()
    {
        // Ensure main menu is active on start
        ShowMainMenu();

        // Start background music if assigned
        if (_backgroundMusic != null && !_backgroundMusic.isPlaying)
        {
            _backgroundMusic.Play();
        }
    }

    private void Update()
    {
        // Handle ESC key to return to previous menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeKey();
        }
    }

    public void StartGame()
    {
        PlayButtonSound();
        // Load the first game map or gameplay scene
        SceneManager.LoadScene("GameplayScene");
    }

    public void ShowMapSelect()
    {
        PlayButtonSound();
        _mainMenuPanel.SetActive(false);
        _mapSelectPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        PlayButtonSound();
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        PlayButtonSound();
        _mainMenuPanel.SetActive(false);
        _creditsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        PlayButtonSound();
        _mainMenuPanel.SetActive(true);
        _settingsPanel.SetActive(false);
        _creditsPanel.SetActive(false);
        _mapSelectPanel.SetActive(false);
    }

    public void ExitGame()
    {
        PlayButtonSound();
        // Show confirmation dialog before exiting
        ShowExitConfirmation();
    }

    private void ShowExitConfirmation()
    {
        // In a production game, you would show a custom dialog
        // For now, we'll use Application.Quit() directly
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void HandleEscapeKey()
    {
        if (_settingsPanel.activeSelf || _creditsPanel.activeSelf || _mapSelectPanel.activeSelf)
        {
            ShowMainMenu();
        }
    }

    private void PlayButtonSound()
    {
        if (_buttonClickSound != null)
        {
            _buttonClickSound.Play();
        }
    }
}