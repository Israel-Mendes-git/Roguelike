using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Player_Controller player;

    private void Start()
    {
        // Encontrar o Player_Controller na cena
        player = FindObjectOfType<Player_Controller>();

        if (player == null)
        {
            Debug.LogError("Player_Controller não foi encontrado na cena!");
            return;
        }

        if (slider == null)
        {
            Debug.LogError("O Slider não foi atribuído ao HealthBarUI!");
            return;
        }

        // Definir o valor máximo e o valor inicial como 10
        slider.maxValue = 10;
        slider.value = 10; // Agora o Slider começa com 10
    }

    private void Update()
    {
        if (player != null && slider != null)
        {
            slider.value = player.HP; // Atualiza o Slider com o HP do jogador
        }
    }
}
