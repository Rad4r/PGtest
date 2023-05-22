using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraChange : MonoBehaviour
{
    [SerializeField] private LevelGeneratorUIPanel _levelGeneratorUIPanel;
    private float _cameraMoveSpeed;

    
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
        if (Input.GetKey(KeyCode.Q))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.down * ( 40f * Time.deltaTime));
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * ( 40f * Time.deltaTime));
        }

        Vector3 positionToMove = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        positionToMove.y = 0;
        transform.position += positionToMove * (_cameraMoveSpeed * 10f * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * (_cameraMoveSpeed * 10f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += Vector3.down * (_cameraMoveSpeed * 10f * Time.deltaTime);
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
