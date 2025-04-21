using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private RoomFirstDungeonGenerator roomGenerator;
    private GameObject player;
    public int contador;
    public GameObject bossPrefab; // arraste o prefab do boss aqui pelo Inspector
    private GameObject currentBoss; // referência ao boss instanciado, se quiser manter controle

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
        contador++;

        // Destroi o boss da fase anterior, se ainda existir
        if (currentBoss != null)
        {
            Destroy(currentBoss);
            currentBoss = null;
        }

        // Limpa a dungeon
        roomGenerator.ClearDungeon();

        // Recria a dungeon
        roomGenerator.CreateRooms();

        // Reposiciona o jogador
        if (player != null)
        {
            player.transform.position = new Vector3(
                roomGenerator.playerSpawnPosition.x + 0.5f,
                roomGenerator.playerSpawnPosition.y + 0.5f,
                0
            );
        }

        // Move o portal
        Vector2 portalPosition = roomGenerator.GetLastRoomPosition();
        transform.position = new Vector3(
            portalPosition.x + 0.5f,
            portalPosition.y + 0.5f,
            0
        );

        // Reposiciona a loja principal
        GameObject shop = GameObject.FindWithTag("Shop");
        if (shop != null)
        {
            shop.transform.position = new Vector3(
                roomGenerator.shopSpawnPosition.x + 2f,
                roomGenerator.shopSpawnPosition.y + 2f,
                0
            );
        }

        // Resetar todas as lojas
        ShopSystem[] allShops = FindObjectsOfType<ShopSystem>();
        foreach (ShopSystem shopSystem in allShops)
        {
            shopSystem.ResetShop();
        }

        // Spawna o boss a cada 5 leveis
        if (contador % 5 == 0)
        {
            Vector3 bossSpawnPos = new Vector3(
                roomGenerator.BossSpawnPosition.x + 0.5f,
                roomGenerator.BossSpawnPosition.y + 0.5f,
                0
            );

            currentBoss = Instantiate(bossPrefab, bossSpawnPos, Quaternion.identity);
        }
    }

}
