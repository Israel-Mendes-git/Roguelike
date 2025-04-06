using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private float meleeSpeed;

    [SerializeField] public float damage;
    [SerializeField] public SpriteRenderer srWeapon;

    private Player_Controller controller;
    private Enemy_controller enemy;
    private RangedEnemy ranged;
    private BoxCollider2D boxCollider;

    float timeUntilMelee;

    private void Start()
    {
        controller = FindObjectOfType<Player_Controller>();
        enemy = FindObjectOfType<Enemy_controller>();
        ranged = FindObjectOfType<RangedEnemy>(); 
        boxCollider = GetComponent<BoxCollider2D>();

    }
    private void Update()
    {
        if (controller == null || transform.parent != controller.coldre)
            return;
        if (transform.parent == controller.coldre)
        {
            boxCollider.enabled = false;
        }
        UpdatePosition();
        if (timeUntilMelee <= 0f)
        {
            if (Input.GetMouseButtonDown(0) && (controller.mov.x > 0 || controller.StopWalkDir))
            {
                anim.SetTrigger("Attack");
                anim.ResetTrigger("AttackEsq");
                timeUntilMelee = meleeSpeed;
                boxCollider.enabled = true;
                
            }
            else if (Input.GetMouseButtonDown(0) && (controller.mov.x < 0 || controller.StopWalkEsq))
            {
                anim.SetTrigger("AttackEsq");
                anim.ResetTrigger("Attack");
                timeUntilMelee = meleeSpeed;
                boxCollider.enabled = true;
            }

           
        }
        else
        {
            timeUntilMelee -= Time.deltaTime;
        }

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

    void EnemyDie()
    {
        Destroy(gameObject);
    }

    void UpdatePosition()
    {
        if (controller.mov.x < 0) // Movendo para a esquerda
        {
            srWeapon.flipX = true;
        }
        else if(controller.mov.x > 0)
        {
            srWeapon.flipX = false;
        }
        else
        {
            if (controller. WalkDir) // Se estava indo para a direita
            {
                srWeapon.flipX = false;
            }
            else if (controller.WalkEsq) // Se estava indo para a esquerda
            {
                srWeapon.flipX = true;
            }
        }
    }
}
