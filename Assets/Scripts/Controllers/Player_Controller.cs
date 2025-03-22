using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float DashSpeed;
    float speedAtual;
    public int contador = 0;

    public ItemPickUp itemPickUp;

    public List<GameObject> armas = new List<GameObject>();

    public int Coin;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] public float HP;
    public int Energy;
    [SerializeField] public Enemy_controller enemy;
    [SerializeField] float cooldownDash;

    public bool isDead;
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

        AtualizarArmas(); // Mantém a lista sempre atualizada
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
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
        {
            HP -= enemy.damage;
            if (HP <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    // Método para atualizar a lista de armas do jogador
    void AtualizarArmas()
    {
        if (itemPickUp != null)
        {
            armas.Clear(); // Limpa a lista antes de atualizar
            if (itemPickUp.armaPrincipal != null)
                armas.Add(itemPickUp.armaPrincipal);
            if (itemPickUp.armaSecundaria != null)
                armas.Add(itemPickUp.armaSecundaria);
        }
    }
}
