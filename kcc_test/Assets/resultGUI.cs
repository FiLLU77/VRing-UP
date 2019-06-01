using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resultGUI : MonoBehaviour {
    
    public Text attText = null;
    public Text wavText = null;
    public Text misText = null;

	// Use this for initialization
	void Start () {
        attText.GetComponent<Text>().text = "획득 점수: "+ ScoreManager.score;
        wavText.GetComponent<Text>().text = "팔 굽힌 횟수: " + (Box4android.result_win+Box4android.result_wout);
        misText.GetComponent<Text>().text = "못 맞춘 호랑이: " + (ScoreManager.score - Box4android.result_fist);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
