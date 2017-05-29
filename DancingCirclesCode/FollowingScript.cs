using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingScript : MonoBehaviour {

    public bool isBlack;
    public bool hasFollower;
    public float timeWait;
    public GameObject follower = null;

    private Color normalColor;
    private List<Vector2> positionList;
    private SpriteRenderer sr;

    private void Start()
    {
        isBlack = false;
        sr = GetComponent<SpriteRenderer>();
        normalColor = sr.color;
        positionList = new List<Vector2>();
    }

    private void Update()
    {
        if (hasFollower)
        {
            positionList.Add(transform.position);

            if (Time.timeSinceLevelLoad > timeWait)
            {
                follower.transform.position = positionList[0];
                positionList.RemoveAt(0);
            }
        }
    }

    public void turnOffColor()
    {
        sr.color = Color.black;
        sr.sortingOrder = 0;
        isBlack = true;
    }

    public void turnOnColor()
    {
        sr.color = normalColor;
        sr.sortingOrder = 2;
        isBlack = false;
    }
}
