using UnityEngine;
using System.Collections;

public class InfoObjectScript : MonoBehaviour {

    public bool isAi;
    public int dificultyLevel;
    public int weightCase;

    void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
}
