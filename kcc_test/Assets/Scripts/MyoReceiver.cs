using UnityEngine;
using System.Collections;

public class MyoReceiver : MonoBehaviour 
{

	[SerializeField] private GameObject _androidMyo;
	[SerializeField] private GameObject _normalMyo;

	private GameObject _myoRef;

	private Quaternion _antiYaw = Quaternion.identity;
	private float _referenceRoll = 0.0f;


	// Use this for initialization
	void Start () {
        //안드로이드이며 에디터 아닐(run일)
		#if UNITY_ANDROID && !UNITY_EDITOR
		_myoRef = _androidMyo;
		_androidMyo.GetComponent<MtbMyo>().OnPose -= Myo_OnPose;
		_androidMyo.GetComponent<MtbMyo>().OnPose += Myo_OnPose;
		#else
		_myoRef = _normalMyo;
		#endif
	}

	
	// Update is called once per frame
	void Update () 
	{
		#if UNITY_EDITOR || !UNITY_ANDROID
		CheckNormalMyoPose();
		#endif

		// Current zero roll vector and roll value.
		Vector3 zeroRoll = computeZeroRollVector (_myoRef.transform.forward);
		float roll = rollFromZero (zeroRoll, _myoRef.transform.forward, _myoRef.transform.up);
		float relativeRoll = normalizeAngle (roll - _referenceRoll);
		Quaternion antiRoll = Quaternion.AngleAxis (relativeRoll, _myoRef.transform.forward);
		transform.rotation = _antiYaw * antiRoll * Quaternion.LookRotation (_myoRef.transform.forward);
	}


	private void Myo_OnPose (Thalmic.Myo.Pose pose)
	{
        //_myoRef.GetComponent<ThalmicMyo>().Vibrate(Thalmic.Myo.VibrationType.Long);
		if (pose == Thalmic.Myo.Pose.FingersSpread) {
			// Reset Myo Pose
			_antiYaw = Quaternion.FromToRotation (
				new Vector3 (_myoRef.transform.forward.x, 0, _myoRef.transform.forward.z),
				new Vector3 (0, 0, 1)
			);

			Vector3 referenceZeroRoll = computeZeroRollVector (_myoRef.transform.forward);
			_referenceRoll = rollFromZero (referenceZeroRoll, _myoRef.transform.forward, _myoRef.transform.up);
		}
	}


	/// <summary>
	/// Helper method for normal myo
	/// </summary>
	private Thalmic.Myo.Pose _normalMyoPose;
	private void CheckNormalMyoPose()
	{
		Thalmic.Myo.Pose pose = _normalMyo.GetComponent<ThalmicMyo> ().pose;
		if (pose != _normalMyoPose) {
			Myo_OnPose (pose);
			_normalMyoPose = pose;
		}
	}


	private float rollFromZero (Vector3 zeroRoll, Vector3 forward, Vector3 up)
	{
		float cosine = Vector3.Dot (up, zeroRoll);
		Vector3 cp = Vector3.Cross (up, zeroRoll);
		float directionCosine = Vector3.Dot (forward, cp);
		float sign = directionCosine < 0.0f ? 1.0f : -1.0f;

		return sign * Mathf.Rad2Deg * Mathf.Acos (cosine);
	}

	private Vector3 computeZeroRollVector (Vector3 forward)
	{
		Vector3 antigravity = Vector3.up;
		Vector3 m = Vector3.Cross (_myoRef.transform.forward, antigravity);
		Vector3 roll = Vector3.Cross (m, _myoRef.transform.forward);

		return roll.normalized;
	}

	private float normalizeAngle (float angle)
	{
		if (angle > 180.0f) {
			return angle - 360.0f;
		}
		if (angle < -180.0f) {
			return angle + 360.0f;
		}
		return angle;
	}
}
