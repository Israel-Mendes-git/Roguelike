using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private Player_Controller player;

    void Start()
    {
        player = FindObjectOfType<Player_Controller>();
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PickUpWeapon();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropWeapon();

            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider != null && transform.parent == null) // Só ativa se estiver no chão
                boxCollider.enabled = true;
        }
    }


    void PickUpWeapon()
    {
        if (player.coldre.childCount == 0) // Se não há arma no coldre principal
        {
            this.gameObject.transform.SetParent(player.coldre);
        }
        else if (player.coldreSecundário.childCount == 0) // Se há arma no coldre principal, mas não na secundária
        {
            Transform armaAntiga = player.coldre.GetChild(0);
            armaAntiga.SetParent(player.coldreSecundário);
            armaAntiga.localPosition = Vector3.zero;
            armaAntiga.localRotation = Quaternion.identity;
            this.gameObject.transform.SetParent(player.coldre);
        }
        else // Se ambos os coldres estiverem ocupados, dropar a principal e reposicionar as outras
        {
            DropWeapon(); // Isso promove a arma secundária pra principal

            if (player.coldre.childCount == 0) // Se o coldre ficou vazio após a promoção (por segurança)
            {
                this.gameObject.transform.SetParent(player.coldre);
            }
            else // Caso a secundária tenha sido promovida e agora temos uma arma no principal
            {
                this.gameObject.transform.SetParent(player.coldreSecundário);
            }
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Garante que qualquer colisor seja desativado
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
            Debug.Log($"Colisor da arma '{gameObject.name}' desativado após coleta.");
        }

        // Remove a marca de "arma dropada"
        DroppedWeaponIdentifier identifier = GetComponent<DroppedWeaponIdentifier>();
        if (identifier != null)
        {
            Destroy(identifier);
        }


    }


    void DropWeapon()
    {
        if (player.coldre.childCount > 0)
        {
            // Dropa a arma principal
            Transform armaPrincipal = player.coldre.GetChild(0);
            armaPrincipal.SetParent(null);
            armaPrincipal.position = player.transform.position + new Vector3(2, 0, 0);
            armaPrincipal.gameObject.SetActive(true);

            // Marca como dropada para ser destruída ao passar de fase
            if (armaPrincipal.GetComponent<DroppedWeaponIdentifier>() == null)
            {
                armaPrincipal.gameObject.AddComponent<DroppedWeaponIdentifier>();
            }

            // Reativa o collider
            BoxCollider2D boxCollider = armaPrincipal.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
                boxCollider.enabled = true;

            // Se existir arma secundária, ela vira a nova principal
            if (player.coldreSecundário.childCount > 0)
            {
                Transform armaSecundaria = player.coldreSecundário.GetChild(0);
                armaSecundaria.SetParent(null); // Remove do coldre secundário primeiro
                armaSecundaria.SetParent(player.coldre); // Depois move para o principal
                armaSecundaria.localPosition = Vector3.zero;
                armaSecundaria.localRotation = Quaternion.identity;
            }
        }
    }



}
