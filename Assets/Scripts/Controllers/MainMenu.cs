using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartTutorial()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
