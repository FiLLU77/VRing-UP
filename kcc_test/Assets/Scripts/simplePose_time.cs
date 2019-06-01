using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
public class simplePose_time : MonoBehaviour {

    //1~
    public Image image = null;
    public Image fist = null;
    public Image wave = null;

    private int flag = 0;
    private int flag2 = 0;
    private int flag22 = 0;
    private int flag3 = 0;
    public static int lv = 0;
    //public GameObject imageprefab;

    public Text timeLabel = null;
    public Text mode = null;
    public float timeCount = 30;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (flag == 0)
            {
                image.gameObject.SetActive(true);
                Invoke("offPalm", 1.2f);
                Invoke("onFist", 1.8f);
                Invoke("offFist", 3f);
                flag++;
            }
           
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (flag2 == 0)
            {
                image.gameObject.SetActive(false);
                fist.gameObject.SetActive(false);
                wave.gameObject.SetActive(true);
                Invoke("offWave", 1.2f);
                Invoke("onFist", 1.8f);
                Invoke("offFist", 3f);
                flag2++;
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            //timeLabel.SetActive(true);
            timeCount -= Time.deltaTime;
            // if (Mathf.Approximately(timeCount,Mathf.Epsilon))
            if (timeCount < Mathf.Epsilon)
            {
                flag++;
                new_event2.lv = ScoreManager.score;
                SceneManager.LoadScene("main");
                timeCount = 60;
            }
            timeLabel.text = string.Format("{0:N0}", timeCount);
            if (flag3 == 0)
            {
                image.gameObject.SetActive(false);
                fist.gameObject.SetActive(false);
                Invoke("onFist", .5f);
                Invoke("offFist", 2f);
                Invoke("onFist", 3f);
                Invoke("offFist", 4.2f);
                flag3++;
            }
        }
	}
	void onFist()
    {
        fist.gameObject.SetActive(true);
    }

    void onWave()
    {
        wave.gameObject.SetActive(true);
    }

	void onPalm()
    {
        image.gameObject.SetActive(true);
    }

    void offPalm()
    {
        image.gameObject.SetActive(false);
    }

	void offFist()
    {
		fist.gameObject.SetActive(false);
    }

    void offWave()
    {
		wave.gameObject.SetActive(false);
    }
}
