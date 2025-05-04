using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private Animator anim;

    [Header("Refer�ncias de Objetos")]
    public Canvas transition;               // Canvas da transi��o
    public GameObject mainMenuCanvas;       // Menu principal
    [SerializeField] private GameObject optionsMenu; // Menu de op��es
    [SerializeField] private GameObject controlsMenu;

    private void Start()
    {
        anim = transition.GetComponent<Animator>();

    }

    public void Transition(string cena)
    {
        Debug.Log("a transi��o foi ativada");
        StartCoroutine(LoadScene(cena));
    }

    IEnumerator LoadScene(string cena)
    {
        Debug.Log("Trigger ser� chamado");
        anim.SetTrigger("ButtonPressed");
        Debug.Log("Trigger chamado, esperando 2 segundos");
        yield return new WaitForSeconds(2f);
        Debug.Log("Tempo esperado, carregando cena: " + cena);
        SceneManager.LoadScene(cena);
        
    }


    public void EnableOptions()
    {
        optionsMenu.SetActive(true);
    }
    public void EnableControls()
    {
        controlsMenu.SetActive(true);
    }
    public void DisableControls()
    {
        controlsMenu.SetActive(false);
    }
    public void DisableOptions()
    {
        optionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
