using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    private Player_Controller playerController;
    [SerializeField] private int coinRequery;
    private Detection_controller detectionArea;

    [SerializeField] private List<GameObject> itemsToSell; // Lista de itens disponíveis na loja
    private GameObject currentItem; // Item atual que está sendo vendido
    [SerializeField] private Transform localItem; // Local onde a arma será exibida na loja
    private GameObject displayedItem; // Referência ao item instanciado na loja

    private void Start()
    {
        playerController = FindObjectOfType<Player_Controller>(); // Obtém o Player_Controller na cena
        detectionArea = GetComponentInChildren<Detection_controller>();
        ChooseRandomItem();
    }

    private void ChooseRandomItem()
    {
        if (itemsToSell.Count > 0)
        {
            int randomIndex = Random.Range(0, itemsToSell.Count);
            currentItem = itemsToSell[randomIndex]; // Escolhe um item aleatório da lista

            // Remove o item anterior na loja (se houver)
            if (displayedItem != null)
            {
                Destroy(displayedItem);
            }

            // Instancia o novo item na posição do localItem
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
            Debug.LogWarning("A lista de itens está vazia!");
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
        if (currentItem == null) return; // Se não houver item na loja, sai da função

        if (playerController.Coin < coinRequery)
        {
            Debug.Log("Moedas insuficientes!");
            return;
        }

        playerController.Coin -= coinRequery; // Deduz as moedas do jogador

        if (currentItem.CompareTag("Arma") || currentItem.CompareTag("Espada"))
        {
            // Se já existe uma arma no coldre principal, movê-la para o secundário
            if (playerController.coldre.childCount > 0)
            {
                Transform oldWeapon = playerController.coldre.GetChild(0);

                if (playerController.coldreSecundário.childCount > 0)
                {
                    // Se já há uma arma no coldre secundário, descarta a antiga secundária
                    Transform oldSecondary = playerController.coldreSecundário.GetChild(0);
                    oldSecondary.SetParent(null);
                    oldSecondary.position = playerController.transform.position + new Vector3(2, 0, 0);
                    oldSecondary.gameObject.SetActive(true);
                }

                oldWeapon.SetParent(playerController.coldreSecundário);
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
            // Cria a poção no inventário do jogador
            GameObject potion = Instantiate(currentItem, playerController.transform.position, Quaternion.identity);
            potion.SetActive(true);
            Debug.Log("Comprou: " + currentItem.name);
        }

        // Remove o item da loja e gera um novo
        Destroy(localItem.gameObject);
        ChooseRandomItem();
    }
}
