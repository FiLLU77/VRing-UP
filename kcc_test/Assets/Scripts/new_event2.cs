using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class new_event2 : MonoBehaviour {

    public static new_event2 instance = null; 

    public GameObject player = null;
    public GameObject myo = null;
    public GameObject canvas = null;
    public GameObject AI = null;
    public GameObject effect = null;
    //public GameObject systemthis = null;
    public static int start = 0;
    public static int end = 1;
    public static int lv =1;

	// Use this for initialization
	void Start () {
		
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
	
	// Update is called once per frame
	void Update () {

        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            lv = ScoreManager.level;
        }

        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            canvas.SetActive(false);;
        }
	}
}
