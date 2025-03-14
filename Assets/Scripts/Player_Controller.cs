using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float DashSpeed;
    float speedAtual;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    public float HP;

    [SerializeField]
    public Enemy_controller enemy;

    [SerializeField]
    float cooldownDash;

    bool isDash;

    Vector2 mov;

    private void Start()
    {
        speedAtual = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (speedAtual == speed)
        {
            mov.x = Input.GetAxisRaw("Horizontal");
            mov.y = Input.GetAxisRaw("Vertical");
        }

        mov.Normalize();

        if (Input.GetKeyDown(KeyCode.LeftShift) && mov != Vector2.zero && isDash == false)
        {
            isDash = true;
            speedAtual = DashSpeed;
            Invoke("PosDash", 0.1f);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + mov * speedAtual * Time.deltaTime);
    }

    void PosDash()
    {
        speedAtual = speed;
        Invoke("EndDash", cooldownDash);
    }

    void EndDash()
    {
        isDash = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
        {
            HP -= enemy.damage;
            if (HP <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
