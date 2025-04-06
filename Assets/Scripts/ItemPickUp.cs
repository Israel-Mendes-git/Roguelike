using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private Player_Controller player;
    private SistemaArma sistemaArma;

    void Start()
    {
        player = FindObjectOfType<Player_Controller>();
        sistemaArma = FindObjectOfType<SistemaArma>();
        sistemaArma.podeAtirar = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.coldre.childCount == 0) // Se n�o houver arma no coldre principal, coloca l�
            {
                this.gameObject.transform.SetParent(player.coldre);
            }
            else if (player.coldreSecund�rio.childCount == 0) // Se o coldre principal j� tiver uma arma, coloca no secund�rio
            {
                Transform armaAntiga = player.coldre.GetChild(0); // Pega a arma do coldre principal

                // Move a arma principal para o coldre secund�rio
                armaAntiga.SetParent(player.coldreSecund�rio);
                armaAntiga.localPosition = Vector3.zero;
                armaAntiga.localRotation = Quaternion.identity;

                // Coloca a nova arma no coldre principal
                this.gameObject.transform.SetParent(player.coldre);
            }
            else // Se os dois coldres j� estiverem ocupados, dropar a arma secund�ria
            {
                Transform armaAntigaSecundaria = player.coldreSecund�rio.GetChild(0); // Pega a arma secund�ria antiga

                // Remove a arma secund�ria e joga no mundo
                armaAntigaSecundaria.SetParent(null);
                armaAntigaSecundaria.position = player.transform.position + new Vector3(2, 0, 0);
                armaAntigaSecundaria.gameObject.SetActive(true); // Ativa a arma dropada

                // Move a arma do coldre para o coldre secund�rio
                Transform armaAntiga = player.coldre.GetChild(0);
                armaAntiga.SetParent(player.coldreSecund�rio);
                armaAntiga.localPosition = Vector3.zero;
                armaAntiga.localRotation = Quaternion.identity;

                // Coloca a nova arma no coldre principal
                this.gameObject.transform.SetParent(player.coldre);
            }


            // Ajusta posi��o e rota��o para ficar centralizado no coldre
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
