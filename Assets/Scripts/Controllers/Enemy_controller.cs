using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_controller : MonoBehaviour
{
    //===VARI�VEIS DO ENEMY CONTROLLER===//
    public float moveSpeed = 2f;
    Rigidbody2D rig;
    public Transform target;
    Vector2 moveDirection;
    public Detection_controller detectionArea;
    public SistemaArma sistema;
    public MeleeAttack melee;


    [SerializeField] GameObject Coin;

    [SerializeField]
    public float HP;

    [SerializeField]
    public float damage;

    private void Awake()
    {
        //atribui um componente de f�sica a uma vari�vel 
        rig = GetComponent<Rigidbody2D>();
        sistema = GetComponent<SistemaArma>();
        melee = GetComponent<MeleeAttack>();
    }

    void Start()
    {
        //a vari�vel target recebe o valor do game object com a tag player 
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        if (target)
        {
            //armazena a dire��o horizontal e vertical do movimento em um vetor 2d
            moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }

    private void FixedUpdate()
    {
        //caso a quantidade de objetos dentro da �rea de detec��o seja maior que 0 
        if (detectionArea.detectedObjs.Count > 0)
        {
            //a dire��o do movimento do inimigo � at� o player 
            moveDirection = (detectionArea.detectedObjs[0].transform.position - transform.position).normalized;

            rig.MovePosition(rig.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }

    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
        Instantiate(Coin, transform.position, Quaternion.identity);
    }

    

}
