using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaPato : MonoBehaviour
{
    Vector2 mousePosi;
    Vector2 dirArma;
    float angle;

    [SerializeField] SpriteRenderer srGun;
    [SerializeField] float tempoEntreTiros;
    bool podeAtirar = true;

    [SerializeField] Transform pontoDeFogo;
    [SerializeField] GameObject tiro;
    [SerializeField] Player_Controller player;

    private ItemPickUp itemPickUp; // Agora privado para evitar confus�o

    private void Start()
    {
        // Busca o ItemPickUp no pr�prio objeto
        itemPickUp = GetComponent<ItemPickUp>();

        // Se n�o encontrar no mesmo objeto, tenta na "Gun"
        if (itemPickUp == null)
        {
            GameObject gunObj = GameObject.Find("Pato");
            if (gunObj != null)
            {
                itemPickUp = gunObj.GetComponent<ItemPickUp>();
            }
        }
    }

    void Update()
    {
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Verifica se itemPickUp foi encontrado antes de acessar
        if (itemPickUp != null && itemPickUp.isEquip && Input.GetMouseButton(0) && podeAtirar)
        {
            podeAtirar = false;
            Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);
            Invoke("CDTiro", tempoEntreTiros);
            player.Energy -= 5;
        }
    }

    private void FixedUpdate()
    {
        dirArma = mousePosi - (Vector2)transform.position;
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    void CDTiro()
    {
        podeAtirar = true;
    }
}
