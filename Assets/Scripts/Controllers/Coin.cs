using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value; // Quantidade de moedas que esta moeda representa

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();
            if (player != null)
            {
                player.Coin += value; // Adiciona o valor correto de moedas ao jogador
            }

            Destroy(gameObject);
        }
    }
}
