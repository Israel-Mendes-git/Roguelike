using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInfo : MonoBehaviour
{
    [SerializeField]
    public Text NameTxt;
    public Text CostTxt;
    public Text EnergyTxt;
    public Text DamageTxt;
    public Text CooldownTxt;
    public Image WeaponImage;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    

}
