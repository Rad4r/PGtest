using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NewBackupScript : MonoBehaviour
{
    [SerializeField] private LevelGeneratorUIPanel _levelGeneratorUIPanel;

    private int _roomNumbers;
    private float _roomSpacing;
    
    [SerializeField] private GameObject _roomHolder;
    [SerializeField] private GameObject _doorPrefab;
    [SerializeField] private Transform _levelHolder;
    // [SerializeField] private bool _bottomWallVisibility;
    // [Range(1,6)][SerializeField] private int _wallHeight;
    //private LevelArranger _levelArranger;
   
    private List<Vector3> _spawnedLocations;
    private List<Vector3> _possibleDoorSpawnLocations;
    private List<Vector3> _availableDoorSpawnLocations;
    private int _currentRoomNumber;
    
    private List<RoomStructureComplex> _roomsSpawned;
    private RoomStructureComplex _newRoomStructureToSpawn;
    
    private Vector3 _groundScale;

    private void Awake() //use collision box size for scale maybe for random assets
    {
        SetupDoorSpawnPoints();
        _groundScale = _roomHolder.transform.GetChild(0).transform.localScale;
    }

    private void OnEnable()
    {
        _levelGeneratorUIPanel.OnGenerationValueChanged += ChangeGenerationSettings;
    }

    private void Start()
    {
        GenerateLevel();
    }

    private void ChangeGenerationSettings(int rooms, float spacingBetweenRooms)
    {
        _roomNumbers = rooms;
        _roomSpacing = spacingBetweenRooms;
    }

    private void SetupDoorSpawnPoints()
    {
        _possibleDoorSpawnLocations = new List<Vector3>
        {
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(-1,0,0),
            new Vector3(0,0,-1)
        };
        _availableDoorSpawnLocations = new List<Vector3>();
    }

    public void GenerateLevel()
    {
        ResetLevel();
        _roomsSpawned = new List<RoomStructureComplex>();
        _currentRoomNumber = 0;
        
        _newRoomStructureToSpawn.ConnectingDoorToSpawnPosition = Vector3.zero;
        _newRoomStructureToSpawn.RoomLocation = Vector3.zero;

        SpawnRoom();

        if (_roomNumbers >= 3)
        {
            SpawnRoom();
            while (_currentRoomNumber < _roomNumbers)
            {
                SpawnAdjacentRoom();
            }
        }
    }

    private void SpawnAdjacentRoom()
    {
        // check if there is a room already in that position and retry
        // spawn door in current room on the side
        // spawn the new room itself
        // spawn the connecting doors
        // add to the list
        
        RoomStructureComplex randomRoomStructure = _roomsSpawned[Random.Range(0, _roomsSpawned.Count)];
        Vector3 sideToSpawn = randomRoomStructure.GetRandomAvailablePosition(); // TO-DO: if this is full try some other room.Also maybe ad it to the spawned rooms list
        Vector3 doorSpawnLocation = sideToSpawn;
        Vector3 roomSpawnLocation = new Vector3();

        if (sideToSpawn == Vector3.zero)
        {
            SpawnAdjacentRoom();
        }

        if (sideToSpawn.x != 0)
        {
            doorSpawnLocation.x = sideToSpawn.x * _groundScale.x * 0.5f;
            roomSpawnLocation.x = sideToSpawn.x * _groundScale.x + _roomSpacing * sideToSpawn.x;

        }
        else if (sideToSpawn.z != 0)
        {
            doorSpawnLocation.z = sideToSpawn.z * _groundScale.z * 0.5f;
            roomSpawnLocation.z = sideToSpawn.z * _groundScale.x + _roomSpacing * sideToSpawn.z;
        }

        roomSpawnLocation += randomRoomStructure.RoomLocation;

        if (PositionUnavailable(roomSpawnLocation))
        {
            SpawnAdjacentRoom(); //retry
        }
        else 
        {
            Vector3 connectingDoorPosition = sideToSpawn * -1;
            
            Instantiate(_doorPrefab, doorSpawnLocation, Quaternion.Euler(0,90,0), randomRoomStructure.RoomObjectTransform).transform.localPosition = doorSpawnLocation;
            GameObject newRoom = Instantiate(_roomHolder, roomSpawnLocation, Quaternion.identity, randomRoomStructure.RoomObjectTransform);
            SpawnConnectingDoors(newRoom.transform, connectingDoorPosition);
                
            _newRoomStructureToSpawn = new RoomStructureComplex
            {
                RoomLocation = roomSpawnLocation,
                RoomObjectTransform = newRoom.transform
            };
            _newRoomStructureToSpawn.SetAvailableDoorPoints(_possibleDoorSpawnLocations, connectingDoorPosition);
            _roomsSpawned.Add(_newRoomStructureToSpawn);
            _currentRoomNumber++;
        }
    }

    private void SpawnRoom()
    {
        GameObject newRoom = Instantiate(_roomHolder, _newRoomStructureToSpawn.RoomLocation, Quaternion.identity, _levelHolder);
        _newRoomStructureToSpawn.RoomObjectTransform = newRoom.transform;
        _roomsSpawned.Add(_newRoomStructureToSpawn);
        
        _newRoomStructureToSpawn = new RoomStructureComplex();
        SpawnDoors();
        _currentRoomNumber++;
    }

    private void SpawnDoors() //need to check if the direction has a room already (Mainly for multiple doors in a room)
    {
        _availableDoorSpawnLocations = _possibleDoorSpawnLocations;
        Vector3 connectingDoorLocation = _roomsSpawned[_currentRoomNumber].ConnectingDoorToSpawnPosition;
        Transform roomParent = _roomsSpawned[_currentRoomNumber].RoomObjectTransform;
        
        if (connectingDoorLocation != Vector3.zero)
        {
            _availableDoorSpawnLocations.Remove(connectingDoorLocation);
            SpawnConnectingDoors(roomParent, connectingDoorLocation);
        }

        if (_currentRoomNumber == _roomNumbers-1)
        {
            return;
        }

        int randomNumber = Random.Range(0,_availableDoorSpawnLocations.Count);
        Vector3 randomSideToSpawn = _availableDoorSpawnLocations[randomNumber];
        Vector3 spawnLocation = randomSideToSpawn;

        _newRoomStructureToSpawn.RoomLocation = randomSideToSpawn;

        if (randomSideToSpawn.x != 0) //Spawned in the x axis
        {
            spawnLocation.x = randomSideToSpawn.x * _groundScale.x * 0.5f;
            Instantiate(_doorPrefab, spawnLocation, Quaternion.Euler(0,90,0), roomParent).transform.localPosition = spawnLocation;
            _newRoomStructureToSpawn.RoomLocation.x = randomSideToSpawn.x * _groundScale.x + _roomSpacing * randomSideToSpawn.x;
        }
        else if (randomSideToSpawn.z != 0)
        {
            spawnLocation.z = randomSideToSpawn.z * _groundScale.z * 0.5f;
            Instantiate(_doorPrefab, spawnLocation, Quaternion.identity, roomParent).transform.localPosition = spawnLocation;
            _newRoomStructureToSpawn.RoomLocation.z = randomSideToSpawn.z * _groundScale.z + _roomSpacing * randomSideToSpawn.z;
        }

        _newRoomStructureToSpawn.RoomLocation += _roomsSpawned[_currentRoomNumber].RoomLocation;
        
        _roomsSpawned[_currentRoomNumber].SetAvailableDoorPoints(_availableDoorSpawnLocations, randomSideToSpawn);
        _newRoomStructureToSpawn.ConnectingDoorToSpawnPosition = randomSideToSpawn * -1;
    }

    private void SpawnConnectingDoors(Transform roomParent, Vector3 connectingDoorLocation)
    {
        Vector3 connectingSpawnLocation = new Vector3();
        
        if (connectingDoorLocation.x != 0) //Spawned in the x axis
        {
            connectingSpawnLocation.x = connectingDoorLocation.x * _groundScale.x * 0.5f;
            Instantiate(_doorPrefab, connectingSpawnLocation, Quaternion.Euler(0,90,0), roomParent).transform.localPosition = connectingSpawnLocation;
        }
        else if (connectingDoorLocation.z != 0)
        {
            connectingSpawnLocation.z = connectingDoorLocation.z * _groundScale.z * 0.5f;
            Instantiate(_doorPrefab, connectingSpawnLocation, Quaternion.identity, roomParent).transform.localPosition = connectingSpawnLocation;
        }
    }

    private bool PositionUnavailable(Vector3 positionToCheck)
    {
        foreach (RoomStructureComplex room in _roomsSpawned)
        {
            if (room.RoomLocation == positionToCheck)
            {
                return true;
            }
        }
        return false;
    }
    
    private void ResetLevel()
    {
        foreach (Transform obj in _levelHolder)
        {
            Destroy(obj.gameObject);
        }
    }
}

public struct RoomStructureComplex // Make room types and set the doors on each side to be true or false
{
    public Transform RoomObjectTransform;
    public Vector3 RoomLocation;
    public Vector3 ConnectingDoorToSpawnPosition;
    private List<Vector3> _availableDoorPositions;

    public void SetAvailableDoorPoints(List<Vector3> availablePos, Vector3 removePos)
    {
        List<Vector3> newPositionList = new List<Vector3>();
        availablePos.ForEach(position => newPositionList.Add(position));
        newPositionList.Remove(removePos);
        _availableDoorPositions = newPositionList;

        _availableDoorPositions.ForEach( pos => Debug.Log("position is: "+ pos));
    }

    public Vector3 GetRandomAvailablePosition() // might return null
    {
        if (_availableDoorPositions == null)
        {
            return Vector3.zero;
        }
        int randomIndex = Random.Range(0, _availableDoorPositions.Count);
        Vector3 availablePosition = _availableDoorPositions[randomIndex];
        _availableDoorPositions.RemoveAt(randomIndex); //could be error
        return availablePosition;
    }
}