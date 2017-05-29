using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class MainMenuCanvasScript : MonoBehaviour {

    public Text bonusPoints;
    public Text highscore;

    private void Start()
    {
        bonusPoints.text = "BONUS POINTS: " + PlayerPrefs.GetInt("BonusPoints");
        highscore.text = "HIGHSCORE: " + PlayerPrefs.GetInt("HighScore");
    }

    public void playPress()
    {
        SceneManager.LoadScene("PlayScrean");
    }

    public void bonusPress()
    {
        const string RewardedPlacementId = "rewardedVideo";

#if UNITY_ADS
        if (!Advertisement.IsReady(RewardedPlacementId))
        {
            Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
            return;
        }

        var options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show(RewardedPlacementId, options);
#endif
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                PlayerPrefs.SetInt("PointBoost", 1);
                break;
            case ShowResult.Skipped:
               // Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
               // Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}
