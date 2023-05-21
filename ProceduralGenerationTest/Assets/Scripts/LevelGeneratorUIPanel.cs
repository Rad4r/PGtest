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
    
    [SerializeField] private Button _wallOneInputButton;
    [SerializeField] private Button _wallTwoInputButton;
    [SerializeField] private Button _wallThreeInputButton;
    [SerializeField] private Button _wallFourInputButton;
    [SerializeField] private Button _wallFiveInputButton;
    [SerializeField] private Button _wallSixInputButton;

    public Action<int, float> OnGenerationValueChanged;
    public Action<float> OnCameraSpeedChanged;

    private bool panelIsOpen;

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

    private void OnEnable()
    {
        _roomNumberSlider.onValueChanged.AddListener(UpdateGenerationValues);
        _roomSpacingSlider.onValueChanged.AddListener(UpdateGenerationValues);
        _cameraSpeedSlider.onValueChanged.AddListener(UpdateCameraSpeed);
        _closePanelButton.onClick.AddListener(CloseOpenPanel);
        
        // Wall Buttons
        _wallOneInputButton.onClick.AddListener(WallOneClicked);
        _wallTwoInputButton.onClick.AddListener(WallTwoClicked);
        _wallThreeInputButton.onClick.AddListener(WallThreeClicked);
        _wallFourInputButton.onClick.AddListener(WallFourClicked);
        _wallFiveInputButton.onClick.AddListener(WallFiveClicked);
        _wallSixInputButton.onClick.AddListener(WallSixClicked);
    }

    private void OnDisable()
    {
        //Leave it for now
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
        panelIsOpen = !panelIsOpen;
    }
    
    private void UpdateCameraSpeed(float value)
    {
        _cameraSpeedText.text = "" + Mathf.Round(value * 100f) / 100f;
        OnCameraSpeedChanged?.Invoke(value);
    }
    
    private void WallOneClicked()
    {
        _objectToUpdate = _wallsToSpawn[0];
        _imageToUpdate = _wallOneInputButton.GetComponentsInChildren<Image>()[1];
        WallSelectPopUp();
    }
    private void WallTwoClicked()
    {
        _objectToUpdate = _wallsToSpawn[1];
        _imageToUpdate = _wallTwoInputButton.GetComponentsInChildren<Image>()[1];
        WallSelectPopUp();
    }
    private void WallThreeClicked()
    {
        _objectToUpdate = _wallsToSpawn[2];
        _imageToUpdate = _wallThreeInputButton.GetComponentsInChildren<Image>()[1];
        WallSelectPopUp();
    }
    private void WallFourClicked()
    {
        _objectToUpdate = _wallsToSpawn[3];
        _imageToUpdate = _wallFourInputButton.GetComponentsInChildren<Image>()[1];
        WallSelectPopUp();
    }
    private void WallFiveClicked()
    {
        _objectToUpdate = _wallsToSpawn[4];
        _imageToUpdate = _wallFiveInputButton.GetComponentsInChildren<Image>()[1];
        WallSelectPopUp();
    }
    private void WallSixClicked()
    {
        _objectToUpdate = _wallsToSpawn[5];
        _imageToUpdate = _wallSixInputButton.GetComponentsInChildren<Image>()[1];
        WallSelectPopUp();
    }

    private void WallSelectPopUp()
    {
        _wallSelectPopup.SetActive(true);
    }


    public void UpdateWallVisuals(Material wallMaterial, Sprite sprite, Color color)
    {
        _objectToUpdate.GetComponent<MeshRenderer>().material = wallMaterial;
        _imageToUpdate.sprite = sprite;
        _imageToUpdate.color = color;
        _wallSelectPopup.SetActive(false);
    }
}
