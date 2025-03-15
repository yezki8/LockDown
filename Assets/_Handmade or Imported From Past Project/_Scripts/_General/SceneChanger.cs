using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public void Exitgame()
    {
        Debug.Log("exitgame");
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
