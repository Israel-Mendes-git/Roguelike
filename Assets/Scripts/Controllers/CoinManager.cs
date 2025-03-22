using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CoinManager : MonoBehaviour
{

    [SerializeField] Text coinTxt;

    public Player_Controller player;

    private void Start()
    {
        Player_Controller player = GetComponent<Player_Controller>();

    }

    private void Update()
    {
        coinTxt.text = $"{player.Coin}";
    }


}
