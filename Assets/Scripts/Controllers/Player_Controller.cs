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
    private Animator anim;
    public bool WalkDir;
    public bool WalkEsq;
    public bool StopWalkDir;
    public bool StopWalkEsq;


    public ItemPickUp itemPickUp;
    public Transform coldre;
    public Transform coldreSecundário;

    public int Coin;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] public float HP;
    public float HPMax;
    [SerializeField] public int Energy;
    public int MaxEnergy;
    [SerializeField] public Enemy_controller enemy;
    [SerializeField] float cooldownDash;

    public bool isDead;
    bool isDash;
    public Vector2 mov;

    private void Start()
    {
        speedAtual = speed;
        anim = GetComponent<Animator>();

        // Verifica se há uma arma no coldre secundário ao iniciar o jogo
        if (coldreSecundário.childCount <= 0)
        {
            coldreSecundário.gameObject.SetActive(false);
        }
        Vector2Int spawnPos = FindObjectOfType<RoomFirstDungeonGenerator>().playerSpawnPosition;
        transform.position = new Vector3(spawnPos.x + 0.5f, spawnPos.y + 0.5f, 0);
        MaxEnergy = Energy;

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            TrocarArma();
        }

        UpdateAnimator();
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

    void UpdateAnimator()
    {
        if (mov.x > 0) // Movendo para a direita
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
        else if (mov.x < 0) // Movendo para a esquerda
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
    void TrocarArma()
    {
        if (coldre.childCount > 0 && coldreSecundário.childCount > 0) // Verifica se há duas armas equipadas
        {
            Transform armaPrincipal = coldre.GetChild(0); // Arma no coldre principal
            Transform armaSecundaria = coldreSecundário.GetChild(0); // Arma no coldre secundário

            // Troca as armas de coldre
            armaPrincipal.SetParent(coldreSecundário);
            armaSecundaria.SetParent(coldre);

            // Ajusta as posições e rotações para ficarem centralizadas no novo coldre
            armaPrincipal.localPosition = Vector3.zero;
            armaPrincipal.localRotation = Quaternion.identity;

            armaSecundaria.localPosition = Vector3.zero;
            armaSecundaria.localRotation = Quaternion.identity;

            // Ativa somente a arma no coldre principal e desativa a do secundário
            armaPrincipal.gameObject.SetActive(false);
            armaSecundaria.gameObject.SetActive(true);
        }
    }

}
