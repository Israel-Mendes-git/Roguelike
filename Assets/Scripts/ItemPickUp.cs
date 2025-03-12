using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private Transform arma;
    private Transform gun;
    public bool isEquip;


    void Start()
    {
        
        arma = GameObject.Find("Arma").transform;
        gun = GameObject.Find("Gun").transform;
        
        
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
        transform.SetParent(arma);

        transform.localPosition = new Vector2(0f, 0f);
        transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);

        gun.rotation = new Quaternion(0f, 0f, 0f, 0f);


    }

    void DropItem()
    {
        isEquip = false;
        transform.SetParent(null); // Remove o item do jogador

        // Move o item um pouco para frente do jogador
        Vector3 dropPosition = transform.position + new Vector3(1.5f, 0f, 0f);
        transform.position = dropPosition;
    }
}