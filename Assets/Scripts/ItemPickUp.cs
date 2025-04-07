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
        else // Se ambos os coldres estiverem ocupados, dropar a arma principal e equipar a nova arma
        {
            DropWeapon();
            this.gameObject.transform.SetParent(player.coldre);
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void DropWeapon()
    {
        if (player.coldre.childCount > 0) // Se houver arma no coldre principal
        {
            Transform armaParaDropar = player.coldre.GetChild(0); // Pega a arma do coldre principal
            armaParaDropar.SetParent(null); // Remove do coldre
            armaParaDropar.position = player.transform.position + new Vector3(2, 0, 0); // Posiciona no mundo
            armaParaDropar.gameObject.SetActive(true); // Ativa no mundo

            if (player.coldreSecund�rio.childCount > 0) // Se houver arma no coldre secund�rio
            {
                Transform armaSecundaria = player.coldreSecund�rio.GetChild(0);
                armaSecundaria.SetParent(player.coldre); // Move a secund�ria para o coldre principal
                armaSecundaria.localPosition = Vector3.zero;
                armaSecundaria.localRotation = Quaternion.identity;

                // Remover a arma do coldre secund�rio
                armaSecundaria.SetParent(null);
            }
        }
    }
}
