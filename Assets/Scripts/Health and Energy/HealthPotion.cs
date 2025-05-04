using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    private Player_Controller player;
    private HealthBarUI healthPotion;
    [SerializeField] public int Heal;
    [SerializeField]public SpriteRenderer potion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();
            HealthBarUI healthBar = collision.gameObject.GetComponent<HealthBarUI>();
            SoundEffectManager.Play("heal");
            if (player != null)
            {
                
                player.HP += Heal;
                if (player.HP > player.HPMax)
                {
                    player.HP = player.HPMax;
                    healthPotion.slider.value = player.HP;
                }
            }
        }
    }

}
