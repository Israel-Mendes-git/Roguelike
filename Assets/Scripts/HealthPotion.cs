using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
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
                
                player.HP += 4;
                if (player.HP > 10)
                {
                    player.HP = 10;
                    healthPotion.slider.value = player.HP;
                }
            }
        }
    }

}
