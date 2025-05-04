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
    [SerializeField] private AudioManager audioManager;

    private bool playingFootsteps = false;
    public float footstepSpeed = 0.5f;

    private bool isPaused;

    public ItemPickUp itemPickUp;
    public Transform coldre;
    public Transform coldreSecund�rio;

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
    public GameObject options;
    [SerializeField] private GameObject controls;


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

        // Verifica se h� uma arma no coldre secund�rio ao iniciar o jogo
        if (coldreSecund�rio.childCount <= 0)
        {
            coldreSecund�rio.gameObject.SetActive(false);
        }
        Vector2Int spawnPos = FindObjectOfType<RoomFirstDungeonGenerator>().playerSpawnPosition;
        transform.position = new Vector3(spawnPos.x + 0.5f, spawnPos.y + 0.5f, 0);
        if (MaxEnergy == 0 || MaxEnergy < Energy)
        {
            MaxEnergy = Energy;
        }

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
        // Se o menu de op��es ou controles estiver ativo, n�o faz nada
        if (options.activeSelf || controls.activeSelf)
        {
            Debug.Log("N�o pode sair do pause com menus adicionais abertos!");
            return;
        }

        if (isPaused)
        {
            SoundEffectManager.Play("unPause");
            isPaused = false;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            AudioListener.pause = false;
        }
        else
        {
            SoundEffectManager.Play("Pause");
            isPaused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            AudioListener.pause = true;
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
    void SetPauseButtonsInteractable(bool interactable)
    {
        Button[] buttons = pausePanel.GetComponentsInChildren<Button>(true);
        foreach (Button btn in buttons)
        {
            if (btn.name != "BackBtn")
            {
                btn.interactable = interactable;
            }
        }
    }


    public void BackToMenu()
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(cena);
    }

    public void GoToOptions()
    {
        SoundEffectManager.Play("Button");
        Debug.Log("Abrindo menu de op��es");

        options.SetActive(true);

        CanvasGroup group = options.GetComponent<CanvasGroup>();
        if (group != null)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
            Debug.Log("CanvasGroup configurado");
        }

        SetPauseButtonsInteractable(false);
    }
    public void GoToControls()
    {
        SoundEffectManager.Play("Button");

        controls.SetActive(true);

        CanvasGroup group = controls.GetComponent<CanvasGroup>();
        if (group != null)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
            Debug.Log("CanvasGroup configurado");
        }

        SetPauseButtonsInteractable(false);
    }

    public void BackToPauseScreen()
    {
        SoundEffectManager.Play("Button");
        options.SetActive(false);
        SetPauseButtonsInteractable(true);
    }
    public void BackToPauseScreenInControls()
    {
        Debug.Log("Bot�o voltar no menu de controles clicado.");
        if (controls == null || pausePanel == null)
        {
            Debug.LogError("Painel de controles ou de pause n�o est� atribu�do no Inspector.");
            return;
        }

        SoundEffectManager.Play("Button");

        controls.SetActive(false);
        pausePanel.SetActive(true);
        isPaused = true;

        SetPauseButtonsInteractable(true);
    }



    public void Restart()
    {
        
        SceneManager.LoadScene(playAgain);
    }

    public void BackToGame()
    {
        SoundEffectManager.Play("Button");
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        AudioListener.pause = false; 
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + mov * speedAtual * Time.deltaTime);

        if (!isPaused && !isDead)
        {
            if (mov.magnitude > 0.1f && !playingFootsteps)
            {
                StartFootsteps();
            }
            else if (mov.magnitude <= 0.1f && playingFootsteps)
            {
                StopFootsteps();
            }
        }

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
            SoundEffectManager.Play("Hit");
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
                EnemyShoot bullet = other.gameObject.GetComponent<EnemyShoot>(); // Obt�m o script da bala

                if (bullet != null)
                {
                    HP -= bullet.damage; // Aplica o dano correto
                    Debug.Log("Player tomou " + bullet.damage + " de dano! HP atual: " + HP);
                }
                if (HP <= 0)
                {
                    StartCoroutine(Die());
                }
                else
                {
                    Debug.LogError("A bala n�o tem o script EnemyShoot!");
                }

                Destroy(other.gameObject); // Destroi a bala ap�s o impacto
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
        if (mov.magnitude > 0.1f) // Est� se movendo em qualquer dire��o
        {
            anim.SetBool("WalkDir", mov.x > 0);
            anim.SetBool("WalkEsq", mov.x < 0);
            anim.SetBool("StopWalkDir", false);
            anim.SetBool("StopWalkEsq", false);

            WalkDir = mov.x > 0;
            WalkEsq = mov.x < 0;

            if (!playingFootsteps)
            {
                StartFootsteps();
            }
        }
        else // Parado
        {
            StopFootsteps();

            anim.SetBool("WalkDir", false);
            anim.SetBool("WalkEsq", false);

            if (WalkDir)
            {
                anim.SetBool("StopWalkDir", true);
                anim.SetBool("StopWalkEsq", false);
            }
            else if (WalkEsq)
            {
                anim.SetBool("StopWalkDir", false);
                anim.SetBool("StopWalkEsq", true);
            }

            WalkDir = false;
            WalkEsq = false;
        }
    }

    void TrocarArma()
    {
        if (coldre.childCount > 0 && coldreSecund�rio.childCount > 0) // Verifica se h� duas armas equipadas
        {
            Transform armaPrincipal = coldre.GetChild(0); // Arma no coldre principal
            Transform armaSecundaria = coldreSecund�rio.GetChild(0); // Arma no coldre secund�rio

            // Troca as armas de coldre
            armaPrincipal.SetParent(coldreSecund�rio);
            armaSecundaria.SetParent(coldre);

            // Ajusta as posi��es e rota��es para ficarem centralizadas no novo coldre
            armaPrincipal.localPosition = Vector3.zero;
            armaPrincipal.localRotation = Quaternion.identity;

            armaSecundaria.localPosition = Vector3.zero;
            armaSecundaria.localRotation = Quaternion.identity;

            // Ativa somente a arma no coldre principal e desativa a do secund�rio
            armaPrincipal.gameObject.SetActive(false);
            armaSecundaria.gameObject.SetActive(true);
        }
    }

    void StartFootsteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootstep), 0f, footstepSpeed);
        Debug.Log("Start Footsteps");
    }

    void StopFootsteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootstep));
        Debug.Log("Footsteps parou");
    }

    void PlayFootstep()
    {
        SoundEffectManager.Play("Run");
        Debug.Log("o efeito aconteceu");
    }

}
