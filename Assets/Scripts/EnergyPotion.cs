using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPotion : MonoBehaviour
{
    private Player_Controller player;
    private HealthBarUI healthPotion;
    [SerializeField] private int EnergyRestore;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();
            HealthBarUI healthBar = collision.gameObject.GetComponent<HealthBarUI>();
            if (player != null)
            {

                player.Energy += EnergyRestore;
                if(player.Energy > player.MaxEnergy)
                {
                    player.Energy = player.MaxEnergy;
                    healthPotion.sliderEnergy.value = player.Energy;
                }

            }
        }
    }
}
