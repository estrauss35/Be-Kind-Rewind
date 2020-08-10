using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameController gameController;

    public Button nextButton;
    public Button mainMenuButton;

    public GameObject rewinder;
    public GameObject goodBin;

    private GameObject[] texts;

    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        texts = GameObject.FindGameObjectsWithTag("TutorialText");
        foreach(GameObject text in texts)
        {
            text.SetActive(false);
        }
        texts[0].SetActive(true);
        nextButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextText()
    {
        texts[index].SetActive(false);
        index++;
        if (index <= texts.Length - 1)
        {
            texts[index].SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(false);
            mainMenuButton.gameObject.SetActive(true);
        }
        if(index == 2)
        {
            gameController.SpawnNewBox();
        }
        if (index == 6)
        {
            rewinder.SetActive(true);
        }
        if (index == 7)
        {
            goodBin.SetActive(true);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
