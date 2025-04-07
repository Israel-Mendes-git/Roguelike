using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_controller : MonoBehaviour
{
    //===VARIÁVEIS DO ENEMY CONTROLLER===//
    public float moveSpeed = 2f;
    Rigidbody2D rig;
    public Transform target;
    Vector2 moveDirection;
    public Detection_controller detectionArea;
    public Player_Controller playerController; // Pegamos o Player_Controller do Player

    [SerializeField] GameObject Coin;

    [SerializeField]
    public int damage;
    public float HP;

    private void Awake()
    {
        // Atribui um componente de física à variável
        rig = GetComponent<Rigidbody2D>();

        // Encontra o Player_Controller globalmente
        playerController = FindObjectOfType<Player_Controller>();
    }

    void Start()
    {
        // A variável target recebe o valor do GameObject com a tag "Player"
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target)
        {
            // Armazena a direção horizontal e vertical do movimento em um vetor 2D
            moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }

    private void FixedUpdate()
    {
        // Caso a quantidade de objetos dentro da área de detecção seja maior que 0
        if (detectionArea.detectedObjs.Count > 0)
        {
            // A direção do movimento do inimigo é até o player
            moveDirection = (detectionArea.detectedObjs[0].transform.position - transform.position).normalized;

            rig.MovePosition(rig.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Espada"))
        {
            if (playerController != null && playerController.coldre != null)
            {
                SistemaArma arma = playerController.coldre.GetComponentInChildren<SistemaArma>();
                MeleeAttack sword = playerController.coldre.GetComponentInChildren<MeleeAttack>();

                if (collision.gameObject.CompareTag("Bullet"))
                {
                    TakeDamage(arma.damage);
                }
                else if (collision.gameObject.CompareTag("Espada"))
                {
                    TakeDamage(sword.damage);
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log("HP do inimigo: " + HP);
        if (HP <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
        Instantiate(Coin, transform.position, Quaternion.identity);
    }
}
