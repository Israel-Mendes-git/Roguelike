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


    private float timeUntilMelee;

    private void Update()
    {
       
        // Desativa a hitbox quando a arma não está atacando
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

    private Collider2D hitbox; // Adicione isso como variável

    private void Start()
    {
        controller = FindObjectOfType<Player_Controller>();
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
            yield return new WaitForSeconds(0.2f); // Ativa a hitbox por 0.2 segundos
            hitbox.enabled = false;
        }
    }


    void UpdatePosition()
    {
        if (controller.mov.x < 0) // Movendo para a esquerda
        {
            srWeapon.flipX = true;
        }
        else if (controller.mov.x > 0)
        {
            srWeapon.flipX = false;
        }
        else
        {
            if (controller.WalkDir) // Se estava indo para a direita
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
