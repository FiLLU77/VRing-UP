using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class new_event : MonoBehaviour {

    public GameObject player = null;
    public GameObject myo = null;
    public GameObject canvas = null;
   // public GameObject AI = null;

	// Use this for initialization
	void Start () {
       
	}

	void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 3)
        {
          
            Destroy(player);
            Destroy(myo);
            Destroy(canvas);
        }/*
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            AI.SetActive(true);
        }*/
    }
}
