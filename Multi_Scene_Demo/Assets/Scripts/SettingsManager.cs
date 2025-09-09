using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
// Settings Manager - Handles all game settings and preferences
public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer _masterMixer;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI _masterVolumeText;
    [SerializeField] private TextMeshProUGUI _musicVolumeText;
    [SerializeField] private TextMeshProUGUI _sfxVolumeText;
    
    [Header("Graphics Settings")]
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    [SerializeField] private Toggle _fullscreenToggle;
    
    [Header("Default Values")]
    [SerializeField] private float _defaultMasterVolume = 0.75f;
    [SerializeField] private float _defaultMusicVolume = 0.6f;
    [SerializeField] private float _defaultSfxVolume = 0.8f;
    
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string QUALITY_KEY = "QualityLevel";
    private const string FULLSCREEN_KEY = "Fullscreen";

    public AudioMixer MasterMixer { get { return _masterMixer; } set { _masterMixer = value; } }
    public Slider SfxVolumeSlider { get { return _sfxVolumeSlider;} set { _sfxVolumeSlider = value;}}
    public Slider MasterVolumeSlider { get { return _masterVolumeSlider;} set{ _masterVolumeSlider = value;}}
    public Slider MusicVolumeSlider { get { return _musicVolumeSlider;} set {_musicVolumeSlider = value;}}

    private void Start()
    {
        LoadSettings();
        SetupSliderListeners();
    }
    
    private void SetupSliderListeners()
    {
        if (_masterVolumeSlider != null)
            _masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        
        if (_musicVolumeSlider != null)
            _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        
        if (_sfxVolumeSlider != null)
            _sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        
        if (_qualityDropdown != null)
            _qualityDropdown.onValueChanged.AddListener(SetQuality);
        
        if (_fullscreenToggle != null)
            _fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }
    
    public void LoadSettings()
    {
        // Load volume settings
        float masterVol = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, _defaultMasterVolume);
        float musicVol = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, _defaultMusicVolume);
        float sfxVol = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, _defaultSfxVolume);
        
        // Apply volume settings
        SetMasterVolume(masterVol);
        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);
        
        // Update UI sliders
        if (_masterVolumeSlider != null) _masterVolumeSlider.value = masterVol;
        if (_musicVolumeSlider != null) _musicVolumeSlider.value = musicVol;
        if (_sfxVolumeSlider != null) _sfxVolumeSlider.value = sfxVol;
        
        // Load graphics settings
        int qualityLevel = PlayerPrefs.GetInt(QUALITY_KEY, QualitySettings.GetQualityLevel());
        bool isFullscreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, Screen.fullScreen ? 1 : 0) == 1;
        
        SetQuality(qualityLevel);
        SetFullscreen(isFullscreen);
        
        if (_qualityDropdown != null) _qualityDropdown.value = qualityLevel;
        if (_fullscreenToggle != null) _fullscreenToggle.isOn = isFullscreen;
    }
    
    public void SetMasterVolume(float volume)
    {
        if (_masterMixer != null)
        {
            float dbValue = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            _masterMixer.SetFloat("MasterVolume", dbValue);
        }
        
        if (_masterVolumeText != null)
            _masterVolumeText.text = Mathf.RoundToInt(volume * 100f).ToString();
        
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        if (_masterMixer != null)
        {
            float dbValue = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            _masterMixer.SetFloat("MusicVolume", dbValue);
        }
        
        if (_musicVolumeText != null)
            _musicVolumeText.text = Mathf.RoundToInt(volume * 100f).ToString();
        
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
    }
    
    public void SetSFXVolume(float volume)
    {
        if (_masterMixer != null)
        {
            float dbValue = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
            _masterMixer.SetFloat("SFXVolume", dbValue);
        }
        
        if (_sfxVolumeText != null)
            _sfxVolumeText.text = Mathf.RoundToInt(volume * 100f).ToString();
        
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
    }
    
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt(QUALITY_KEY, qualityIndex);
    }
    
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
    }
    
    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) SaveSettings();
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) SaveSettings();
    }
}