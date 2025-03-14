using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private Transform coldre;
    private Transform pistola;
    private Transform coldreSmg;
    private Transform Smg;
    public bool isEquip;
    public string WeaponActive;


    void Start()
    {
        
        coldre = GameObject.Find("ColdrePistola").transform;
        pistola = GameObject.Find("PistolaPadrão").transform;
        coldreSmg = GameObject.Find("ColdreSmg").transform;
        Smg = GameObject.Find("Smg").transform;

    }

    private void Update()
    {
        // Se o jogador pressionar "G", dropa o item
        if (isEquip && Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUpItem();
        }
    }

    void PickUpItem()
    {
        isEquip = true;
        if (coldre.name == "ColdrePistola")
        {
            transform.SetParent(coldre);
            WeaponActive = pistola.name;
            
        }
        if(coldreSmg.name == "ColdreSmg")
        {
            transform.SetParent(coldreSmg);
            WeaponActive = Smg.name;
        }
        
        transform.localPosition = new Vector2(0f, 0f);
        transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);

        pistola.rotation = new Quaternion(0f, 0f, 0f, 0f);
        Smg.rotation = new Quaternion(0f, 0f, 0f, 0f);

        WeaponActivated();

    }

    void DropItem()
    {
        isEquip = false;
        transform.SetParent(null); // Remove o item do jogador

        // Move o item um pouco para frente do jogador
        Vector3 dropPosition = transform.position + new Vector3(1.5f, 0f, 0f);
        transform.position = dropPosition;
    }

    void WeaponActivated()
    {
        switch (WeaponActive)
        {
            case "PistolaPadrão":
                Debug.Log("Pistola é a arma padrão");

                break;
            case "Smg":
                Debug.Log("Smg é a arma padrão");
                break;

        }

    }
}