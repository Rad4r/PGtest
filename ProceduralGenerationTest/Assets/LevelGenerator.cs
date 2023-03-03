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
    //private int roomsLeft;


    private List<Vector3> availablePositions;
    private Vector3 groundScale;

    private void Awake()
    {
        _levelArranger = roomHolder.GetComponent<LevelArranger>();
        availablePositions = new List<Vector3>();
        groundScale = roomHolder.transform.GetChild(0).transform.localScale;
    }

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        ResetLevel();
        //roomsLeft = roomNumbers;

        Vector3 spawnPosition = Vector3.zero;

        for (int i = 0; i < roomNumbers; i++)
        {
            Instantiate(roomHolder, spawnPosition, Quaternion.identity, levelHolder);
            spawnedLocations.Add(spawnPosition);
            //roomsLeft--;
            availablePositions.Add(spawnPosition += Vector3.left * (groundScale.x + roomSpacing));
            availablePositions.Add(spawnPosition += Vector3.right * (groundScale.x + roomSpacing));
            availablePositions.Add(spawnPosition += Vector3.forward * (groundScale.z + roomSpacing));
            availablePositions.Add(spawnPosition += Vector3.back * (groundScale.z + roomSpacing));
            spawnPosition = GetNewSpawnPosition();
        }
    }

    //have an availableSpawnPositions list and update it with for each by removing
    private Vector3 GetNewSpawnPosition()
    {
        int randomSpawnIndex = Random.Range(0, availablePositions.Count);
        Vector3 newSpawnPoint = availablePositions[randomSpawnIndex];
        
        for (int i = 0; i < spawnedLocations.Count; i++)
        {
            if (spawnedLocations[i] ==  newSpawnPoint)
            {
                availablePositions.RemoveAt(randomSpawnIndex);
                GetNewSpawnPosition();
            }
            else
            {
                break;
            }
        }
        
        return newSpawnPoint;
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
