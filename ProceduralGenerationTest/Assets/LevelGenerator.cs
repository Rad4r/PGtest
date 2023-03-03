using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] 
    private bool bottomWallVisibility;
    
    [SerializeField] 
    private int wallHeight;
    
    [SerializeField]
    private int roomNumbers;
    
    [SerializeField]
    private int roomSpacing;
    
    [SerializeField]
    private GameObject roomHolder;
    
    [SerializeField]
    private Transform levelHolder;
    
    private LevelArranger _levelArranger;
    private List<Vector3> spawnedLocations;
    private int roomsLeft;

    private void Awake()
    {
        _levelArranger = roomHolder.GetComponent<LevelArranger>();
    }

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        ResetLevel();
        roomsLeft = roomNumbers;

        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < roomNumbers; i++)
        {
            GameObject currentRoom = Instantiate(roomHolder, spawnPosition, Quaternion.identity, levelHolder);
            spawnedLocations.Add(spawnPosition);
            roomsLeft--;

            Vector3 groundScale = currentRoom.transform.GetChild(0).transform.localScale;
            
            spawnPosition = GetNewSpawnPosition(groundScale.x, groundScale.z, spawnPosition);
        }
    }

    //have an availableSpawnPositions list and update it with for each by removing
    private Vector3 GetNewSpawnPosition(float xRoomScale, float zRoomScale, Vector3 currentSpawnPosition)
    {
        List<Vector3> positionsToSpawn = new List<Vector3>();
        positionsToSpawn.Add(currentSpawnPosition += Vector3.left * (xRoomScale + roomSpacing));
        positionsToSpawn.Add(currentSpawnPosition += Vector3.right * (xRoomScale + roomSpacing));
        positionsToSpawn.Add(currentSpawnPosition += Vector3.forward * (zRoomScale + roomSpacing));
        positionsToSpawn.Add(currentSpawnPosition += Vector3.back * (zRoomScale + roomSpacing));
        
        bool positionFound = false;
        Vector3 newPositionToSpawn = Vector3.up * 5f;
    
        while (!positionFound)
        {
            int randomPositionIndex = Random.Range(0, positionsToSpawn.Count);
            newPositionToSpawn = positionsToSpawn[randomPositionIndex];
            
            foreach (Vector3 position in spawnedLocations)
            {
                if (newPositionToSpawn == position)
                {
                    positionsToSpawn.RemoveAt(randomPositionIndex);
                }
                else
                {
                    spawnedLocations.Add(newPositionToSpawn);
                    positionFound = true;
                    break;
                }
            }
        }
        return newPositionToSpawn;
    }
    
    
    public void ResetLevel()
    {
        spawnedLocations = new List<Vector3>();
        _levelArranger.SetWallValues(bottomWallVisibility, wallHeight);
        
        foreach (Transform obj in levelHolder)
        {
            Destroy(obj.gameObject);
        }
    }
}

public struct Room
{
    public Vector3 location; //and rthe 4 sides
    public bool isFilled;
}
