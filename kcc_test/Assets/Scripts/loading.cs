using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class loading : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Invoke("next", 5f);
    }

    void next()
    {
        SceneManager.LoadScene("game_test");
       // SceneManager.LoadScene("scene_test1");
    }
}
