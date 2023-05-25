using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelGeneratorUIPanel : MonoBehaviour
{
    [Header("General")]
    
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Button _closePanelButton;
    [SerializeField] private Transform _buttonArrowTransform;
    
    [Header("Camera Settings")]
    [SerializeField] private Slider _cameraSpeedSlider;
    [SerializeField] private TMP_Text _cameraSpeedText;
    [SerializeField] private float _maxCameraSpeed;
    
    [Header("Level Generation Settings")]
    
    [SerializeField] private Slider _roomNumberSlider;
    [SerializeField] private Slider _roomSpacingSlider;
    [SerializeField] private TMP_Text _roomNumberText;
    [SerializeField] private TMP_Text _roomSpacingText;
    [SerializeField] private int _maxRoomNumbers;
    [SerializeField] private int _maxRoomSpacing;

    [Header("Level Generation Settings")]
    
    [SerializeField] private GameObject _wallSelectPopup;
    [SerializeField] private GameObject[] _wallsToSpawn;
    
    [SerializeField] private Button _wallSelectCloseButton;
    
    [SerializeField] private Button _wallOneInputButton;
    [SerializeField] private Button _wallTwoInputButton;
    [SerializeField] private Button _wallThreeInputButton;
    [SerializeField] private Button _wallFourInputButton;
    [SerializeField] private Button _wallFiveInputButton;

    public Action<int, float> OnGenerationValueChanged;
    public Action<float> OnCameraSpeedChanged;

    private bool panelIsOpen;
    private WallInputButton _currentWallInputButton;

    [SerializeField] private GameObject _objectToUpdate;
    [SerializeField] private Image _imageToUpdate;

    private void Awake()
    {
        panelIsOpen = true;
        _roomNumberSlider.maxValue = _maxRoomNumbers;
        _roomSpacingSlider.maxValue = _maxRoomSpacing;
        _cameraSpeedSlider.maxValue = _maxCameraSpeed;
        UpdateGenerationValues(0f);
        UpdateCameraSpeed(1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CloseOpenPanel();
        }
    }

    private void OnEnable()
    {
        _roomNumberSlider.onValueChanged.AddListener(UpdateGenerationValues);
        _roomSpacingSlider.onValueChanged.AddListener(UpdateGenerationValues);
        _cameraSpeedSlider.onValueChanged.AddListener(UpdateCameraSpeed);
        _closePanelButton.onClick.AddListener(CloseOpenPanel);
        
        // Wall Buttons
        _wallSelectCloseButton.onClick.AddListener(CloseWallPopUp);
        
        _wallOneInputButton.onClick.AddListener(WallOneClicked);
        _wallTwoInputButton.onClick.AddListener(WallTwoClicked);
        _wallThreeInputButton.onClick.AddListener(WallThreeClicked);
        _wallFourInputButton.onClick.AddListener(WallFourClicked);
        _wallFiveInputButton.onClick.AddListener(WallFiveClicked);
    }

    private void UpdateGenerationValues(float value)
    {
        float spacing = Mathf.Round(_roomSpacingSlider.value * 100f) / 100f;

        _roomNumberText.text = _roomNumberSlider.value.ToString();
        _roomSpacingText.text = spacing.ToString();
        
        OnGenerationValueChanged?.Invoke(Mathf.RoundToInt(_roomNumberSlider.value), spacing);
    }

    private void CloseOpenPanel()// TO-DO: lean tween for smooth transition
    {
        _rectTransform.pivot = panelIsOpen ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
        _buttonArrowTransform.localScale = panelIsOpen ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        _wallSelectPopup.SetActive(false);
        panelIsOpen = !panelIsOpen;
    }
    
    private void UpdateCameraSpeed(float value)
    {
        _cameraSpeedText.text = "" + Mathf.Round(value * 100f) / 100f;
        OnCameraSpeedChanged?.Invoke(value);
    }
    
    private void WallOneClicked()
    {
        WallSelectPopUp(_wallsToSpawn[0], _wallOneInputButton.GetComponent<WallInputButton>());
    }
    private void WallTwoClicked()
    {
        WallSelectPopUp(_wallsToSpawn[1], _wallTwoInputButton.GetComponent<WallInputButton>());
    }
    private void WallThreeClicked()
    {
        WallSelectPopUp(_wallsToSpawn[2], _wallThreeInputButton.GetComponent<WallInputButton>());
    }
    private void WallFourClicked()
    {
        WallSelectPopUp(_wallsToSpawn[3], _wallFourInputButton.GetComponent<WallInputButton>());
    }
    private void WallFiveClicked()
    {
        WallSelectPopUp(_wallsToSpawn[4], _wallFiveInputButton.GetComponent<WallInputButton>());
    }

    private void WallSelectPopUp(GameObject wallToAffect, WallInputButton wallInputButton)
    {
        _objectToUpdate = wallToAffect;
        _currentWallInputButton = wallInputButton;
        _wallSelectPopup.SetActive(true);
    }
    
    private void CloseWallPopUp()
    {
        _wallSelectPopup.SetActive(false);
    }
    
    public void UpdateWallVisuals(Material wallMaterial, Sprite sprite, Color color)
    {
        _currentWallInputButton.UpdateButtonImage(sprite, color);
        
        _objectToUpdate.GetComponent<MeshRenderer>().material = wallMaterial;
        _wallSelectPopup.SetActive(false);
    }
}
