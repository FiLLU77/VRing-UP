using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class JointOrientation2 : MonoBehaviour
{
    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;
    
    private Quaternion _antiYaw = Quaternion.identity;
    
    private float _referenceRoll = 0.0f;

    private Pose _lastPose = Pose.Unknown;


	public GameObject cam;
	public GameObject player;
	public GameObject joint;
	public GameObject hub;

	private bool started = false;

	private float offsetH = 0;
	private float offsetV = 0;

	void Start () {
		started = false;
		offsetH = 0;
		offsetV = 0;
	}


	// Update is called once per frame.
    void Update ()
    {
        // Access the ThalmicMyo component attached to the Myo object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

        // Update references when the pose becomes fingers spread or the q key is pressed.
        bool updateReference = false;

        if (thalmicMyo.pose != _lastPose) {
            _lastPose = thalmicMyo.pose;

			if ((thalmicMyo.pose == Pose.FingersSpread) && !started) {
                updateReference = true;
				started = true;

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }
        if (Input.GetKeyDown ("r")) {
            updateReference = true;
        }

		if (started) {
			
			float JangleH = -joint.transform.rotation.eulerAngles.y;
			JangleH = normalizeAngle (JangleH);
			float JangleV = joint.transform.rotation.eulerAngles.x;
			JangleV = normalizeAngle (JangleV);
			//Debug.Log ("offsetH");
			//Debug.Log (offsetH);

		}


        // Update references. This anchors the joint on-screen such that it faces forward away
        // from the viewer when the Myo armband is oriented the way it is when these references are taken.
        if (updateReference) {
            // _antiYaw represents a rotation of the Myo armband about the Y axis (up) which aligns the forward
            // vector of the rotation with Z = 1 when the wearer's arm is pointing in the reference direction.

			offsetH = 0;
			offsetV = 0;
            _antiYaw = Quaternion.FromToRotation (
                new Vector3 (myo.transform.forward.x, 0, myo.transform.forward.z),
				new Vector3 (cam.transform.forward.x, 0, cam.transform.forward.z)
            );

       
            Vector3 referenceZeroRoll = computeZeroRollVector (myo.transform.forward);
            _referenceRoll = rollFromZero (referenceZeroRoll, myo.transform.forward, myo.transform.up);
        }

        // Current zero roll vector and roll value.
        Vector3 zeroRoll = computeZeroRollVector (myo.transform.forward);
        float roll = rollFromZero (zeroRoll, myo.transform.forward, myo.transform.up);

        // The relative roll is simply how much the current roll has changed relative to the reference roll.
        // adjustAngle simply keeps the resultant value within -180 to 180 degrees.
        float relativeRoll = normalizeAngle (roll - _referenceRoll);

        // antiRoll represents a rotation about the myo Armband's forward axis adjusting for reference roll.
        Quaternion antiRoll = Quaternion.AngleAxis (relativeRoll, myo.transform.forward);

        // Here the anti-roll and yaw rotations are applied to the myo Armband's forward direction to yield
        // the orientation of the joint.
        transform.rotation = _antiYaw * antiRoll * Quaternion.LookRotation (myo.transform.forward);

        // The above calculations were done assuming the Myo armbands's +x direction, in its own coordinate system,
        // was facing toward the wearer's elbow. If the Myo armband is worn with its +x direction facing the other way,
        // the rotation needs to be updated to compensate.
        if (thalmicMyo.xDirection == Thalmic.Myo.XDirection.TowardWrist) {
            // Mirror the rotation around the XZ plane in Unity's coordinate system (XY plane in Myo's coordinate
            // system). This makes the rotation reflect the arm's orientation, rather than that of the Myo armband.
            transform.rotation = new Quaternion(transform.localRotation.x,
                                                transform.localRotation.y,
                                                transform.localRotation.z,
                                                -transform.localRotation.w);
        }

    }

    
    float rollFromZero (Vector3 zeroRoll, Vector3 forward, Vector3 up)
    {
 
        float cosine = Vector3.Dot (up, zeroRoll);
	
        Vector3 cp = Vector3.Cross (up, zeroRoll);
        float directionCosine = Vector3.Dot (forward, cp);
        float sign = directionCosine < 0.0f ? 1.0f : -1.0f;

        // Return the angle of roll (in degrees) from the cosine and the sign.
        return sign * Mathf.Rad2Deg * Mathf.Acos (cosine);
    }

    // Compute a vector that points perpendicular to the forward direction,
    // minimizing angular distance from world up (positive Y axis).
    // This represents the direction of no rotation about its forward axis.
    Vector3 computeZeroRollVector (Vector3 forward)
    {
        Vector3 antigravity = Vector3.up;
        Vector3 m = Vector3.Cross (myo.transform.forward, antigravity);
        Vector3 roll = Vector3.Cross (m, myo.transform.forward);

        return roll.normalized;
    }

    // Adjust the provided angle to be within a -180 to 180.
    float normalizeAngle (float angle)
    {
        if (angle > 180.0f) {
            return angle - 360.0f;
        }
        if (angle < -180.0f) {
            return angle + 360.0f;
        }
        return angle;
    }

    // Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
    // recognized.
    void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard) {
            myo.Unlock (UnlockType.Timed);
        }

        myo.NotifyUserAction ();
    }
}
