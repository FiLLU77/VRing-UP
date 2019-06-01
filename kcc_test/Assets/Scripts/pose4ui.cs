using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
public class pose4ui : MonoBehaviour {

    private Quaternion _antiYaw = Quaternion.identity;
    private float _referenceRoll = 0.0f;

    //
    public Text score = null;

    //1~
    public GameObject myo = null;
    public Image image = null;
    public Image fist = null;
    public Image wave = null;

    public int flag = 0;
    public int flag2 = 0;
    public int flag22 = 0;
    public int flag3 = 0;
    public static int lv = 0;
    //public GameObject imageprefab;

    public Text timeLabel = null;
    public Text mode = null;
    public float timeCount = 30;

    public static main_GUI instance = null;

    [SerializeField] private GameObject _androidMyo;
    [SerializeField] private GameObject _normalMyo;

    private void Start()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
                 myo = _androidMyo;
                /*_androidMyo.GetComponent<MtbMyo>().OnPose -= GUI_OnPose;
                _androidMyo.GetComponent<MtbMyo>().OnPose += GUI_OnPose;*/
        #else
                myo = _normalMyo;
        #endif
    }

    void Awake()
    {
        /*
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        Application.targetFrameRate = 60;
        */
    }

    // Update is called once per frame
    void Update()
    {
        //Thalmic.Myo.Pose pose = myo.GetComponent<ThalmicMyo>().pose;
        //ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        #if UNITY_EDITOR || !UNITY_ANDROID
                NormalMyoPose();
#elif UNITY_ANDROID && !UNITY_EDITOR
                _androidMyo.GetComponent<MtbMyo>().OnPose -= GUI_OnPose;
                _androidMyo.GetComponent<MtbMyo>().OnPose += GUI_OnPose;
#endif

        // Current zero roll vector and roll value.
        /*  Vector3 zeroRoll = computeZeroRollVector(myo.transform.forward);
          float roll = rollFromZero(zeroRoll, myo.transform.forward, myo.transform.up);
          float relativeRoll = normalizeAngle(roll - _referenceRoll);
          Quaternion antiRoll = Quaternion.AngleAxis(relativeRoll, myo.transform.forward);
          transform.rotation = _antiYaw * antiRoll * Quaternion.LookRotation(myo.transform.forward);*/

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (flag == 0)
            {
                image.gameObject.SetActive(true);
                flag++;
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
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            //timeLabel.SetActive(true);
            timeCount -= Time.deltaTime;
            // if (Mathf.Approximately(timeCount,Mathf.Epsilon))
            if (timeCount < Mathf.Epsilon)
            {
                flag++;
                new_event2.lv = ScoreManager.score;
                SceneManager.LoadScene("main");
                timeCount = 180;
            }
            timeLabel.text = string.Format("{0:N0}", timeCount);

            if (flag3 == 0)
            {
                image.gameObject.SetActive(false);
                wave.gameObject.SetActive(false);
                Invoke("Fist", .5f);
                flag3++;
            }
        }
    }

    //imageprefab.SetActive(true);

    private Thalmic.Myo.Pose _normalMyoPose;
    private void NormalMyoPose()
    {

        Thalmic.Myo.Pose pose = _normalMyo.GetComponent<ThalmicMyo>().pose;
        if (pose != _normalMyoPose)
        {
            GUI_OnPose(pose);
            _normalMyoPose = pose;
        }

        // Access the ThalmicMyo component attached to the Myo game object.
    }

    private void GUI_OnPose(Thalmic.Myo.Pose pose)
    {
        //ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();


        if (pose == Pose.FingersSpread)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                image.gameObject.SetActive(false);
                flag = 0;
                Invoke("Fist", .5f);
            }
        }
        if (pose == Pose.Fist)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1&&flag == 0)
            {
                fist.gameObject.SetActive(false);
                flag++;
                flag22 = 0;
                //Invoke("Wave",5f); 
            }
            if (SceneManager.GetActiveScene().buildIndex == 2 && flag2 > 0)
            {
                fist.gameObject.SetActive(false);
            }
            if (SceneManager.GetActiveScene().buildIndex == 3 && flag3 > 0)
            {
                fist.gameObject.SetActive(false);
            }
        }
        if (pose == Pose.WaveIn)
        {
            if (SceneManager.GetActiveScene().buildIndex == 2 && flag2 == 0 && flag22 > 0)
            {
                wave.gameObject.SetActive(false);
                flag2++;
                Invoke("Fist", .5f);
            }
        }
        /*
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (flag == 0)
            {
                image.gameObject.SetActive(true);
                flag++;
            }

            if (pose == Pose.FingersSpread)
            {
                image.gameObject.SetActive(false);
                flag = 0;
                Invoke("Fist", .5f);
            }

            if (pose == Pose.Fist && flag == 0)
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

            if (pose == Pose.WaveIn && flag2 == 0 && flag22 > 0)
            {
                wave.gameObject.SetActive(false);
                flag2++;
                Invoke("Fist", .5f);
            }

            if (pose == Pose.Fist && flag2 > 0)
            {
                fist.gameObject.SetActive(false);
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            //timeLabel.SetActive(true);
            timeCount -= Time.deltaTime;
            // if (Mathf.Approximately(timeCount,Mathf.Epsilon))
            if (timeCount < Mathf.Epsilon)
            {
                flag++;
                new_event2.lv = ScoreManager.score;
                SceneManager.LoadScene("main");
                timeCount = 180;
            }
            timeLabel.text = string.Format("{0:N0}", timeCount);

            if (flag3 == 0)
            {
                image.gameObject.SetActive(false);
                wave.gameObject.SetActive(false);
                Invoke("Fist", .5f);
                flag3++;
            }
            if (pose == Pose.Fist && flag3 > 0)
            {
                fist.gameObject.SetActive(false);
            }
        }
*/
    }

    void Fist()
    {
        fist.gameObject.SetActive(true);
    }

    void Wave()
    {
        wave.gameObject.SetActive(true);
    }
}
