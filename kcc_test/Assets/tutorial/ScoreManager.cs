using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {
    public static ScoreManager SMi;
    public Text scoreText;
    public static int level=0;
    public static int score = 0;
    private int check = 0;

    public static ScoreManager instance = null; 

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;


        scoreText = GameObject.Find("Score").GetComponent<Text>();
        if (!SMi)
            SMi = this;
    }

    public void AddScore(int num)
    {
        score += num;
        scoreText.text = "Score : " + score;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       // if (SceneManager.GetActiveScene().buildIndex == 4&&check==0)
        {
            level = score;
            score = 0;
            check++;
        }
	}
}
