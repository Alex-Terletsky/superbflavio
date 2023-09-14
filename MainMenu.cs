using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour //Class to control the main menu
{
    public void PlayGame() //Changes the scene to the game if they press the appropriate button
    {
        SceneManager.LoadScene("Game1");
    }

    public void QuitGame() //Quits the game if they press the appropriate button
    {
        Application.Quit();
    }

}
