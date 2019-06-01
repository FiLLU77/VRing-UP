using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GazeInteraction : MonoBehaviour
{
    public Text text = null;
    public static int start = 0;
    public int manager = 0;
    public float gazeTime = 3f;
    private float timer;
    private bool gazedAt;


    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.HasKey("flag") == true)
        {
            start = PlayerPrefs.GetInt("flag", 0);
            //PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (gazedAt)
        {
            timer += Time.deltaTime;

            if (timer >= gazeTime)
            {
                // execute pointerdown handler
                ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
                timer = 0f;
            }
            if (SceneManager.GetActiveScene().buildIndex == 0&&start ==1)
            {
                text.GetComponent<Text>().text = "게임을 진행해주세요";
            }
            /*
            if(manager==1){
                SceneManager.LoadScene("test1");
            }
            else if(manager==2){
                SceneManager.LoadScene("game_test");
            }
            */
        }
    }

    public void PointerEnter()
    {
        gazedAt = true;
        Debug.Log("PointerEnter");
    }

    public void PointerExit()
    {
        gazedAt = false;
        Debug.Log("PointerExit");
    }

    public void PointerDown()
    {
        Debug.Log("PointerDown");
    }


    public void OnClickTest()
    {
        new_event2.start++;
        PlayerPrefs.SetInt("flag", start);
        gazedAt = true;
        SceneManager.LoadScene("test1");
    }
    public void OnClickGame()
    {
        // PlayerPrefs.GetInt("flag");
        //SceneManager.LoadScene("game_test");
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (new_event2.start == 1)
            {
                //         manager = 2;
                gazedAt = true;
                new_event2.end++;
                SceneManager.LoadScene("game_test");
                PlayerPrefs.DeleteAll();
            }
            else
            {
                text.GetComponent<Text>().text = "측정하기를 먼저 진행하세요";
            }
        }
    }

    public void OnClickResult()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (new_event2.end == 1)
            {
                //         manager = 2;
                gazedAt = true;
                SceneManager.LoadScene("result");
            }
            else
            {
                text.GetComponent<Text>().text = "게임을 먼저 진행하세요";
            }
        }
    }

    public void OnClickBack()
    {
        /*start++;
        PlayerPrefs.SetInt("flag", start);
        */gazedAt = true;
        SceneManager.LoadScene("main");
    }

}