using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class PlayCanvasScript : MonoBehaviour {

    public Canvas looseCanvas;

    private void Start()
    {
        looseCanvas.enabled = false;
    }

    public void resetPress()
    {
        SceneManager.LoadScene("PlayScrean");
    }

    public void stop()
    {
        looseCanvas.enabled = true;
    }

    public void playAgainPress()
    {
        SceneManager.LoadScene("PlayScrean");
        if (PlayerPrefs.GetInt("Ads") == 3)
        {
            Debug.Log("ads");
            PlayerPrefs.SetInt("Ads", 0);
            Advertisement.Show();
        }
        else
        {
            Debug.Log("nie");
            PlayerPrefs.SetInt("Ads", PlayerPrefs.GetInt("Ads") + 1);
        }
    }

    public void backToMAinMenuPress()
    {
        SceneManager.LoadScene("MainMenu");
        if (PlayerPrefs.GetInt("Ads") == 3)
        {
            Debug.Log("ads");
            PlayerPrefs.SetInt("Ads", 0);
            Advertisement.Show();
        }
        else
        {
            Debug.Log("nie");
            PlayerPrefs.SetInt("Ads", PlayerPrefs.GetInt("Ads") + 1);
        }
    }
}
