using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraChange : MonoBehaviour
{
    [SerializeField] private LevelGeneratorUIPanel _levelGeneratorUIPanel;

    [SerializeField] private float _cameraSensitivity;
    private float _cameraMoveSpeed;
    private float _cameraRotateSpeed;

    private Vector3 _defaultCameraPosition;
    private Quaternion _defaultCameraRotation;
    private bool _cameraOnPlayer;
    Vector3 rotation;
    
    private void Awake()
    {
        _defaultCameraPosition = transform.position;
        _defaultCameraRotation = transform.rotation;
    }

    private void OnEnable()
    {
        _levelGeneratorUIPanel.OnCameraSpeedChanged += ChangeCameraSpeed;
        _levelGeneratorUIPanel.OnCameraRotateChanged += ChangeCameraRotationSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    
        if (Input.GetMouseButton(1))
        {
            CameraLookAround(); //if right click held down
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCamera();
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.down * ( _cameraRotateSpeed * 20f * Time.deltaTime));
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * ( _cameraRotateSpeed * 20f * Time.deltaTime));
        }
        
        Vector3 positionToMove = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        positionToMove.y = 0;
        transform.position += positionToMove * (_cameraMoveSpeed * 20f * Time.deltaTime);
        transform.position += transform.forward * (Input.mouseScrollDelta.y * _cameraMoveSpeed * 1000f * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * (_cameraMoveSpeed * 20f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += Vector3.down * (_cameraMoveSpeed * 20f * Time.deltaTime);
        }
    }

    private void CameraLookAround()
    {
        rotation.x += Input.GetAxis("Mouse X") * _cameraSensitivity;
        rotation.y += Input.GetAxis("Mouse Y") * _cameraSensitivity;
        Quaternion xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
        
        transform.localRotation = xQuat * yQuat; 
    }

    private void ChangeCameraSpeed(float speed)
    {
        _cameraMoveSpeed = speed;
    }
    
    private void ChangeCameraRotationSpeed(float speed)
    {
        _cameraRotateSpeed = speed;
    }

    private void ResetCamera()
    {
        transform.position = _defaultCameraPosition;
        transform.rotation = _defaultCameraRotation;
    }
}
