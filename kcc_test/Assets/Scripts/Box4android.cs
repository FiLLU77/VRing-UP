using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

// Change the material when certain poses are made with the Myo armband.
// Change code Based on ColorBoxByPose and MyoReceiver
// Made By Kim Jong Keon in ROK
public class Box4android : MonoBehaviour {
    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;
    public GameObject hub;

    // Materials to change to when poses are made.
    public Material waveInMaterial;
    public Material waveOutMaterial;

    public GameObject flamethrow;
    public GameObject hitbox;
    public GameObject bullet;

    public bool flameON = true;
    public bool gunON = false;
    private bool started = false;

    [SerializeField] private GameObject _androidMyo;
    [SerializeField] private GameObject _normalMyo;

    private Quaternion _antiYaw = Quaternion.identity;
    private float _referenceRoll = 0.0f;

    private Pose _lastPose = Pose.Unknown;
    // Use this for initialization

    public static int result_fist = 0;
    public static int result_win = 0;
    public static int result_wout = 0;

    void Start()
    {
        GetComponent<Renderer>().material = waveOutMaterial;
        //안드로이드이며 에디터 아닐(run일)
    #if UNITY_ANDROID && !UNITY_EDITOR
                myo = _androidMyo;
                _androidMyo.GetComponent<MtbMyo>().OnPose -= Myo_OnPose;
                _androidMyo.GetComponent<MtbMyo>().OnPose += Myo_OnPose;
    #else
            myo = _normalMyo;
    #endif
    }


    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR || !UNITY_ANDROID
                NormalMyoPose();
        #endif

        // Current zero roll vector and roll value.
        Vector3 zeroRoll = computeZeroRollVector(myo.transform.forward);
        float roll = rollFromZero(zeroRoll, myo.transform.forward, myo.transform.up);
        float relativeRoll = normalizeAngle(roll - _referenceRoll);
        Quaternion antiRoll = Quaternion.AngleAxis(relativeRoll, myo.transform.forward);
        transform.rotation = _antiYaw * antiRoll * Quaternion.LookRotation(myo.transform.forward);
    }


    private void Myo_OnPose(Thalmic.Myo.Pose pose)
    {
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        flamethrow.SetActive(false);
        hitbox.SetActive(false);

        if (pose == Pose.Fist)
            {
                //thalmicMyo.Vibrate(VibrationType.Medium);
                if (flameON)
                {
                    flamethrow.SetActive(true);
                    hitbox.SetActive(true);
                }
                else if (gunON)
                {
                    //instantiate new bullet here
                    GameObject newBullet = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation) as GameObject;
                    newBullet.SetActive(true);
                    newBullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 2000);
                    Physics.IgnoreCollision(transform.root.GetComponent<Collider>(), newBullet.GetComponent<Collider>(), true);
                    Destroy(newBullet, 1.0f);
                }

                ExtendUnlockAndNotifyUserAction(thalmicMyo);

                // Change material when wave in, wave out or double tap poses are made.
            }
        else if (pose == Pose.WaveIn)
            {
                GetComponent<Renderer>().material = waveInMaterial;
                flameON = false;
                gunON = true;

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        else if (pose == Pose.WaveOut)
            {
                GetComponent<Renderer>().material = waveOutMaterial;
                gunON = false;
                flameON = true;

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }

        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            if (pose == Pose.Fist){
                result_fist++;
            }
            else if (pose == Pose.WaveIn){
                result_win++;
            }
            else if (pose == Pose.WaveOut){
                result_wout++;
            }
        }
    }


    /// <summary>
    /// Helper method for normal myo
    /// </summary>
    private Thalmic.Myo.Pose _normalMyoPose;
    private void NormalMyoPose()
    {

        Thalmic.Myo.Pose pose = _normalMyo.GetComponent<ThalmicMyo>().pose;
        if (pose != _normalMyoPose)
        {
            Myo_OnPose(pose);
            _normalMyoPose = pose;
        }

        // Access the ThalmicMyo component attached to the Myo game object.
    }


    private float rollFromZero(Vector3 zeroRoll, Vector3 forward, Vector3 up)
    {
        float cosine = Vector3.Dot(up, zeroRoll);
        Vector3 cp = Vector3.Cross(up, zeroRoll);
        float directionCosine = Vector3.Dot(forward, cp);
        float sign = directionCosine < 0.0f ? 1.0f : -1.0f;

        return sign * Mathf.Rad2Deg * Mathf.Acos(cosine);
    }

    private Vector3 computeZeroRollVector(Vector3 forward)
    {
        Vector3 antigravity = Vector3.up;
        Vector3 m = Vector3.Cross(myo.transform.forward, antigravity);
        Vector3 roll = Vector3.Cross(m, myo.transform.forward);

        return roll.normalized;
    }

    private float normalizeAngle(float angle)
    {
        if (angle > 180.0f)
        {
            return angle - 360.0f;
        }
        if (angle < -180.0f)
        {
            return angle + 360.0f;
        }
        return angle;
    }

    void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard)
        {
            myo.Unlock(UnlockType.Timed);
        }

        myo.NotifyUserAction();
    }
}