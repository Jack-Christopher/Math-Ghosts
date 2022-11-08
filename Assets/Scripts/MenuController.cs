using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // when play button is clicked, load the game scene
    public void PlayGame()
    {
        SceneManager.LoadScene("FPS");
    }

    // when quit button is clicked, quit the game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    // when back button is clicked, load the main menu scene
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // when options button is clicked, load the options scene
    public void GoToOptions()
    {
        SceneManager.LoadScene("Options");
    }
}
