using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraChange : MonoBehaviour
{
    [SerializeField] private LevelGeneratorUIPanel _levelGeneratorUIPanel;
    [SerializeField] private float _cameraMoveSpeed;

    
    private Vector3 _overviewPosition;
    private Quaternion _defaultRotation;
    private bool _cameraOnPlayer;

    private void Awake()
    {
        _overviewPosition = transform.position;
        _defaultRotation = transform.rotation;
    }

    private void OnEnable()
    {
        _levelGeneratorUIPanel.OnCameraSpeedChanged += ChangeCameraSpeed;
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     Application.Quit();
        // }
        
        transform.position += new Vector3(Input.GetAxis("Horizontal"),0 , Input.GetAxis("Vertical")) * _cameraMoveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * _cameraMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += Vector3.down * _cameraMoveSpeed * Time.deltaTime;
        }
    }

    private void ChangeCameraSpeed(float speed)
    {
        _cameraMoveSpeed = speed;
    }

    private void FollowPlayer()
    {
        transform.LookAt(Input.mousePosition, Vector3.up);
    }
}
