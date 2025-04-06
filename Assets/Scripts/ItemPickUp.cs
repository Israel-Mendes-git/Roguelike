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
            if (player.coldre.childCount == 0) // Se não houver arma no coldre principal, coloca lá
            {
                this.gameObject.transform.SetParent(player.coldre);
            }
            else if (player.coldreSecundário.childCount == 0) // Se o coldre principal já tiver uma arma, coloca no secundário
            {
                Transform armaAntiga = player.coldre.GetChild(0); // Pega a arma do coldre principal

                // Move a arma principal para o coldre secundário
                armaAntiga.SetParent(player.coldreSecundário);
                armaAntiga.localPosition = Vector3.zero;
                armaAntiga.localRotation = Quaternion.identity;

                // Coloca a nova arma no coldre principal
                this.gameObject.transform.SetParent(player.coldre);
            }
            else // Se os dois coldres já estiverem ocupados, dropar a arma secundária
            {
                Transform armaAntigaSecundaria = player.coldreSecundário.GetChild(0); // Pega a arma secundária antiga

                // Remove a arma secundária e joga no mundo
                armaAntigaSecundaria.SetParent(null);
                armaAntigaSecundaria.position = player.transform.position + new Vector3(2, 0, 0);
                armaAntigaSecundaria.gameObject.SetActive(true); // Ativa a arma dropada

                // Move a arma do coldre para o coldre secundário
                Transform armaAntiga = player.coldre.GetChild(0);
                armaAntiga.SetParent(player.coldreSecundário);
                armaAntiga.localPosition = Vector3.zero;
                armaAntiga.localRotation = Quaternion.identity;

                // Coloca a nova arma no coldre principal
                this.gameObject.transform.SetParent(player.coldre);
            }


            // Ajusta posição e rotação para ficar centralizado no coldre
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
