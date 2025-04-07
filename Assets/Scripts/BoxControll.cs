using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxControll : MonoBehaviour
{
    [SerializeField] GameObject HealthPotion;
    [SerializeField] GameObject EnergyPotion;
    [SerializeField] Transform box;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || (collision.gameObject.CompareTag("Espada")))
        {
            DropItem(100);
            DestroyBox();
        }
    }



    void DropItem(int yes)
    {
        if (Rand() <= yes)
        {
            if (RandDrop() == 0)
            {
                Instantiate(HealthPotion, transform.position, Quaternion.identity);
                Debug.Log("Dropou uma poção de cura");
            }
            else if (RandDrop() == 1)
            {
                Instantiate(EnergyPotion, transform.position, Quaternion.identity);
                Debug.Log("Dropou uma poção de energia");
            }
        }
        else
        {
            Debug.Log("Não dropou nada");
        }
    }


    int Rand()
    {
        int rand = Random.Range(0, 100);
        return rand;
    }
    int RandDrop()
    {
        int rand = Random.Range(0, 2);
        return rand;
    }
    void DestroyBox()
    {
        Destroy(gameObject);
    }
}
