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
    public bool WalkDir;
    public bool WalkEsq;
    public bool StopWalkDir;
    public bool StopWalkEsq;
    private Animator anim;
    private PortalManager portalManager;

    [SerializeField] GameObject Coin;

    [SerializeField]
    public int damage;
    public float HP;

    private void Awake()
    {
        // Atribui um componente de física à variável
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Encontra o Player_Controller globalmente
        playerController = FindObjectOfType<Player_Controller>();
    }

    void Start()
    {
        // A variável target recebe o valor do GameObject com a tag "Player"
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        portalManager = GameObject.FindObjectOfType<PortalManager>();
        BuffEnemy();
    }

    void Update()
    {
        if (target)
        {
            // Armazena a direção horizontal e vertical do movimento em um vetor 2D
            moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        UpdateAnimator();
        
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
        Debug.Log("Colisão detectada com: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Objeto detectado é uma bala ou espada");

            if (playerController != null && playerController.coldre != null)
            {
                
                if (collision.gameObject.CompareTag("Bullet"))
                {
                    SistemaArma arma = playerController.coldre.GetComponentInChildren<SistemaArma>();
                    if (arma != null)
                    {
                        Debug.Log("Bala detectada. Dano: " + arma.damage);
                        TakeDamage(arma.damage);
                    }
                    else
                    {
                        Debug.Log("Erro: Arma não encontrada no coldre!");
                    }
                }
            }
        }
    }

    void BuffEnemy()
    {
        Debug.Log(portalManager.contador);
        if(portalManager.contador % 3 == 0 && (portalManager.contador > 0))
        {
            HP += 5;
            damage += 1;
            Debug.Log(damage);
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
        playerController.AddEnemyKill(false); // Para inimigos comuns
        Destroy(gameObject);
        Instantiate(Coin, transform.position, Quaternion.identity);
        
    }
    void UpdateAnimator()
    {
        if (moveDirection.x > 0) // Movendo para a direita
        {
            WalkDir = true;
            WalkEsq = false;
            StopWalkDir = false;
            StopWalkEsq = false;

            anim.SetBool("WalkDir", true);
            anim.SetBool("WalkEsq", false);
            anim.SetBool("StopWalkDir", false);
            anim.SetBool("StopWalkEsq", false);
        }
        else if (moveDirection.x < 0) // Movendo para a esquerda
        {
            WalkEsq = true;
            WalkDir = false;
            StopWalkDir = false;
            StopWalkEsq = false;

            anim.SetBool("WalkEsq", true);
            anim.SetBool("WalkDir", false);
            anim.SetBool("StopWalkDir", false);
            anim.SetBool("StopWalkEsq", false);
        }
        else // Jogador parou
        {
            if (WalkDir) // Se estava indo para a direita
            {
                StopWalkDir = true;
                StopWalkEsq = false;
            }
            else if (WalkEsq) // Se estava indo para a esquerda
            {
                StopWalkEsq = true;
                StopWalkDir = false;
            }

            WalkDir = false;
            WalkEsq = false;

            anim.SetBool("WalkDir", false);
            anim.SetBool("WalkEsq", false);
            anim.SetBool("StopWalkDir", StopWalkDir);
            anim.SetBool("StopWalkEsq", StopWalkEsq);
        }
    }
}
