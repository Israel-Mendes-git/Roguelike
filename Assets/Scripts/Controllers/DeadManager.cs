using UnityEngine;
using UnityEngine.UI;

public class DeadManager : MonoBehaviour
{
    [SerializeField] private Text EnemyPointsText;
    [SerializeField] private Text RangedEnemyPointsText;

    public void ShowScore(int enemyPoints, int rangedEnemyPoints)
    {
        EnemyPointsText.text = $"Inimigos simples mortos: {enemyPoints}";
        RangedEnemyPointsText.text = $"Inimigos de longo alcance mortos: {rangedEnemyPoints}";


    }
}
