using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SistemaArma : MonoBehaviour
{
    Vector2 mousePosi;
    Vector2 dirArma;
    float angle;

    [SerializeField] SpriteRenderer srGun;
    [SerializeField] public float tempoEntreTiros;
    public bool podeAtirar = false;
    private Player_Controller controller;

    [SerializeField] Transform pontoDeFogo;
    [SerializeField] GameObject tiro;
    [SerializeField] public float damage;
    [SerializeField] public int energy;
    private Enemy_controller enemy;
    private RangedEnemy ranged;
    [SerializeField] public SpriteRenderer srWeapon;
    private float firePointOriginalX;


    private void Start()
    {
        controller = FindObjectOfType<Player_Controller>();
        enemy = FindObjectOfType<Enemy_controller>();
        ranged = FindObjectOfType<RangedEnemy>();

        firePointOriginalX = pontoDeFogo.localPosition.x; // Inicializa firePointOriginalX
    }


    void Update()
    {
        // Se a arma não estiver no coldre principal, não faz nada
        if (controller == null || transform.parent != controller.coldre)
            return;
        
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (controller.Energy <= 0) return;
        if (Input.GetMouseButton(0) && podeAtirar)
        {
            podeAtirar = false;
            Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);
            Invoke("CDTiro", tempoEntreTiros);
            controller.Energy -= energy;
        }
    }

    private void FixedUpdate()
    {
        // Se a arma não estiver no coldre principal, não faz nada
        if (controller == null || transform.parent != controller.coldre)
            return;

        // Calcula direção entre o jogador e o mouse
        Vector2 direction = mousePosi - (Vector2)controller.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Define a distância da arma em relação ao jogador
        float distanceFromPlayer = 1f; // ajuste conforme o tamanho da arma/sprite
        Vector2 offset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * distanceFromPlayer;

        // Posiciona a arma ao redor do jogador
        transform.position = (Vector2)controller.transform.position + offset;

        // Faz a arma apontar na direção do mouse
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        bool mirandoEsquerda = (angle > 90 || angle < -90);
        srWeapon.flipX = mirandoEsquerda;

        //// Reposiciona o ponto de fogo se estiver virando
        //Vector3 firePos = pontoDeFogo.localPosition;
        //firePos.x = mirandoEsquerda ? -Mathf.Abs(firePointOriginalX) : Mathf.Abs(firePointOriginalX);
        //pontoDeFogo.localPosition = firePos;

        
    }



    public void CDTiro()
    {
        podeAtirar = true;
    }
   
}
