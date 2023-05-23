using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallInputButton : MonoBehaviour
{
    [SerializeField] private GameObject _plusImageObject;
    [SerializeField] private Image _wallImage;

    // public void DisablePlusImage()
    // {
    //     _plusImageObject.SetActive(false);
    // }
    //
    // public Image GetWallImage()
    // {
    //     return  _wallImage;
    // }
    
    public void UpdateButtonImage(Sprite sprite, Color color)
    {
        if (_plusImageObject.activeSelf)
            _plusImageObject.SetActive(false);
        _wallImage.sprite = sprite;
        _wallImage.color = color;
    }
}
