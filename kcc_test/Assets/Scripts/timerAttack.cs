using UnityEngine;
using System.Collections;

public class timerAttack : MonoBehaviour
{
    private float done = 10.0F;
    public GUIText gui_text;
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (done > 0F)
        {
            done -= Time.deltaTime;
            gui_text.text = "Time : " + done + "sec";
        }
        else
        {
            print("Time is Over");
        }
    }
}