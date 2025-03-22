using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPotion : MonoBehaviour
{
    private Player_Controller player;
    private HealthBarUI healthPotion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();
            HealthBarUI healthBar = collision.gameObject.GetComponent<HealthBarUI>();
            if (player != null)
            {

                player.Energy += 30;
                if(player.Energy > 200)
                {
                    player.Energy = 200;
                    healthPotion.sliderEnergy.value = player.Energy;
                }

            }
        }
    }
}
