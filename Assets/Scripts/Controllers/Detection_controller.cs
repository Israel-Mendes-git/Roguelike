using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection_controller : MonoBehaviour
{
    public string TagTarget = "Player";
    public List<Collider2D> detectedObjs = new List<Collider2D>();

    //caso entre na �rea de colis�o 
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //e a tag seja igual a tag do alvo (Player)
        if (collision.gameObject.CompareTag(TagTarget))
        {
            //adiciona na lista o colisor do objeto
            detectedObjs.Add(collision);
            Debug.Log($"Trigger Enter: {collision.name}");
        }
    }

    //caso saia da �rea de colis�o
    private void OnTriggerExit2D(Collider2D collision)
    {
        //e a tag for igual a da tag alvo (Player)
        if (collision.gameObject.CompareTag(TagTarget))
        {
            //remove o colisor do objeto da lista
            detectedObjs.Remove(collision);
            Debug.Log($"Trigger Exit: {collision.name}");
        }

    }
}
