using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelArranger : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] wallObjects;
    
    [SerializeField] 
    private Collider groundArea;

    private void Start()
    {
        GenerateLevel();
    }

    
    //need corner pieces
    public void GenerateLevel() //find the bounds and spawn the walls accordingly (then also rotate)
    {
        ResetWalls();
        
        //top layer
        Vector3 topStartPoint = groundArea.bounds.max;
        topStartPoint.x = groundArea.bounds.min.x;
        GenerateWall(topStartPoint, GetCorrectWalls(groundArea.transform.localScale.x), Vector3.right);
        
        //right layer
        Vector3 rightStartPoint = groundArea.bounds.max;
        rightStartPoint.z = groundArea.bounds.min.z;
        GenerateWall(rightStartPoint, GetCorrectWalls(groundArea.transform.localScale.z), Vector3.forward);

        //left layer
        Vector3 leftStartPoint = groundArea.bounds.min;
        leftStartPoint.y = groundArea.bounds.max.y;
        GenerateWall(leftStartPoint, GetCorrectWalls(groundArea.transform.localScale.z), Vector3.forward);

        //bottom layer (can avoid)
        Vector3 bottomStartPoint = groundArea.bounds.min;
        bottomStartPoint.y = groundArea.bounds.max.y;
        GenerateWall(bottomStartPoint, GetCorrectWalls(groundArea.transform.localScale.x), Vector3.right);
    }

    private List<GameObject> GetCorrectWalls(float distance)
    {
        List<GameObject> listOfWalls = new List<GameObject>();
        //get the right walls and add them to the list
        float currentWallDistance = 0;

        while (currentWallDistance < distance)
        {
            GameObject randomWall = wallObjects[Random.Range(0, wallObjects.Length)];
            currentWallDistance += randomWall.transform.localScale.x;

            // if (currentWallDistance > distance) //effects the prefab scale as well
            // {
            //     Vector3 tempScale = randomWall.transform.localScale;
            //     tempScale.x = distance - (currentWallDistance - randomWall.transform.localScale.x);
            //     randomWall.transform.localScale = tempScale;
            // }
            listOfWalls.Add(randomWall);
            //Debug.Log("Current distance is: " + currentWallDistance);
        }
        
        return listOfWalls;
    }

    private void GenerateWall(Vector3 startPoint, List<GameObject> wallsToSpawn, Vector3 spawnDirection)
    {
        float distanceFromStart = 0;
        Quaternion rotation = spawnDirection == Vector3.forward ?  Quaternion.Euler(0, 90, 0) : Quaternion.identity;
        
        for (int i = 0; i < wallsToSpawn.Count; i++) //auto calculated the distance and distribute the walls
        {
            GameObject currentWall = wallsToSpawn[i];
            Vector3 currentObjectScale = currentWall.transform.localScale;
            Vector3 spawnPosition = startPoint + spawnDirection * (currentObjectScale.x/2f + distanceFromStart) + Vector3.up * currentObjectScale.y/2f;
            Instantiate(currentWall, spawnPosition, rotation, transform);
            distanceFromStart += currentObjectScale.x;
        }
    }
    
    private void ResetWalls()
    {
        foreach (Transform obj in transform)
        {
            Destroy(obj.gameObject);
        }
    }
}
