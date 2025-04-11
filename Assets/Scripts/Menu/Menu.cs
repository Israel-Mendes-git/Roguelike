using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Animator anim;
    public Canvas transition;
    public Canvas canvas;

    private void Start()
    {
        anim = transition.GetComponent<Animator>();
    }

    public void Transition(string cena)
    {
        StartCoroutine(LoadScene(cena));
    }

    //carrega uma cena específica
    IEnumerator LoadScene(string cena)
    {
        anim.SetTrigger("ButtonPressed");
        yield return new WaitForSeconds(2f);
        Destroy(canvas);


        //carrega a cena 
        SceneManager.LoadScene(cena);
        
    }

    //função para sair do jogo
    public void QuitGame()
    {
        //sai da aplicação 
        Application.Quit();
    }


}
