﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvasScript : MonoBehaviour {

    public GameObject infoObject;
    public Canvas pickCanvas;

    public Button vsPlayer;
    public Button vsComp;
    public Button exit;

    private void Start()
    {
        pickCanvas.enabled = false;
        vsPlayer = vsPlayer.GetComponent<Button>();
        vsComp = vsComp.GetComponent<Button>();
        exit = exit.GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pickCanvas.enabled == true)
            {
                vsPlayer.enabled = true;
                vsComp.enabled = true;
                exit.enabled = true;
                pickCanvas.enabled = false;
            }
            else
            {
                Application.Quit();
            }
        }
    }


    public void playerTextPress()
    {
        infoObject.GetComponent<InfoObjectScript>().isAi = false;
        SceneManager.LoadScene("PlayScrean");
    }

    public void computerTexPress()
    {
        infoObject.GetComponent<InfoObjectScript>().isAi = true;
        vsPlayer.enabled = false;
        vsComp.enabled = false;
        exit.enabled = false;
        pickCanvas.enabled = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void pick1Press()
    {
        infoObject.GetComponent<InfoObjectScript>().dificultyLevel = 0;
        SceneManager.LoadScene("PlayScrean");
    }

    public void pick2Press()
    {
        infoObject.GetComponent<InfoObjectScript>().dificultyLevel = 1;
        SceneManager.LoadScene("PlayScrean");
    }

    public void pick3Press()
    {
        infoObject.GetComponent<InfoObjectScript>().dificultyLevel = 2;
        infoObject.GetComponent<InfoObjectScript>().weightCase = 0;
        SceneManager.LoadScene("PlayScrean");
    }

    public void pick4Press()
    {
        infoObject.GetComponent<InfoObjectScript>().dificultyLevel = 2;
        infoObject.GetComponent<InfoObjectScript>().weightCase = 1;
        SceneManager.LoadScene("PlayScrean");
    }

    public void backPress()
    {
        vsPlayer.enabled = true;
        vsComp.enabled = true;
        exit.enabled = true;
        pickCanvas.enabled = false;
    }
}
