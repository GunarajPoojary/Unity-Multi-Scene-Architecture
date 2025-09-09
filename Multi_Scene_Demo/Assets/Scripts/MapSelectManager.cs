using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MapSelectManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private MapData[] _maps;
    [SerializeField] private GameObject _mapButtonPrefab;
    [SerializeField] private Transform _mapButtonParent;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _mapNameText;
    [SerializeField] private TextMeshProUGUI _mapDescriptionText;
    [SerializeField] private Button _playMapButton;

    private int _selectedMapIndex = 0;

    [System.Serializable]
    public class MapData
    {
        public string mapName;
        public string sceneName;
        public string description;
        public Sprite previewImage;
        public bool isUnlocked = true;
    }

    private void Start()
    {
        GenerateLevelButtons();
        SelectLevel(0);
    }

    private void GenerateLevelButtons()
    {
        for (int i = 0; i < _maps.Length; i++)
        {
            GameObject buttonObj = Instantiate(_mapButtonPrefab, _mapButtonParent);
            Button levelButton = buttonObj.GetComponent<Button>();

            int levelIndex = i; // Capture for closure
            levelButton.onClick.AddListener(() => SelectLevel(levelIndex));

            // Update button text
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = _maps[i].mapName;
            }

            // Disable button if level is locked
            levelButton.interactable = _maps[i].isUnlocked;
        }
    }

    public void SelectLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= _maps.Length)
            return;

        _selectedMapIndex = levelIndex;
        MapData selectedLevel = _maps[levelIndex];

        if (_mapNameText != null)
            _mapNameText.text = selectedLevel.mapName;

        if (_mapDescriptionText != null)
            _mapDescriptionText.text = selectedLevel.description;

        if (_playMapButton != null)
            _playMapButton.interactable = selectedLevel.isUnlocked;
    }

    public void PlaySelectedLevel()
    {
        if (_selectedMapIndex >= 0 && _selectedMapIndex < _maps.Length)
        {
            MapData selectedLevel = _maps[_selectedMapIndex];
            if (selectedLevel.isUnlocked)
            {
                SceneManager.LoadScene(selectedLevel.sceneName);
            }
        }
    }
}