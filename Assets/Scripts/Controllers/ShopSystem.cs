using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShopSystem : MonoBehaviour
{
    private Player_Controller playerController;
    [SerializeField] public int coinRequery;
    private Detection_controller detectionArea;

    [SerializeField] private List<GameObject> itemsToSell; // Lista de todos os itens da loja
    private GameObject currentItem; // Item atual da loja
    [SerializeField] private Transform localItem; // Ponto onde o item é exibido
    private GameObject displayedItem; // Referência ao item instanciado na loja
    [SerializeField] public DialogueTrigger dialogueTrigger;
    [SerializeField] private GameObject ShopInfo;
    private Animator anim;



    public void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();
        detectionArea = GetComponentInChildren<Detection_controller>();
        anim = ShopInfo.GetComponent<Animator>();
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
            Debug.LogWarning("A lista de itens da loja está vazia!");
        }
    }

    private void Update()
    {
        if (detectionArea.detectedObjs.Count > 0 && Input.GetKeyDown(KeyCode.E))
        {
            BuyItem();
        }

        if (detectionArea.detectedObjs.Count > 0)
        {
            ShopInfo.gameObject.SetActive(true);
            anim.Play("ShopInfo");
            ShowShopInfo();
        }
        else
        {
            anim.Play("hideShopInfo");
            ShopInfo.gameObject.SetActive(false);
        }
        
    }

    private void ShowShopInfo()
    {
        if (currentItem.CompareTag("Arma") || currentItem.CompareTag("Espada"))
        {
            ShopInfo.GetComponent<ShopInfo>().NameTxt.text = $"{currentItem.name}";
            ShopInfo.GetComponent<ShopInfo>().DamageTxt.text = $"Dano: {currentItem.GetComponent<SistemaArma>().damage}";
            ShopInfo.GetComponent<ShopInfo>().EnergyTxt.text = $"Gasto de energia: {currentItem.GetComponent<SistemaArma>().energy}";
            ShopInfo.GetComponent<ShopInfo>().CooldownTxt.text = $"Cooldown: {currentItem.GetComponent<SistemaArma>().tempoEntreTiros}";
            ShopInfo.GetComponent<ShopInfo>().WeaponImage.sprite = currentItem.GetComponent<SistemaArma>().srWeapon.sprite;
            ShopInfo.GetComponent<ShopInfo>().CostTxt.text = $"Custo: {coinRequery}";
        }
        if(currentItem.CompareTag("Potions"))
        {
            ShopInfo.GetComponent<ShopInfo>().NameTxt.text = $"{currentItem.name}";
            ShopInfo.GetComponent<ShopInfo>().DamageTxt.text = null;
            ShopInfo.GetComponent<ShopInfo>().EnergyTxt.text = $"+{currentItem.GetComponent<HealthPotion>().Heal} de vida";
            if(currentItem.GetComponent<HealthPotion>() == null)
            {
                ShopInfo.GetComponent<ShopInfo>().EnergyTxt.text = $"+{currentItem.GetComponent<HealthIncrement>().HPIncrement} de vida máxima";
                ShopInfo.GetComponent<ShopInfo>().WeaponImage.sprite = currentItem.GetComponent<HealthIncrement>().potion.sprite;
                if (currentItem.GetComponent<HealthIncrement>() == null)
                {
                    ShopInfo.GetComponent<ShopInfo>().EnergyTxt.text = $"+{currentItem.GetComponent<EnergyPotion>().EnergyRestore} de energia";
                    ShopInfo.GetComponent<ShopInfo>().WeaponImage.sprite = currentItem.GetComponent<EnergyPotion>().potion.sprite;
                    if (currentItem.GetComponent<EnergyPotion>() == null)
                    {
                        ShopInfo.GetComponent<ShopInfo>().WeaponImage.sprite = currentItem.GetComponent<EnergyIncrement>().potion.sprite;
                        ShopInfo.GetComponent<ShopInfo>().EnergyTxt.text = $"+{currentItem.GetComponent<EnergyIncrement>().energyIncrement} de energia máxima";
                    }
                }
                
            }
            ShopInfo.GetComponent<ShopInfo>().CooldownTxt.text = null;
            ShopInfo.GetComponent<ShopInfo>().WeaponImage.sprite = currentItem.GetComponent<HealthPotion>().potion.sprite;
            ShopInfo.GetComponent<ShopInfo>().CostTxt.text = $"Custo: {coinRequery}";
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

                if (playerController.coldreSecundário.childCount > 0)
                {
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

        // Após a compra, remove o item da loja
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
