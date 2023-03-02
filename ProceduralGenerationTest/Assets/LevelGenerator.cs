using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private int roomNumbers;
    
    [SerializeField]
    private int roomSpacing;
    
    [SerializeField]
    private GameObject roomHolder;
    
    [SerializeField]
    private Transform levelHolder;

    private List<Vector3> spawnedLocations;
    private int roomsLeft;

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

    private Vector3 GetNewSpawnPosition(float xRoomScale, float zRoomScale, Vector3 currentSpawnPosition)
    {
        List<Vector3> positionsToSpawn = new List<Vector3>();
        positionsToSpawn.Add(currentSpawnPosition += Vector3.left * (xRoomScale + roomSpacing));
        positionsToSpawn.Add(currentSpawnPosition += Vector3.right * (xRoomScale + roomSpacing));
        positionsToSpawn.Add(currentSpawnPosition += Vector3.forward * (zRoomScale + roomSpacing));
        positionsToSpawn.Add(currentSpawnPosition += Vector3.back * (zRoomScale + roomSpacing));

        Vector3 newPositionToSpawn = positionsToSpawn[Random.Range(0, positionsToSpawn.Count)];
        
        foreach (Vector3 position in spawnedLocations)
        {
            if (newPositionToSpawn == position)
            {
                //there is already a room there so remove it and try the other ones
                //if none of them can fit then move onto another room as the current room
            }
            
        }
        
        return new Vector3();
    }
    
    public void ResetLevel()
    {
        spawnedLocations = new List<Vector3>();
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
