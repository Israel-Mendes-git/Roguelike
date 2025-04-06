using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsIndirections(floorPositions, Direction2D.cardinalDirectionsList);

        foreach (var position in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleBasicWall(position);
        }

        // Adiciona um TilemapCollider2D se não houver um
        AddTilemapCollider(tilemapVisualizer);
    }

    private static HashSet<Vector2Int> FindWallsIndirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (!floorPositions.Contains(neighbourPosition))
                    wallPositions.Add(neighbourPosition);
            }
        }
        return wallPositions;
    }

    private static void AddTilemapCollider(TilemapVisualizer tilemapVisualizer)
    {
        // Obtém o GameObject do tilemap
        GameObject tilemapGameObject = tilemapVisualizer.wallTilemap.gameObject;

        // Verifica se já existe um TilemapCollider2D
        if (!tilemapGameObject.TryGetComponent<TilemapCollider2D>(out _))
        {
            tilemapGameObject.AddComponent<TilemapCollider2D>();
        }
    }
}
