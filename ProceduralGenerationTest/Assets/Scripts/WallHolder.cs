using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WallHolder : MonoBehaviour
{
    [SerializeField] private LevelGeneratorUIPanel _levelGeneratorUIPanel;
    [SerializeField] private Button _wallSelectButton;
    [SerializeField] private Material _wallMaterial;
    [SerializeField] private Image _wallIconImageComponent;
    private void Awake()
    {
        _wallSelectButton.onClick.AddListener(UpdateWallVisuals);
    }
    
    
    private void UpdateWallVisuals()
    {
        _levelGeneratorUIPanel.UpdateWallVisuals(_wallMaterial, _wallIconImageComponent.sprite, _wallIconImageComponent.color);
    }
}

//     wallTextureIcon.texture = AssetPreview.GetAssetPreview(wallObject);
//     File.WriteAllBytes("Assets/" + name + ".png",  AssetPreview.GetAssetPreview(wallObject).EncodeToPNG());
//     AssetDatabase.CreateAsset(ImageConversion.EncodeToPNG(wallTextureIcon.texture), "Assets/" + name + ".png" );
