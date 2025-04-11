using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] public Slider sliderEnergy;
    private Player_Controller player;
    [SerializeField] private Text hpTxt; // Mantém apenas uma variável para o texto do HP
    [SerializeField] private Text energyTxt;

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

        if (hpTxt == null)
        {
            Debug.LogError("O TMP_Text hpTxt não foi atribuído ao HealthBarUI!");
            return;
        }

        // Definir o valor máximo e o valor inicial como 10
        slider.maxValue = player.HPMax;
        slider.value = player.HP; // Agora o Slider começa com 10
        sliderEnergy.maxValue = player.MaxEnergy;
        sliderEnergy.value = player.Energy;
        UpdateHealthUI(); // Atualiza a UI inicialmente
    }

    private void Update()
    {
        if (player != null && slider != null)
        {
            slider.value = player.HP; // Atualiza o Slider com o HP do jogador
            sliderEnergy.value = player.Energy;
            UpdateHealthUI(); // Atualiza o texto do HP
            
        }
    }

    public void UpdateHealthUI()
    {
        if (hpTxt != null && energyTxt != null)
        {
            hpTxt.text = $"{player.HP} / {slider.maxValue}"; // Atualiza o texto do HP
            energyTxt.text = $"{player.Energy} / {sliderEnergy.maxValue}";
        }
        else
        {
            Debug.Log("hpTxt é nulo");
        }
    }
}
