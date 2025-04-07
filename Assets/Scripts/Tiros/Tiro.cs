using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiro : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float tempoDeVida;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    private void FixedUpdate()
    {
        transform.Translate(transform.up * speed * Time.fixedDeltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || (other.gameObject.CompareTag("Box")) || (other.gameObject.CompareTag("RangedEnemy")))
        {
            Destroy(gameObject);
        }
    }
}
