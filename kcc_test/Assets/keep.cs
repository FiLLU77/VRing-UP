using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keep : MonoBehaviour
{

    public static keep instance = null;
    // Use this for initialization
    void Start()
    {

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
    void Update()
    {
        this.gameObject.SetActive(true);
    }
}
