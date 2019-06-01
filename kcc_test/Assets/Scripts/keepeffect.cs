using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepeffect : MonoBehaviour {

    public static keepeffect instance = null; 
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
		
	}
}
