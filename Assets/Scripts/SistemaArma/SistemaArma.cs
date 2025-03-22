using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaArma : MonoBehaviour
{
    Vector2 mousePosi;
    Vector2 dirArma;
    float angle;

    [SerializeField] SpriteRenderer srGun;
    [SerializeField] float tempoEntreTiros;
    bool podeAtirar = true;

    [SerializeField] Transform pontoDeFogo;
    [SerializeField] GameObject tiro;
    [SerializeField] HealthBarUI healthBarUI;

    private ItemPickUp itemPickUp; // Agora privado para evitar confusão

    private void Start()
    {
        // Busca o ItemPickUp no próprio objeto
        itemPickUp = GetComponent<ItemPickUp>();

        // Se não encontrar no mesmo objeto, tenta na "Gun"
        if (itemPickUp == null)
        {
            GameObject gunObj = GameObject.Find("PistolaPadrão");
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
        }
    }

    private void FixedUpdate()
    {
        dirArma = mousePosi - (Vector2)transform.position;
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        srGun.flipX = !(angle >= 100 || angle < 0);
    }

    void CDTiro()
    {
        podeAtirar = true;
    }
}
