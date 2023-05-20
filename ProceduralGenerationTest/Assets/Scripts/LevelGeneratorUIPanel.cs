using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelGeneratorUIPanel : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    
    [SerializeField] private Slider _roomNumberSlider;
    [SerializeField] private Slider _roomSpacingSlider;
    
    [SerializeField] private TMP_Text _roomNumberText;
    [SerializeField] private TMP_Text _roomSpacingText;
    
    [SerializeField] private int _maxRoomNumbers;
    [SerializeField] private int _maxRoomSpacing;

    [SerializeField] private Button _closePanelButton;
    [SerializeField] private Transform _buttonArrowTransform;

    [Header("Wall Buttons")] 
    [SerializeField] private GameObject _wallSelectPopup;
    
    [SerializeField] private Button _wallOneInputButton;
    [SerializeField] private Button _wallTwoInputButton;
    [SerializeField] private Button _wallThreeInputButton;
    [SerializeField] private Button _wallFourInputButton;
    [SerializeField] private Button _wallFiveInputButton;
    [SerializeField] private Button _wallSixInputButton;

    public Action<int, float> OnGenerationValueChanged;

    private bool panelIsOpen;

    private void Awake()
    {
        panelIsOpen = true;
        UpdateGenerationValues(0f);
    }

    private void OnEnable()
    {
        _roomNumberSlider.onValueChanged.AddListener(UpdateGenerationValues);
        _roomSpacingSlider.onValueChanged.AddListener(UpdateGenerationValues);
        _closePanelButton.onClick.AddListener(CloseOpenPanel);
        // throw new NotImplementedException();
    }

    private void OnDisable()
    {
        //Leave it for now
    }

    private void UpdateGenerationValues(float value)
    {
        int roomValue = Mathf.RoundToInt( _roomNumberSlider.value * _maxRoomNumbers);
        
        int rooms =  roomValue == 0 ? 1: roomValue;
        float spacing = Mathf.Round(_roomSpacingSlider.value * _maxRoomSpacing * 100f) / 100f;

        _roomNumberText.text = rooms.ToString();
        _roomSpacingText.text = spacing.ToString();
        
        OnGenerationValueChanged?.Invoke(rooms, spacing);
    }

    private void CloseOpenPanel()
    {
        _rectTransform.pivot = panelIsOpen ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
        _buttonArrowTransform.localScale = panelIsOpen ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1); 
        // _rectTransform.localPosition = panelIsOpen ? new Vector3(600,0,0) : Vector3.zero; //Need lean tween for smooth
        panelIsOpen = !panelIsOpen;
    }
}
