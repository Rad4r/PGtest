using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] 
    private Transform playerCharacter;
    
    [SerializeField] 
    private Transform overviewHolder;
    
    private Vector3 overviewPosition;
    private Quaternion defaultRotation;
    private bool cameraOnPlayer;

    private void Awake()
    {
        overviewPosition = transform.position;
        defaultRotation = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            cameraOnPlayer = !cameraOnPlayer;
            
            if (!cameraOnPlayer)
            {
                transform.parent = overviewHolder;
                transform.position = overviewPosition;
                transform.rotation = defaultRotation;
            }
            else
            {
                transform.parent = playerCharacter;
                transform.localPosition = new Vector3(0,0.5f,0);
            }
        }

        if (cameraOnPlayer)
            FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.LookAt(Input.mousePosition, Vector3.up);
    }
}
