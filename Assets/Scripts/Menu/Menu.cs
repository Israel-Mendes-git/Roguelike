using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //carrega uma cena específica
    public void LoadScene(string cena)
    {
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
