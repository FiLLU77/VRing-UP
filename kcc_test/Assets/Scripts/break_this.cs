using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class break_this : MonoBehaviour {
    
    public GameObject systemthis = null;
	// Use this for initialization
	void Start () {
        EventSystem sceneEventSystem = FindObjectOfType<EventSystem>();
        if (sceneEventSystem == null)
        {
            GameObject eventSystem = new GameObject("EventSystem_main");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            //systemthis.SetActive(false);
            Destroy(systemthis);
        }
	}
}
