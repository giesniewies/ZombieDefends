using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // Loads the game scene
    public void LoadGame()
    {
        SceneManager.LoadScene("MainGame"); // Change to your actual game scene name
    }

    // Loads the main menu scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Change to your actual main menu scene name
    }

    // Quits the game
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
