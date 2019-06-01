using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class main_GUI : MonoBehaviour {

    //0
    public Text text = null;
    public static int start = 0;
    public Image btn1 = null;
    public Image btn2 = null;
    public Image btn3 = null;

    //
    public Text score = null;

    //1~
    public static int lv = 0;
    //public GameObject imageprefab;

    public Text timeLabel = null;
    public Text mode = null;
    public float timeCount;

    public static main_GUI instance = null; 


    private void Start()
    {
        if(PlayerPrefs.HasKey("flag")==true)
        {
            start=PlayerPrefs.GetInt("flag", 0);
            //PlayerPrefs.Save();
        }
        /*
        if (start == 0)
        {
            text.GetComponent<Text>().text = "측정하기를 진행해주세요";
        }
        else if (start >= 1)
        {
            text.GetComponent<Text>().text = "게임을 진행하세요 -" + new_event2.lv;
        }
*/
        timeLabel.GetComponent<Text>().text = " ";
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            timeCount = 30;
        }
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            timeCount = 180;
        }
        /*
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            text.gameObject.SetActive(true);
            btn1.gameObject.SetActive(true);
            btn2.gameObject.SetActive(true);
            btn3.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
            btn1.gameObject.SetActive(false);
            btn2.gameObject.SetActive(false);
            btn3.gameObject.SetActive(false);
            score.gameObject.SetActive(true);
        }*/
    }

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
    }

/*    public void OnClickTest (){
        start++;//안
        PlayerPrefs.SetInt("flag", start);

        SceneManager.LoadScene("test1");
    }
    public void OnClickGame()
    {
        //SceneManager.LoadScene("game_test");
        if (start > 0)
        {
            SceneManager.LoadScene("game_test");
            PlayerPrefs.DeleteAll();
        }
        else{
            text.GetComponent<Text>().text = "측정하기를 먼저 진행하세요";
        }
    }
*/
    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            text.gameObject.SetActive(true);
            btn1.gameObject.SetActive(true);
            btn2.gameObject.SetActive(true);
            btn3.gameObject.SetActive(true);
        }
        else{
            text.gameObject.SetActive(false);
            btn1.gameObject.SetActive(false);
            btn2.gameObject.SetActive(false);
            btn3.gameObject.SetActive(false);
        }

        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            mode.gameObject.SetActive(true);
            if (ScoreManager.level < 6)
            { mode.text = "EASY"; }
            else if (ScoreManager.level >= 6 && ScoreManager.level < 12)
            {
                mode.text = "NORMAL";
            }
            else if (ScoreManager.level >= 12)
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

}