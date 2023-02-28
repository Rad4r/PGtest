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

    // [SerializeField] 
    // private GameObject[] groundObjects;
    // [SerializeField]
    // private List<GameObject> wallsToSpawn;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel() //find the bounds and spawn the walls accordingly (then also rotate)
    {
        //top layer

        Vector3 startPoint = groundArea.bounds.max;
        startPoint.x = groundArea.bounds.min.x;
        float distanceFromStart = 0;

        List<GameObject> topWalls = GetCorrectWalls(groundArea.transform.localScale.x);

        for (int i = 0; i < topWalls.Count; i++) //auto calculated the distance and distribute the walls
        {
            GameObject currentWall = topWalls[i];
            Vector3 currentObjectScale = currentWall.transform.localScale;
            Vector3 spawnPosition = startPoint + Vector3.right * (currentObjectScale.x/2f + distanceFromStart) + Vector3.up * currentObjectScale.y/2f;
            Instantiate(currentWall, spawnPosition, Quaternion.identity, transform);
            distanceFromStart += currentObjectScale.x;
            //topWalls.RemoveAt(i);
        }

        //left layer

        //right layer

        //bottom layer (can avoid)
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

            if (currentWallDistance > distance)
            {
                Vector3 tempScale = randomWall.transform.localScale;
                tempScale.x = distance - (currentWallDistance - randomWall.transform.localScale.x);
                randomWall.transform.localScale = tempScale;
            }
            listOfWalls.Add(randomWall);
            //Debug.Log("Current distance is: " + currentWallDistance);
        }
        
        return listOfWalls;
    }
}
