using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyIncrement : MonoBehaviour
{
    private Player_Controller player;
    [SerializeField] public int energyIncrement;
    private HealthBarUI healthBar;
    [SerializeField] public SpriteRenderer potion;

    private void Start()
    {
        // Tenta encontrar a barra de vida de forma mais segura
        healthBar = FindObjectOfType<HealthBarUI>();

        if (healthBar == null)
        {
            Debug.LogError("HealthBarUI n�o encontrado! Certifique-se de que h� um objeto com este script na cena.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colis�o detectada com: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colis�o com Player confirmada.");
            Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();
            SoundEffectManager.Play("Energy");
            if (player != null)
            {
                Debug.Log("Player_Controller encontrado.");

                player.MaxEnergy += energyIncrement;
                player.Energy += energyIncrement;

                if (healthBar != null)
                {
                    healthBar.AtualizarEnergiaMaxima(player.MaxEnergy);
                    healthBar.sliderEnergy.value = player.Energy;
                }

                else
                {
                    Debug.LogError("healthBar ainda � null!");
                }

                Debug.Log("HP Atual: " + player.Energy);
                Debug.Log("HP M�ximo: " + player.MaxEnergy);

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("O Player n�o tem o componente Player_Controller!");
            }
        }
    }
}
