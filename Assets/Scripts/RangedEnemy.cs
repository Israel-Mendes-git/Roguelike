using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField] SpriteRenderer srEnemy;
    public Transform target;
    public float speed = 3f;
    public float rotateSpeed = 0.0025f;
    private Rigidbody2D rb;
    public GameObject bulletPrefab;
    private Animator anim;
    private bool isShot;
    public float distanceToShoot = 5f;
    public float distanceToStop = 3f;

    public float fireRate;
    private float timeToFire;
    public Detection_controller detectionArea;
    public Player_Controller playerController;
    public SistemaArma sistema;
    public MeleeAttack melee;
    [SerializeField] GameObject Coin;
    private PortalManager portalManager;

    public Transform firingPoint;
    public float HP;
    public int damage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detectionArea = GetComponentInChildren<Detection_controller>(); // Pega o script no filho
        anim = GetComponent<Animator>();
        sistema = GetComponent<SistemaArma>();
        melee = GetComponent<MeleeAttack>();
        playerController = FindObjectOfType<Player_Controller>();
        portalManager = GameObject.FindObjectOfType<PortalManager>();
        BuffEnemy();

    }

    void Update()
    {
        if (!target)
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget();
            if (Vector2.Distance(target.position, transform.position) <= distanceToShoot)
            {
                Shoot();
            }
            else
            {
                isShot = false;
                anim.SetBool("IsShot", isShot); 
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (target != null && detectionArea.detectedObjs.Contains(target.GetComponent<Collider2D>()))
        {
            if (Vector2.Distance(target.position, transform.position) >= distanceToStop)
            {
                rb.velocity = (target.position - transform.position).normalized * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            target = null; // Reseta o alvo se ele sair da área de detecção
        }
    }

    private void GetTarget()
    {
        if (detectionArea != null && detectionArea.detectedObjs.Count > 0)
        {
            foreach (var col in detectionArea.detectedObjs)
            {
                if (col.CompareTag("Player"))
                {
                    target = col.transform; // Agora estamos acessando o transform do Collider2D corretamente
                    return;
                }
            }
        }

        // Se não encontrou um alvo válido, zera o target e para o inimigo
        target = null;
        rb.velocity = Vector2.zero;
    }



    private void RotateTowardsTarget()
    {
        if (target == null) return;
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 180f;
        srEnemy.flipX = !(angle >= 100 || angle < 0);
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }

    private void Shoot()
    {
        if (timeToFire <= 0f)
        {
            Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
            timeToFire = fireRate;
            isShot = true;
            anim.SetBool("IsShot", isShot);

        }
        else
        {
            timeToFire -= Time.deltaTime;

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

    void BuffEnemy()
    {
        int multiplicador = portalManager.contador / 3;

        if (multiplicador > 0)
        {
            HP += 2 * multiplicador;
            damage += 2 * multiplicador;
            Debug.Log($"Buff aplicado! x{multiplicador} → HP: {HP}, Dano: {damage}");
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log("HP do inimigo: " + HP);
        if (HP <= 0)
            Die();
    }
    void Die()
    {
        playerController.AddEnemyKill(true); // Para inimigos de longo alcance
        Destroy(gameObject);
        Instantiate(Coin, transform.position, Quaternion.identity);
        
    }
}
