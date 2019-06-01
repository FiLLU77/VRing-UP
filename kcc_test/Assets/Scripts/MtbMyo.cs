using UnityEngine;
using System.Collections;

using Pose = Thalmic.Myo.Pose;

public class MtbMyo : MonoBehaviour 
{
	[SerializeField] private string _desiredMacAddress = "";

	private string _macAddress = "";
	private Pose _pose;

	public string desiredMacAddress {
		get{ return _desiredMacAddress; }
	}

	public string macAddress{
		get{ return _macAddress; }
		set{ _macAddress = value; }
	}

	public Pose pose{
		get{ return _pose; }
		set{ 
			if (value != _pose) {
				_pose = value; 
				if (OnPose != null)
					OnPose (_pose);
			}
		}
	}

	public delegate void OnPoseEventHandler (Pose pose);
	public event OnPoseEventHandler OnPose;


	void Start()
	{
		if (_desiredMacAddress != "" && !MyoAndroidBridge.Instance.startScanActivityOnStart) {
			StartCoroutine (tryAttachWithMacAddress());
		}
	}


	private IEnumerator tryAttachWithMacAddress()
	{
		while (true) {
			Debug.Log ("tyring to connect to: " + _desiredMacAddress);
			MyoAndroidBridge.Instance.ConnectMyoWithAddress (_desiredMacAddress);

			if (macAddress != "")
				yield break;

			yield return new WaitForSeconds (1.0f);
		}
	}


	public void InvokeConnectEvent()
	{
		Debug.Log ("CONNECTED!");
	}

	public void InvokeDisconnectEvent()
	{
		Debug.Log ("DISCONNECTED!");
	}
		
}
