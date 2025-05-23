using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiro : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float tempoDeVida;
    public bool isPistol;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Destroy(gameObject, tempoDeVida);
        if (isPistol)
        {
            anim.Play("Bullet Animation");
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(transform.up * speed * Time.fixedDeltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || (other.gameObject.CompareTag("Boss") || (other.gameObject.CompareTag("Box")) || (other.gameObject.CompareTag("RangedEnemy")) || (other.gameObject.CompareTag("Walls"))))
        {
            Destroy(gameObject);
        }
    }
}
