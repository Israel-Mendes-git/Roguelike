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
            if (boxCollider != null && transform.parent == null) // S� ativa se estiver no ch�o
                boxCollider.enabled = true;
        }
    }


    void PickUpWeapon()
    {
        if (player.coldre.childCount == 0) // Se n�o h� arma no coldre principal
        {
            this.gameObject.transform.SetParent(player.coldre);
        }
        else if (player.coldreSecund�rio.childCount == 0) // Se h� arma no coldre principal, mas n�o na secund�ria
        {
            Transform armaAntiga = player.coldre.GetChild(0);
            armaAntiga.SetParent(player.coldreSecund�rio);
            armaAntiga.localPosition = Vector3.zero;
            armaAntiga.localRotation = Quaternion.identity;
            this.gameObject.transform.SetParent(player.coldre);
        }
        else // Se ambos os coldres estiverem ocupados, dropar a principal e reposicionar as outras
        {
            DropWeapon(); // Isso promove a arma secund�ria pra principal

            if (player.coldre.childCount == 0) // Se o coldre ficou vazio ap�s a promo��o (por seguran�a)
            {
                this.gameObject.transform.SetParent(player.coldre);
            }
            else // Caso a secund�ria tenha sido promovida e agora temos uma arma no principal
            {
                this.gameObject.transform.SetParent(player.coldreSecund�rio);
            }
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Garante que qualquer colisor seja desativado
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
            Debug.Log($"Colisor da arma '{gameObject.name}' desativado ap�s coleta.");
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

            // Marca como dropada para ser destru�da ao passar de fase
            if (armaPrincipal.GetComponent<DroppedWeaponIdentifier>() == null)
            {
                armaPrincipal.gameObject.AddComponent<DroppedWeaponIdentifier>();
            }

            // Reativa o collider
            BoxCollider2D boxCollider = armaPrincipal.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
                boxCollider.enabled = true;

            // Se existir arma secund�ria, ela vira a nova principal
            if (player.coldreSecund�rio.childCount > 0)
            {
                Transform armaSecundaria = player.coldreSecund�rio.GetChild(0);
                armaSecundaria.SetParent(null); // Remove do coldre secund�rio primeiro
                armaSecundaria.SetParent(player.coldre); // Depois move para o principal
                armaSecundaria.localPosition = Vector3.zero;
                armaSecundaria.localRotation = Quaternion.identity;
            }
        }
    }



}
