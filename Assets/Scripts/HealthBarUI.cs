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
            Debug.LogError("Player_Controller n�o foi encontrado na cena!");
            return;
        }

        if (slider == null)
        {
            Debug.LogError("O Slider n�o foi atribu�do ao HealthBarUI!");
            return;
        }

        // Definir o valor m�ximo e o valor inicial como 10
        slider.maxValue = 10;
        slider.value = 10; // Agora o Slider come�a com 10
    }

    private void Update()
    {
        if (player != null && slider != null)
        {
            slider.value = player.HP; // Atualiza o Slider com o HP do jogador
        }
    }
}
