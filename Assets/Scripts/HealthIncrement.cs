using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncrement : MonoBehaviour
{
    private Player_Controller player;
    [SerializeField] private int HPIncrement;
    private HealthBarUI healthBar;

    private void Start()
    {
        // Tenta encontrar a barra de vida de forma mais segura
        healthBar = FindObjectOfType<HealthBarUI>();

        if (healthBar == null)
        {
            Debug.LogError("HealthBarUI não encontrado! Certifique-se de que há um objeto com este script na cena.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisão detectada com: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colisão com Player confirmada.");
            Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();

            if (player != null)
            {
                Debug.Log("Player_Controller encontrado.");

                if (healthBar != null)
                {
                    healthBar.slider.maxValue += HPIncrement;
                    healthBar.slider.value += HPIncrement;
                }
                else
                {
                    Debug.LogError("healthBar ainda é null!");
                }

                player.HPMax += HPIncrement;
                player.HP += HPIncrement;

                Debug.Log("HP Atual: " + player.HP);
                Debug.Log("HP Máximo: " + player.HPMax);

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("O Player não tem o componente Player_Controller!");
            }
        }
    }
}
