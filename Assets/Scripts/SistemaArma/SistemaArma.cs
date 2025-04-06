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
    public bool podeAtirar = false;
    private Player_Controller controller;

    [SerializeField] Transform pontoDeFogo;
    [SerializeField] GameObject tiro;
    [SerializeField] public float damage;
    private Enemy_controller enemy;
    private RangedEnemy ranged;

    private void Start()
    {
        controller = FindObjectOfType<Player_Controller>(); // Garante que pegamos o controlador do jogador
        enemy = FindObjectOfType<Enemy_controller>();
        ranged = FindObjectOfType<RangedEnemy>();
    }

    void Update()
    {
        // Se a arma não estiver no coldre principal, não faz nada
        if (controller == null || transform.parent != controller.coldre)
            return;

        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && podeAtirar)
        {
            podeAtirar = false;
            Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);
            Invoke("CDTiro", tempoEntreTiros);
        }
    }

    private void FixedUpdate()
    {
        // Se a arma não estiver no coldre principal, não faz nada
        if (controller == null || transform.parent != controller.coldre)
            return;

        dirArma = mousePosi - (Vector2)transform.position;
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        srGun.flipX = !(angle >= 100 || angle < 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RangedEnemy"))
        {
            ranged.TakeDamage(damage);
        }
        if (collision.gameObject.CompareTag("Enemy"))
            enemy.TakeDamage(damage);
    }

    void CDTiro()
    {
        podeAtirar = true;
    }
}
