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

        if (Input.GetKey(KeyCode.Q))
        {
            // Vector3 newPosition = transform.position;
            // newPosition.x = -transform.position.z;
            // newPosition.z = transform.position.x;
            // transform.position = newPosition;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.down * (_cameraMoveSpeed * 10f * Time.deltaTime));

            // transform.forward += Vector3.left * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            // Vector3 newPosition = transform.position;
            // newPosition.x = -transform.position.z;
            // newPosition.z = transform.position.x;
            // transform.position = newPosition;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * (_cameraMoveSpeed * 10f * Time.deltaTime));
            
            // transform.forward += Vector3.right  * Time.deltaTime;
        }

        Vector3 positionToMove = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        positionToMove.y = 0;
        transform.position += positionToMove * (_cameraMoveSpeed * 10f * Time.deltaTime);
        
        // transform.position += new Vector3(transform.right.x * Input.GetAxis("Horizontal"),0 , transform.forward.z * Input.GetAxis("Vertical")) * _cameraMoveSpeed * 10f * Time.deltaTime;// can't move side when on side camera

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
