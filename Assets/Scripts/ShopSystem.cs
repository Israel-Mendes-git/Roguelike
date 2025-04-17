using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShopSystem : MonoBehaviour
{
    private Player_Controller playerController;
    [SerializeField] private int coinRequery;
    private Detection_controller detectionArea;

    [SerializeField] private List<GameObject> itemsToSell; // Lista de todos os itens da loja
    private GameObject currentItem; // Item atual da loja
    [SerializeField] private Transform localItem; // Ponto onde o item � exibido
    private GameObject displayedItem; // Refer�ncia ao item instanciado na loja
    [SerializeField] public DialogueTrigger dialogueTrigger;


    public void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();
        detectionArea = GetComponentInChildren<Detection_controller>();
        ChooseRandomItem();
    }

    public void ChooseRandomItem()
    {
        List<GameObject> availableItems = new List<GameObject>(itemsToSell);

        if (availableItems.Count > 0)
        {
            int randomIndex = Random.Range(0, availableItems.Count);
            currentItem = availableItems[randomIndex];

            if (displayedItem != null)
            {
                Destroy(displayedItem);
            }

            displayedItem = Instantiate(currentItem, localItem);
            displayedItem.transform.localPosition = Vector3.zero;
            displayedItem.transform.localRotation = Quaternion.identity;

            Collider2D itemCollider = displayedItem.GetComponent<Collider2D>();
            if (itemCollider != null)
            {
                itemCollider.enabled = false;
            }

            Debug.Log("Item selecionado para venda: " + currentItem.name);
        }
        else
        {
            Debug.LogWarning("A lista de itens da loja est� vazia!");
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
        if (currentItem == null) return;

        if (playerController.Coin < coinRequery)
        {
            Debug.Log("Moedas insuficientes!");
            dialogueTrigger.TriggerDialogue();
            
            return;
        }

        playerController.Coin -= coinRequery;

        if (currentItem.CompareTag("Arma") || currentItem.CompareTag("Espada"))
        {
            if (playerController.coldre.childCount > 0)
            {
                Transform oldWeapon = playerController.coldre.GetChild(0);

                if (playerController.coldreSecund�rio.childCount > 0)
                {
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

            GameObject newItem = Instantiate(currentItem, playerController.coldre);
            newItem.transform.localPosition = Vector3.zero;
            newItem.transform.localRotation = Quaternion.identity;
            newItem.SetActive(true);

            SistemaArma sistemaArma = newItem.GetComponent<SistemaArma>();
            if (sistemaArma != null)
            {
                sistemaArma.CDTiro();
            }

            Debug.Log("Comprou: " + currentItem.name);
        }
        else if (currentItem.CompareTag("Potions"))
        {
            GameObject potion = Instantiate(currentItem, playerController.transform.position, Quaternion.identity);
            potion.SetActive(true);
            Debug.Log("Comprou: " + currentItem.name);
        }

        // Ap�s a compra, remove o item da loja
        currentItem = null;

        if (localItem.childCount > 0)
        {
            Destroy(localItem.GetChild(0).gameObject);
        }
    }

    public void ResetShop()
    {
        if (localItem.childCount > 0)
        {
            Destroy(localItem.GetChild(0).gameObject);
        }

        ChooseRandomItem();
        Debug.Log(displayedItem);
    }


    
}
