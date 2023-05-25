using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
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
    private int _currentRoomNumber;
    
    private List<RoomStructure> _roomsSpawned;
    private RoomStructure _newRoomStructureToSpawn;
    
    private Vector3 _groundScale;

    private void Awake() //use collision box size for scale maybe for random assets
    {
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateLevel();
        }
    }

    private void ChangeGenerationSettings(int rooms, float spacingBetweenRooms)
    {
        _roomNumbers = rooms;
        _roomSpacing = spacingBetweenRooms;
    }


    public void GenerateLevel()
    {
        ResetLevel();
        SpawnRoom();

        while (_currentRoomNumber < _roomNumbers)
        {
            SpawnRoom();
        }
    }

    private void SpawnRoom()
    {
        GameObject newRoom = Instantiate(_roomHolder, _newRoomStructureToSpawn.GetRoomPosition, Quaternion.identity, _levelHolder);
        _newRoomStructureToSpawn.SetRoomObjectTransform(newRoom.transform);
        _roomsSpawned.Add(_newRoomStructureToSpawn);

        IndicateRoomName();
        
        _newRoomStructureToSpawn = new RoomStructure();
        SpawnDoors();
        _currentRoomNumber++;
    }

    private void IndicateRoomName()
    {
        if (_currentRoomNumber == 0 )
        {
            LevelArranger _roomSettings = _newRoomStructureToSpawn.GetRoomTransform.GetComponent<LevelArranger>();
            _roomSettings.UpdateRoomText("Start");
        }
        
        if (_currentRoomNumber == _roomNumbers -1)
        {
            LevelArranger _roomSettings = _newRoomStructureToSpawn.GetRoomTransform.GetComponent<LevelArranger>();
            _roomSettings.UpdateRoomText("End");
        }
    }

    private void SpawnDoors() //need to check if the direction has a room already (Mainly for multiple doors in a room)
    {
        Vector3 connectingDoorLocation = _roomsSpawned[_currentRoomNumber].GetConnectingDoorPosition;
        Transform roomParent = _roomsSpawned[_currentRoomNumber].GetRoomTransform;
        
        if (connectingDoorLocation != Vector3.zero)
        {
            SpawnConnectingDoors(roomParent, connectingDoorLocation);
        }

        if (_currentRoomNumber == _roomNumbers-1)
        {
            return;
        }
        
        SpawnNewDoors();
    }

    private void SpawnNewDoors()
    {
        // TO-DO Need to check if the point already has a room, if so, make it unavailable and pick a new side 
        Transform roomParent = _roomsSpawned[_currentRoomNumber].GetRoomTransform;
        
        int randomNumber = Random.Range(0,_roomsSpawned[_currentRoomNumber].GetAvailableDoorPoints.Count); //Can Loop
        Vector3 randomSideToSpawn = _roomsSpawned[_currentRoomNumber].GetAvailableDoorPoints[randomNumber];
        
        Vector3 doorSpawnLocation = randomSideToSpawn;
        Vector3 newRoomSpawnLocation = randomSideToSpawn;
        bool rotated = false;

        if (randomSideToSpawn.x != 0) //Spawned in the x axis
        {
            doorSpawnLocation.x = randomSideToSpawn.x * _groundScale.x * 0.5f;
            newRoomSpawnLocation.x = randomSideToSpawn.x * _groundScale.x + _roomSpacing * randomSideToSpawn.x;
            rotated = true;
            
        }
        else if (randomSideToSpawn.z != 0)
        {
            doorSpawnLocation.z = randomSideToSpawn.z * _groundScale.z * 0.5f;
            newRoomSpawnLocation.z = randomSideToSpawn.z * _groundScale.z + _roomSpacing * randomSideToSpawn.z;
        }
        
        newRoomSpawnLocation += _roomsSpawned[_currentRoomNumber].GetRoomPosition;

        if (PositionOccupied(newRoomSpawnLocation))
        {
            _roomsSpawned[_currentRoomNumber].RemovePositionFromAvailability(randomSideToSpawn);
            SpawnNewDoors();
        }
        else
        {
            if (rotated)
            {
                Instantiate(_doorPrefab, doorSpawnLocation, Quaternion.Euler(0,90,0), roomParent).transform.localPosition = doorSpawnLocation;
            }
            else
            {
                Instantiate(_doorPrefab, doorSpawnLocation, Quaternion.identity, roomParent).transform.localPosition = doorSpawnLocation;
            }
            _newRoomStructureToSpawn.SetRoomPosition(newRoomSpawnLocation);
            _newRoomStructureToSpawn.SetConnectionDoorPoint(randomSideToSpawn * -1);
        }
    }

    private bool PositionOccupied(Vector3 positionToCheck)
    {
        return _roomsSpawned.Any(room => room.GetRoomPosition == positionToCheck);
    }

    private void SpawnConnectingDoors(Transform roomParent, Vector3 connectingDoorLocation)
    {
        Vector3 connectingSpawnLocation = new Vector3();
        
        if (connectingDoorLocation.x != 0)
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
    
    public void ResetLevel()
    {
        foreach (Transform obj in _levelHolder)
        {
            Destroy(obj.gameObject);
        }
        
        _roomsSpawned = new List<RoomStructure>();
        _newRoomStructureToSpawn = new RoomStructure();
        _currentRoomNumber = 0;
        _newRoomStructureToSpawn.SetConnectionDoorPoint(Vector3.zero);
        _newRoomStructureToSpawn.SetRoomPosition(Vector3.zero);
    }
}

public class RoomStructure
{
    private Transform _roomObjectTransform;
    private Vector3 _roomLocation;
    private Vector3 _connectingDoorToSpawnPosition;
    private List<Vector3> _availableDoorPoints = new List<Vector3>()
    {
        new Vector3(1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(-1,0,0),
        new Vector3(0,0,-1)
    };

    public Transform GetRoomTransform => _roomObjectTransform;
    public Vector3 GetRoomPosition => _roomLocation;
    public Vector3 GetConnectingDoorPosition => _connectingDoorToSpawnPosition;

    public List<Vector3> GetAvailableDoorPoints => _availableDoorPoints;
    public void SetRoomObjectTransform(Transform roomTransform)
    {
        _roomObjectTransform = roomTransform;
    }
    
    public void SetRoomPosition(Vector3 roomLocation)
    {
        _roomLocation = roomLocation;
    }

    public void SetConnectionDoorPoint(Vector3 doorConnectPosition)
    {
        _connectingDoorToSpawnPosition = doorConnectPosition;
        _availableDoorPoints.Remove(doorConnectPosition);
    }

    public void RemovePositionFromAvailability(Vector3 position)
    {
        _availableDoorPoints.Remove(position);
    }
}