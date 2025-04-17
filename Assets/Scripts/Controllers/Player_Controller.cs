using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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

    private bool isPaused;

    public ItemPickUp itemPickUp;
    public Transform coldre;
    public Transform coldreSecundário;

    public int Coin;

    [SerializeField] Rigidbody2D rb;
    public int HP;
    public int HPMax;
    [SerializeField] public int Energy;
    public int MaxEnergy;
    [SerializeField] float cooldownDash;

    public bool isDead;
    bool isDash;
    public Vector2 mov;



    [Header("Paineis e Menus")]
    public GameObject pausePanel;
    public string cena;
    public GameObject deadPanel;
    public string playAgain;



    public int EnemyPoints { get; private set; }
    public int RangedEnemyPoints { get; private set; }

    public void AddEnemyKill(bool isRanged)
    {
        if (isRanged)
            RangedEnemyPoints++;
        else
            EnemyPoints++;
    }


    private void Start()
    {
        Time.timeScale = 1f;
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
        if (!isPaused && !isDead)
        {
            Mov();
            Dash();
            SwitchWeapon();
            UpdateAnimator();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen();
        }
        DeadScreen();

        
    }
    void PauseScreen()
    {
        if(isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            
        }
    }

    void DeadScreen()
    {
        if(isDead)
        {
            deadPanel.SetActive(true);
            Time.timeScale = 0f;
            DeadManager deadManager = FindObjectOfType<DeadManager>();
            if (deadManager != null)
            {
                deadManager.ShowScore(EnemyPoints, RangedEnemyPoints);
            }
        }
    }

    void Mov()
    {
        if (speedAtual == speed)
        {
            mov.x = Input.GetAxisRaw("Horizontal");
            mov.y = Input.GetAxisRaw("Vertical");
        }

        mov.Normalize();

    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && mov != Vector2.zero && isDash == false)
        {
            isDash = true;
            speedAtual = DashSpeed;
            Invoke("PosDash", 0.1f);
        }
    }

    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TrocarArma();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(cena);
    }

    public void Restart()
    {
        
        SceneManager.LoadScene(playAgain);
    }

    public void BackToGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);

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
        if (other.gameObject.CompareTag("Enemy"))
        {
            int dano = other.gameObject.GetComponent<Enemy_controller>().damage;
            HP -= dano;
            if (HP <= 0)
            {
                StartCoroutine(Die());
            }
        }
        {
            if (other.gameObject.CompareTag("EnemyBullet"))
            {
                EnemyShoot bullet = other.gameObject.GetComponent<EnemyShoot>(); // Obtém o script da bala

                if (bullet != null)
                {
                    HP -= bullet.damage; // Aplica o dano correto
                    Debug.Log("Player tomou " + bullet.damage + " de dano! HP atual: " + HP);
                }
                else
                {
                    Debug.LogError("A bala não tem o script EnemyShoot!");
                }

                Destroy(other.gameObject); // Destroi a bala após o impacto
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
