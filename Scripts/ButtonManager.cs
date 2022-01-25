using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void btn_StartTheGame()
    {
        SceneManager.LoadScene("Level01");
    }

    public void btn_ReturnToMainMenu()
    {
        SceneManager.LoadScene("TitleMenu");
    }

    public void btn_LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void btn_LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void btn_QuitGame()
    {
        Debug.Log("QUITTING: The quit command was received");
        Application.Quit();
    }

}