using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class GameColorBox : MonoBehaviour
{


    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;
    public GameObject hub;

    // Materials to change to when poses are made.
    public Material waveInMaterial;
    public Material waveOutMaterial;
    public Material doubleTapMaterial;

    public GameObject player;
    public GameObject cam;
    public GameObject joint;
    public GameObject flamethrow;
    public GameObject hitbox;
    public GameObject bullet;

    public bool flameON = true;
    public bool gunON = false;
    private bool started = false;


    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    void Start()
    {
        GetComponent<Renderer>().material = waveOutMaterial;
    }

    // Update is called once per frame.
    void Update()
    {
        // Access the ThalmicMyo component attached to the Myo game object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        // Check if the pose has changed since last update.
        // The ThalmicMyo component of a Myo game object has a pose property that is set to the
        // currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
        // detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
        // is not on a user's arm, pose will be set to Pose.Unknown.
        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;
            flamethrow.SetActive(false);
            hitbox.SetActive(false);

            // Vibrate the Myo armband when a fist is made.
            if (thalmicMyo.pose == Pose.Fist)
            {
                thalmicMyo.Vibrate(VibrationType.Medium);
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
                    Destroy(newBullet, 2.0f);
                }

                ExtendUnlockAndNotifyUserAction(thalmicMyo);

                // Change material when wave in, wave out or double tap poses are made.
            }
            else if (thalmicMyo.pose == Pose.WaveIn)
            {
                GetComponent<Renderer>().material = waveInMaterial;
                flameON = false;
                gunON = true;

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.WaveOut)
            {
                GetComponent<Renderer>().material = waveOutMaterial;
                gunON = false;
                flameON = true;

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.DoubleTap)
            {
                //GetComponent<Renderer> ().material = doubleTapMaterial;

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.FingersSpread)
            {
                started = true;

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }

    }

    // Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
    // recognized.
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
