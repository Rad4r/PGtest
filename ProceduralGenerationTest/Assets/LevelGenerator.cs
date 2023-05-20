using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    // [SerializeField] private bool _bottomWallVisibility;

    // [Range(1,6)][SerializeField] private int _wallHeight;
    [Range(1,10)][SerializeField] private int _roomNumbers;
    [Range(5,20)][SerializeField] private int _roomSpacing;
    
    [SerializeField] private GameObject _roomHolder;
    [SerializeField] private GameObject _doorPrefab;
    [SerializeField] private Transform _levelHolder;
    
    //private LevelArranger _levelArranger;
    private List<Vector3> _spawnedLocations;

    private List<Vector3> _possibleDoorSpawnLocations;
    private List<Vector3> _availableDoorSpawnLocations;
    private int _currentRoomNumber;
    
    private List<RoomStructure> _roomsSpawned;
    private RoomStructure _newRoomStructureToSpawn;
    
    private Vector3 _groundScale;

    private void Awake() //use collision box size for scale maybe for random assets
    {
        SetupDoorSpawnPoints();
        _groundScale = _roomHolder.transform.GetChild(0).transform.localScale;
    }

    private void Start()
    {
        GenerateLevel();
    }

    private void SetupDoorSpawnPoints()
    {
        _possibleDoorSpawnLocations = new List<Vector3>();
        _possibleDoorSpawnLocations.Add(new Vector3(1,0,0));
        _possibleDoorSpawnLocations.Add(new Vector3(0,0,1));
        _possibleDoorSpawnLocations.Add(new Vector3(-1,0,0));
        _possibleDoorSpawnLocations.Add(new Vector3(0,0,-1));
        _availableDoorSpawnLocations = new List<Vector3>();
    }

    public void GenerateLevel()
    {
        ResetLevel();
        _roomsSpawned = new List<RoomStructure>();
        _currentRoomNumber = 0;
        
        _newRoomStructureToSpawn.ConnectingDoorToSpawnPosition = Vector3.zero;
        _newRoomStructureToSpawn.RoomLocation = Vector3.zero;

        SpawnRoom();

        while (_currentRoomNumber < _roomNumbers)
        {
            SpawnRoom();
        }
    }

    private void SpawnRoom()
    {
        GameObject newRoom = Instantiate(_roomHolder, _newRoomStructureToSpawn.RoomLocation, Quaternion.identity, _levelHolder);
        _newRoomStructureToSpawn.RoomObjectTransform = newRoom.transform;
        _roomsSpawned.Add(_newRoomStructureToSpawn);
        
        _newRoomStructureToSpawn = new RoomStructure();
        SpawnDoors();
        _currentRoomNumber++;

    }

    private void SpawnDoors() //need to check if the direction has a room already
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

        int randomNumber = Random.Range(0,_availableDoorSpawnLocations.Count); //Can Loop
        Vector3 randomSideToSpawn = _availableDoorSpawnLocations[randomNumber];
        Vector3 spawnLocation = randomSideToSpawn;

        if (randomSideToSpawn.x != 0) //Spawned in the x axis
        {
            spawnLocation.x = randomSideToSpawn.x * _groundScale.x * 0.5f;
            Instantiate(_doorPrefab, spawnLocation, Quaternion.Euler(0,90,0), roomParent).transform.localPosition = spawnLocation;
            
            _newRoomStructureToSpawn.RoomLocation = randomSideToSpawn;
            _newRoomStructureToSpawn.RoomLocation.x = randomSideToSpawn.x * _groundScale.x + _roomSpacing * randomSideToSpawn.x;
            _newRoomStructureToSpawn.RoomLocation += _roomsSpawned[_currentRoomNumber].RoomLocation;
        }
        else if (randomSideToSpawn.z != 0)
        {
            spawnLocation.z = randomSideToSpawn.z * _groundScale.z * 0.5f;
            Instantiate(_doorPrefab, spawnLocation, Quaternion.identity, roomParent).transform.localPosition = spawnLocation;
            
            _newRoomStructureToSpawn.RoomLocation = randomSideToSpawn;
            _newRoomStructureToSpawn.RoomLocation.z = randomSideToSpawn.z * _groundScale.z + _roomSpacing * randomSideToSpawn.z;
            _newRoomStructureToSpawn.RoomLocation += _roomsSpawned[_currentRoomNumber].RoomLocation;
        }

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
    
    public void ResetLevel()
    {
        
        foreach (Transform obj in _levelHolder)
        {
            Destroy(obj.gameObject);
        }
    }
}

public struct RoomStructure
{
    public Transform RoomObjectTransform;
    public Vector3 RoomLocation;
    public Vector3 ConnectingDoorToSpawnPosition;
}