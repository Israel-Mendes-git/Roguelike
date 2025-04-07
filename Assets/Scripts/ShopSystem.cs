using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    private Player_Controller playerController;
    [SerializeField] private int coinRequery;
    private Detection_controller detectionArea;

    [SerializeField] private List<GameObject> itemsToSell; // Lista de itens dispon�veis na loja
    private GameObject currentItem; // Item atual que est� sendo vendido
    [SerializeField] private Transform localItem; // Local onde a arma ser� exibida na loja
    private GameObject displayedItem; // Refer�ncia ao item instanciado na loja

    private void Start()
    {
        playerController = FindObjectOfType<Player_Controller>(); // Obt�m o Player_Controller na cena
        detectionArea = GetComponentInChildren<Detection_controller>();
        ChooseRandomItem();
    }

    private void ChooseRandomItem()
    {
        if (itemsToSell.Count > 0)
        {
            int randomIndex = Random.Range(0, itemsToSell.Count);
            currentItem = itemsToSell[randomIndex]; // Escolhe um item aleat�rio da lista

            // Remove o item anterior na loja (se houver)
            if (displayedItem != null)
            {
                Destroy(displayedItem);
            }

            // Instancia o novo item na posi��o do localItem
            displayedItem = Instantiate(currentItem, localItem);
            displayedItem.transform.localPosition = Vector3.zero;
            displayedItem.transform.localRotation = Quaternion.identity;

            // Impede que o jogador pegue o item sem comprar
            Collider2D itemCollider = displayedItem.GetComponent<Collider2D>();
            if (itemCollider != null)
            {
                itemCollider.enabled = false;
            }

            Debug.Log("Item selecionado para venda: " + currentItem.name);
        }
        else
        {
            Debug.LogWarning("A lista de itens est� vazia!");
        }
    }

    private void Update()
    {
        if (detectionArea.detectedObjs.Count > 0 && Input.GetKeyDown(KeyCode.E))
        {
            BuyItem();
        }
    }

    private void BuyItem()
    {
        if (currentItem == null) return; // Se n�o houver item na loja, sai da fun��o

        if (playerController.Coin < coinRequery)
        {
            Debug.Log("Moedas insuficientes!");
            return;
        }

        playerController.Coin -= coinRequery; // Deduz as moedas do jogador

        if (currentItem.CompareTag("Arma") || currentItem.CompareTag("Espada"))
        {
            // Se j� existe uma arma no coldre principal, mov�-la para o secund�rio
            if (playerController.coldre.childCount > 0)
            {
                Transform oldWeapon = playerController.coldre.GetChild(0);

                if (playerController.coldreSecund�rio.childCount > 0)
                {
                    // Se j� h� uma arma no coldre secund�rio, descarta a antiga secund�ria
                    Transform oldSecondary = playerController.coldreSecund�rio.GetChild(0);
                    oldSecondary.SetParent(null);
                    oldSecondary.position = playerController.transform.position + new Vector3(2, 0, 0);
                    oldSecondary.gameObject.SetActive(true);
                }

                oldWeapon.SetParent(playerController.coldreSecund�rio);
                oldWeapon.localPosition = Vector3.zero;
                oldWeapon.localRotation = Quaternion.identity;
                oldWeapon.gameObject.SetActive(false);
            }

            // Instancia o novo item e coloca no coldre principal
            GameObject newItem = Instantiate(currentItem, playerController.coldre);
            newItem.transform.localPosition = Vector3.zero;
            newItem.transform.localRotation = Quaternion.identity;
            newItem.SetActive(true);

            // Verifica se a arma tem o script SistemaArma e ativa o disparo
            SistemaArma sistemaArma = newItem.GetComponent<SistemaArma>();
            if (sistemaArma != null)
            {
                sistemaArma.CDTiro();
            }

            Debug.Log("Comprou: " + currentItem.name);
        }
        else if (currentItem.CompareTag("Potions"))
        {
            // Cria a po��o no invent�rio do jogador
            GameObject potion = Instantiate(currentItem, playerController.transform.position, Quaternion.identity);
            potion.SetActive(true);
            Debug.Log("Comprou: " + currentItem.name);
        }

        // Remove o item da loja e gera um novo
        Destroy(localItem.gameObject);
        ChooseRandomItem();
    }
}
