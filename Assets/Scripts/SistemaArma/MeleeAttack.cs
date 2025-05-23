using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float meleeSpeed;
    [SerializeField] public float damage;
    [SerializeField] public SpriteRenderer srWeapon;

    private Player_Controller controller;
    [SerializeField] private float animationTime;
    private Collider2D hitbox; 
    private float originalY;



    private float timeUntilMelee;

    private void Update()
    {
       
        // Desativa a hitbox quando a arma n�o est� atacando
        if (controller == null || transform.parent != controller.coldre)
                return;

        UpdatePosition();

        if (timeUntilMelee <= 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                timeUntilMelee = meleeSpeed;
                hitbox.enabled = true;
            }
        }
        else
        {
            timeUntilMelee -= Time.deltaTime;
        }
    }

    

    private void Start()
    {
        controller = FindObjectOfType<Player_Controller>();
        originalY = transform.localScale.y;
        hitbox = GetComponent<Collider2D>();
        
    }

    private void Attack()
    {
        if (controller.mov.x > 0 || controller.StopWalkDir)
        {
            anim.SetTrigger("Attack");
            anim.ResetTrigger("AttackEsq");
        }
        else if (controller.mov.x < 0 || controller.StopWalkEsq)
        {
            anim.SetTrigger("AttackEsq");
            anim.ResetTrigger("Attack");
        }

        StartCoroutine(EnableHitbox());
    }

    IEnumerator EnableHitbox()
    {
        if (hitbox)
        {
            hitbox.enabled = true;
            Debug.Log("Hitbox ativada!");
            yield return new WaitForSeconds(animationTime);
            hitbox.enabled = false;
            Debug.Log("Hitbox desativada!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            MeleeAttack sword = controller.coldre.GetComponentInChildren<MeleeAttack>();
            if (sword != null)
            {
                Debug.Log("Espada detectada. Dano: " + sword.damage);
                collision.gameObject.GetComponent<Enemy_controller>().TakeDamage(sword.damage);
            }
            else
            {
                Debug.Log("Erro: Espada n�o encontrada no coldre!");
            }
        }
        if (collision.gameObject.CompareTag("RangedEnemy"))
        {
            MeleeAttack sword = controller.coldre.GetComponentInChildren<MeleeAttack>();
            if (sword != null)
            {
                Debug.Log("Espada detectada. Dano: " + sword.damage);
                collision.gameObject.GetComponent<RangedEnemy>().TakeDamage(sword.damage);
            }
            else
            {
                Debug.Log("Erro: Espada n�o encontrada no coldre!");
            }
        }
        if(collision.gameObject.CompareTag("Box"))
        {

            collision.gameObject.GetComponent<BoxControll>().SwordCollider();
            
        }
    }


    void UpdatePosition()
    {
        Vector3 scale = transform.localScale;

        if (controller.mov.x < 0 || controller.WalkEsq)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else if (controller.mov.x > 0 || controller.WalkDir)
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;

    }


}
