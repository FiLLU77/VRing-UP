using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Pose = Thalmic.Myo.Pose;

public class ui : MonoBehaviour {

    public GameObject myo = null;
    public Image image;
    public Image fist;
    public Image wave;
    public Image more;
    public int flag = 0;
    public int flag2 = 0;
    public int flag22 = 0;
    public int flag3 = 0;
    public static int lv = 0;
    //public GameObject imageprefab;

    public Text timeLabel;
    public Text mode;
    public float timeCount = 30;

    // Use this for initialization
	void Start () {
        timeLabel.GetComponent<Text>().text = " ";
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            timeCount = 180;
        }
	}

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
        //imageprefab.SetActive(true);

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (thalmicMyo.pose == Pose.FingersSpread)
            {
                image.gameObject.SetActive(false);
                flag = 0;
                Invoke("Fist", .5f);
            }

            if (thalmicMyo.pose == Pose.Fist && flag == 0)
            {
                fist.gameObject.SetActive(false);
                flag++;
                flag22 = 0;
                //Invoke("Wave",5f); 
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (flag22 == 0)
            {
                flag2 = 0;
                image.gameObject.SetActive(false);
                fist.gameObject.SetActive(false);
                wave.gameObject.SetActive(true);
                flag22++;
            }

            if (thalmicMyo.pose == Pose.WaveIn && flag2 == 0 &&flag22>0)
            {
                wave.gameObject.SetActive(false);
                flag2++;
                Invoke("Fist",.5f); 
            }

            if (thalmicMyo.pose == Pose.Fist&&flag2>0)
            {
                fist.gameObject.SetActive(false);
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            //timeLabel.SetActive(true);
            timeCount -= Time.deltaTime;
           // if (Mathf.Approximately(timeCount,Mathf.Epsilon))
            if (timeCount< Mathf.Epsilon)
            {
                flag++;
                new_event2.lv = ScoreManager.score;
                SceneManager.LoadScene("main");
                timeCount = 180;
            }
            timeLabel.text = string.Format("{0:N0}",timeCount);

            if (flag3 == 0)
            {
                image.gameObject.SetActive(false);
                wave.gameObject.SetActive(false);
                Invoke("Fist", .5f);
                flag3++;
            }
            if (thalmicMyo.pose == Pose.Fist&&flag3>0)
            {
                fist.gameObject.SetActive(false);
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 0 && flag3 != 0)
        {
           
        }
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            mode.gameObject.SetActive(true);
            if (ScoreManager.level < 5)
            { mode.text = "EASY"; }
            else if(ScoreManager.level >=5 &&ScoreManager.level < 10)
            {
                mode.text = "NORMAL";
            }
            else if (ScoreManager.level >= 10)
            {
                mode.text = "HARD";
            }
                //timeLabel.SetActive(true);
                timeCount -= Time.deltaTime;
                // if (Mathf.Approximately(timeCount,Mathf.Epsilon))
                if (timeCount < Mathf.Epsilon)
                {
                    SceneManager.LoadScene("main");
                }
                timeLabel.text = string.Format("{0:N0}", timeCount);
        }
	}

    void Fist(){
        fist.gameObject.SetActive(true);
    }

    void Wave()
    {
        wave.gameObject.SetActive(true);
    }
}
