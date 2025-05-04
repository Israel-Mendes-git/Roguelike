using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private Animator anim;

    [Header("Referências de Objetos")]
    public Canvas transition;               // Canvas da transição
    public GameObject mainMenuCanvas;       // Menu principal
    [SerializeField] private GameObject optionsMenu; // Menu de opções
    [SerializeField] private GameObject controlsMenu;

    private void Start()
    {
        anim = transition.GetComponent<Animator>();

    }

    public void Transition(string cena)
    {
        Debug.Log("a transição foi ativada");
        StartCoroutine(LoadScene(cena));
    }

    IEnumerator LoadScene(string cena)
    {
        Debug.Log("Trigger será chamado");
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
