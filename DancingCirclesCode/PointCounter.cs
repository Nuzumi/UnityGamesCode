using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour {

    public Text pointText;
    public Text bonusPointText;
    public Text score;
    public Text highScore;
    public Text bonusScore;

    private bool canCount;
    private int point;
    private int bonusPoint;
    private bool pointBoost;

    private void Start()
    {
        canCount = false;
        point = 0;
        bonusPoint = 0;
        if (PlayerPrefs.GetInt("PointBoost") == 1)
        {
            pointBoost = true;
            PlayerPrefs.SetInt("PointBoost", 0);
        }
        else
        {
            pointBoost = false;
        }

        bonusPointText.text = "BONUS: " + bonusPoint;
        pointText.text = "SCORE: " + point;
    }

    private void Update()
    {
        if (!canCount)
        {
            score.text = "SCORE:" + point;
            if (PlayerPrefs.GetInt("HighScore") > point)
            {
                highScore.text = "HIGHSCORE:" + PlayerPrefs.GetInt("HighScore");
            }
            else
            {
                highScore.text = "HIGHSCORE:" + point;
                PlayerPrefs.SetInt("HighScore", point);
            }

            bonusScore.text = "BONUS POINTS:" + PlayerPrefs.GetInt("BonusPoints");
        }
    }

    public void pointUp(int pointPlus)
    {
        if (canCount)
        {
            if (pointBoost)
            {
                point += (pointPlus*2);
            }
            else
            {
                point += pointPlus;
            }
            pointText.text = "SCORE: " + point;
        }
    }

    public void bonusPointUp(int bonusPointPlus)
    {
        if (pointBoost)
        {
            bonusPoint += (bonusPointPlus*2);
            point += bonusPointPlus * 10;
            PlayerPrefs.SetInt("BonusPoints", PlayerPrefs.GetInt("BonusPoints") + 2);
        }
        else
        {
            bonusPoint += bonusPointPlus;
            point += bonusPointPlus * 5;
            PlayerPrefs.SetInt("BonusPoints", PlayerPrefs.GetInt("BonusPoints") + 1);
        }
        bonusPointText.text = "BONUS: " + bonusPoint;
    }

    public void canCountOn()
    {
        canCount = true;
    }

    public void canCountOff()
    {
        canCount = false;
    } 

}
