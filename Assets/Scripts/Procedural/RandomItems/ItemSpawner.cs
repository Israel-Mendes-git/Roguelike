using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<ItemData> itemsToSpawn; // Lista de itens que podem ser gerados
    public GameObject itemPrefab; // Prefab do item a ser instanciado
    private ItemPlacementHelper placementHelper;

    public void Initialize(ItemPlacementHelper helper)
    {
        placementHelper = helper;
    }

    public void SpawnItems(int itemCount)
    {
        for (int i = 0; i < itemCount; i++)
        {
            PlacementType type = (Random.value > 0.5f) ? PlacementType.OpenSpace : PlacementType.NearWall;
            Vector2? spawnPosition = placementHelper.GetItemPlacementPosition(type, 10, Vector2Int.one);

            if (spawnPosition.HasValue)
            {
                Vector2 position = spawnPosition.Value;
                ItemData randomItem = itemsToSpawn[Random.Range(0, itemsToSpawn.Count)];
                GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity);
                newItem.GetComponent<Item>().Initialize(randomItem);
            }
        }
    }
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
}

public class Item : MonoBehaviour
{
    private ItemData itemData;

    public void Initialize(ItemData data)
    {
        itemData = data;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
    }
}
