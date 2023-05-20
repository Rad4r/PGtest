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
    
    [SerializeField] private Button _wallOneInputButton;
    [SerializeField] private Button _wallTwoInputButton;
    [SerializeField] private Button _wallThreeInputButton;
    [SerializeField] private Button _wallFourInputButton;
    [SerializeField] private Button _wallFiveInputButton;
    [SerializeField] private Button _wallSixInputButton;

    public Action<int, float> OnGenerationValueChanged;
    public Action<float> OnCameraSpeedChanged;

    private bool panelIsOpen;

    private void Awake()
    {
        panelIsOpen = true;
        _roomNumberSlider.maxValue = _maxRoomNumbers;
        _roomSpacingSlider.maxValue = _maxRoomSpacing;
        _cameraSpeedSlider.maxValue = _maxCameraSpeed;
        UpdateGenerationValues(0f);
        UpdateCameraSpeed(10f);
    }

    private void OnEnable()
    {
        _roomNumberSlider.onValueChanged.AddListener(UpdateGenerationValues);
        _roomSpacingSlider.onValueChanged.AddListener(UpdateGenerationValues);
        _cameraSpeedSlider.onValueChanged.AddListener(UpdateCameraSpeed);
        _closePanelButton.onClick.AddListener(CloseOpenPanel);
        // throw new NotImplementedException();
    }

    private void OnDisable()
    {
        //Leave it for now
    }

    private void UpdateGenerationValues(float value)
    {
        //int rooms =  Mathf.CeilToInt( _roomNumberSlider.value * _maxRoomNumbers);
        float spacing = Mathf.Round(_roomSpacingSlider.value * 100f) / 100f;

        _roomNumberText.text = _roomNumberSlider.value.ToString();
        _roomSpacingText.text = spacing.ToString();
        
        OnGenerationValueChanged?.Invoke(Mathf.RoundToInt(_roomNumberSlider.value), spacing);
    }

    private void CloseOpenPanel()
    {
        _rectTransform.pivot = panelIsOpen ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
        _buttonArrowTransform.localScale = panelIsOpen ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1); 
        // _rectTransform.localPosition = panelIsOpen ? new Vector3(600,0,0) : Vector3.zero; //Need lean tween for smooth
        panelIsOpen = !panelIsOpen;
    }
    
    private void UpdateCameraSpeed(float value)
    {
        _cameraSpeedText.text = "" + Mathf.Round(value * 100f) / 100f;
        OnCameraSpeedChanged?.Invoke(value);
    }
}
