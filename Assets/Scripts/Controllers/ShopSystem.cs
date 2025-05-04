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
    private bool isShowingInfo = false;
    



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
            SoundEffectManager.Play("Buy");
            BuyItem();
        }

        if (detectionArea.detectedObjs.Count > 0 && currentItem != null)
        {
            if (!isShowingInfo)
            {
                ShowShopInfo();
                isShowingInfo = true;
            }
        }
        else
        {
            if (isShowingInfo)
            {
                anim.Play("hideShopInfo");
                isShowingInfo = false;
            }
        }

    }


    private void ShowShopInfo()
    {
        if (currentItem == null) return;

        if (ShopInfo == null)
        {
            Debug.LogError("❌ ShopInfo GameObject está nulo!");
            return;
        }

        var info = ShopInfo.GetComponent<ShopInfo>();
        if (info == null)
        {
            Debug.LogError("❌ Script 'ShopInfo' não está anexado ao GameObject!");
            return;
        }

        anim.Play("ShopInfo");

        info.NameTxt.text = currentItem.name;
        info.CostTxt.text = $"Custo: {coinRequery}";


        // Verifica se é uma arma
        if (currentItem.CompareTag("Arma") || currentItem.CompareTag("Espada"))
        {
            var arma = currentItem.GetComponent<SistemaArma>();
            if (arma != null)
            {
                info.DamageTxt.text = $"Dano: {arma.damage}";
                info.EnergyTxt.text = $"Gasto de energia: {arma.energy}";
                info.CooldownTxt.text = $"Cooldown: {arma.tempoEntreTiros}";
                info.WeaponImage.sprite = arma.srWeapon.sprite;
            }
        }
        // Verifica se é uma poção
        else if (currentItem.CompareTag("Potions"))
        {
            // Tenta encontrar o tipo de poção específico
            var potionHeal = currentItem.GetComponent<HealthPotion>();
            var potionHP = currentItem.GetComponent<HealthIncrement>();
            var potionEnergy = currentItem.GetComponent<EnergyPotion>();
            var potionEnergyInc = currentItem.GetComponent<EnergyIncrement>();

            info.DamageTxt.text = null;
            info.CooldownTxt.text = null;

            if (potionHeal != null)
            {
                info.EnergyTxt.text = $"+{potionHeal.Heal} de vida";
                info.WeaponImage.sprite = potionHeal.potion.sprite;
            }
            else if (potionHP != null)
            {
                info.EnergyTxt.text = $"+{potionHP.HPIncrement} de vida máxima";
                info.WeaponImage.sprite = potionHP.potion.sprite;
            }
            else if (potionEnergy != null)
            {
                info.EnergyTxt.text = $"+{potionEnergy.EnergyRestore} de energia";
                info.WeaponImage.sprite = potionEnergy.potion.sprite;
            }
            else if (potionEnergyInc != null)
            {
                info.EnergyTxt.text = $"+{potionEnergyInc.energyIncrement} de energia máxima";
                info.WeaponImage.sprite = potionEnergyInc.potion.sprite;
            }
            else
            {
                info.EnergyTxt.text = "Poção desconhecida";
                info.WeaponImage.sprite = null;
            }
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
            Collider2D collider = newItem.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
                Debug.Log($"Colisor desativado da arma comprada: {newItem.name}");
            }


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


     
