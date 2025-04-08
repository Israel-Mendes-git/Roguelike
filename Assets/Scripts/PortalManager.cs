using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private RoomFirstDungeonGenerator roomGenerator;
    private GameObject player;

    private void Start()
    {
        roomGenerator = FindObjectOfType<RoomFirstDungeonGenerator>();
        player = GameObject.FindWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entrou no portal");
        if (collision.gameObject.CompareTag("Player"))
        {
            RegenerateDungeon();
        }
    }

    private void RegenerateDungeon()
    {
        // Limpa a dungeon
        roomGenerator.ClearDungeon();

        // Recria a dungeon
        roomGenerator.CreateRooms();

        // Reposiciona o jogador na nova primeira sala gerada
        if (player != null)
        {
            player.transform.position = new Vector3(roomGenerator.playerSpawnPosition.x + 0.5f,
                                                    roomGenerator.playerSpawnPosition.y + 0.5f, 0);
        }

        // Move o portal para a última sala gerada
        Vector2 portalPosition = roomGenerator.GetLastRoomPosition();
        transform.position = new Vector3(portalPosition.x + 0.5f, portalPosition.y + 0.5f, 0);
    }

}
