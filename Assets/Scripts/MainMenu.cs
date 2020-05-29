using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame ()
    {

        SceneManager.LoadScene("floor0_Composite");

    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
        
}
