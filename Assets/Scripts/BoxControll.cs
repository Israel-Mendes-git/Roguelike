using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxControll : MonoBehaviour
{
    [SerializeField] GameObject HealthPotion;
    [SerializeField] GameObject EnergyPotion;
    [SerializeField] GameObject BigHealthPotion;
    [SerializeField] GameObject BigEnergyPotion;
    [SerializeField] GameObject SmallHealthPotion;
    [SerializeField] GameObject SmallEnergyPotion;
    [SerializeField] Transform box;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || (collision.gameObject.CompareTag("Espada")))
        {
            DropItem(10);
            DestroyBox();
        }
    }



    void DropItem(int yes)
    {
        if (Rand() <= yes)
        {
            switch (RandDrop())
            {
                case 0:
                    Instantiate(HealthPotion, transform.position, Quaternion.identity);
                    Debug.Log("Dropou uma po��o de cura");
                    break;
                case 1:
                    Instantiate(EnergyPotion, transform.position, Quaternion.identity);
                    Debug.Log("Dropou uma po��o de energia");
                    break;
                case 2:
                    Instantiate(BigHealthPotion, transform.position, Quaternion.identity);
                    Debug.Log("Dropou uma po��o de cura grande");
                    break;
                case 3:
                    Instantiate(BigEnergyPotion, transform.position, Quaternion.identity);
                    Debug.Log("Dropou uma po��o de energia grande");
                    break;
                case 4: 
                    Instantiate(SmallEnergyPotion, transform.position, Quaternion.identity);
                    Debug.Log("Dropou uma po��o de energia pequena");
                    break;
                case 5:
                    Instantiate(SmallHealthPotion, transform.position, Quaternion.identity);
                    Debug.Log("Dropou uma po��o de cura pequena");
                    break;
            }
               
          
        }
        else
        {
            Debug.Log("N�o dropou nada");
        }
    }


    int Rand()
    {
        int rand = Random.Range(0, 100);
        return rand;
    }
    int RandDrop()
    {
        int rand = Random.Range(0, 6);
        return rand;
    }
    void DestroyBox()
    {
        Destroy(gameObject);
    }
}
