using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;
    [SerializeField]
    private GameObject[] itensParaSpawn;
    [SerializeField]
    private int quantidadeItens = 5;


    public CorridorFirstDungeonGenerator corridorFirst;
    private List<GameObject> itensInstanciados = new List<GameObject>();
    [SerializeField] private GameObject lojinha;
    public Vector2Int playerSpawnPosition;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void Awake()
    {
        tilemapVisualizer.Clear();
        CreateRooms();
    }
    void Start()
    {
        corridorFirst = GetComponent<CorridorFirstDungeonGenerator>();
        // Posiciona o jogador após gerar as salas
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(playerSpawnPosition.x + 0.5f, playerSpawnPosition.y + 0.5f, 0);
        }
    }


    private void CreateRooms()
    {
        var roomsList = ProceduralGeneration.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition,
            new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        List<Vector2Int> roomCenters = new List<Vector2Int>();

        if (roomsList.Count > 0)
        {
            // Define a primeira sala como a sala inicial do jogador
            BoundsInt firstRoom = roomsList[0];
            playerSpawnPosition = (Vector2Int)Vector3Int.RoundToInt(firstRoom.center);
            roomCenters.Add(playerSpawnPosition);
        }

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }

        foreach (var room in roomsList)
        {
            Vector2Int center = (Vector2Int)Vector3Int.RoundToInt(room.center);
            if (!roomCenters.Contains(center))
                roomCenters.Add(center);
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        SpawnItens(floor, corridors);
    }

    private void SpawnItens(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> corridors)
    {
        foreach (var item in itensInstanciados)
        {
            Destroy(item);
        }
        itensInstanciados.Clear();

        List<Vector2Int> validPositions = new List<Vector2Int>(floorPositions);
        validPositions.RemoveAll(pos => corridors.Contains(pos) || pos == playerSpawnPosition);

        for (int i = 0; i < quantidadeItens; i++)
        {
            if (validPositions.Count == 0) break;
            Vector2Int spawnPosition = validPositions[Random.Range(0, validPositions.Count)];
            validPositions.Remove(spawnPosition);
            Vector3 spawnWorldPosition = new Vector3(spawnPosition.x + 0.5f, spawnPosition.y + 0.5f, 0);
            GameObject newItem = Instantiate(itensParaSpawn[Random.Range(0, itensParaSpawn.Length)], spawnWorldPosition, Quaternion.identity);
            itensInstanciados.Add(newItem);
        }

    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int start, Vector2Int end)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        Vector2Int currentPosition = start;
        while (currentPosition != end)
        {
            if (currentPosition.x != end.x)
            {
                currentPosition.x += (end.x > currentPosition.x) ? 1 : -1;
            }
            else if (currentPosition.y != end.y)
            {
                currentPosition.y += (end.y > currentPosition.y) ? 1 : -1;
            }
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    corridor.Add(currentPosition + new Vector2Int(x, y));
                }
            }
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }
}
 