using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pose = Thalmic.Myo.Pose;

/// <summary>
/// Myo android bridge.
/// Static class which handle myo functions from native plugin
/// </summary>
public class MyoAndroidBridge : MonoBehaviour
{
	[SerializeField] private bool _startScanActivityOnStart = false;

	private static string MYO_ANDROID_CLASSNAME = "jp.mtblanc.myoandroidunitylib.MyoBridge";
	private AndroidJavaObject	_unityActivity = null;
	private AndroidJavaClass 	_myoAndroidClass = null;

	private static MyoAndroidBridge instance;
	public static MyoAndroidBridge Instance
	{
		get {
			if (instance == null) {
				instance = (MyoAndroidBridge)FindObjectOfType (typeof(MyoAndroidBridge));
				if (instance == null) {
					Debug.LogError("no game object found with MyoAndroidBridge");
				}
			}
			return instance;
		}
	}


	private MtbMyo[] _myoChildren;
	private Dictionary<string, MtbMyo> _myos = new Dictionary<string, MtbMyo>();

	public bool startScanActivityOnStart{ get { return _startScanActivityOnStart; } }


	void Awake()
	{
		if (this != Instance) {
			Destroy (this);
			Debug.LogError ("This component was destroyed since other object has same behaviour");
			return;
		}

        instance = this;
        DontDestroyOnLoad(this);

		_myoChildren = gameObject.GetComponentsInChildren<MtbMyo> ();

		#if UNITY_ANDROID && !UNITY_EDITOR
		InitMyo (_myoChildren.Length);
		_myoAndroidClass.CallStatic ("SetListener", gameObject.name, "orientationCB", "poseCB", "connectCB", "disconnectCB");
		#endif
	}


	void Start()
	{
		if(_startScanActivityOnStart)
			StartScanActivity ();
	}


	/// <summary>
	/// Initialize Myo.
	/// </summary>
	private bool InitMyo(int myoAttachAllownace)
	{
		if (_unityActivity == null) {
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			_unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"); 
		}

		if (_myoAndroidClass == null) {
			_myoAndroidClass = new AndroidJavaClass(MYO_ANDROID_CLASSNAME);
		}

		bool result = false;
		_unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
			{
				result = _myoAndroidClass.CallStatic<bool>("InitMyo", _unityActivity, false, myoAttachAllownace);
			})
		);

		return result;
	}


	public void ConnectMyoWithAddress(string macAddress)
	{
		if (_myoAndroidClass == null) {
			Debug.LogError ("Init Myo before set listener!!");
			return;
		}

		_myoAndroidClass.CallStatic ("ConnectMyoWithAddress", macAddress);
	}


	public List<string> GetConnectedMyo()
	{
		if (_myoAndroidClass == null) {
			Debug.LogError ("Init Myo before set listener!!");
			return null;
		}

		string result = _myoAndroidClass.CallStatic<string> ("GetConnectedMyo");
		return new List<string> (result.Split (' '));
	}


	/// <summary>
	/// Starts Myo scan activity.
	/// </summary>
	public void StartScanActivity()
	{
		if (_myoAndroidClass == null) {
			Debug.LogError ("Init Myo before start activity!!");
			return;
		}

		_myoAndroidClass.CallStatic ("StartScanActivity", _unityActivity);
	}



	private void orientationCB(string param)
	{
		string[] paramArr = param.Split(' ');

		if (!_myos.ContainsKey (paramArr [0]))
			connectCB (paramArr [0]);

		float qx = float.Parse(paramArr [1]);
		float qy = float.Parse(paramArr [2]);
		float qz = float.Parse(paramArr [3]);
		float qw = float.Parse(paramArr [4]);

		_myos [paramArr [0]].transform.localRotation = new Quaternion (qy, qz, -qx, -qw);
	}


	private void poseCB(string param)
	{
		Debug.Log ("POSE: " + param);

		string[] paramArr = param.Split(' ');

		Pose pose = Pose.Unknown;
		switch (paramArr [1]) {
		case "REST":
			pose = Pose.Rest;
			break;
		case "FIST":
			pose = Pose.Fist;
			break;
		case "WAVE_IN":
			pose = Pose.WaveIn;
			break;
		case "WAVE_OUT":
			pose = Pose.WaveOut;
			break;
		case "FINGERS_SPREAD":
			pose = Pose.FingersSpread;
			break;
		case "DOUBLE_TAP":
			pose = Pose.DoubleTap;
			break;
		default:
			break;
		}

		if (!_myos.ContainsKey (paramArr [0]))
			connectCB (paramArr [0]);

		_myos [paramArr [0]].pose = pose;
	}


	private void connectCB(string param)
	{
		if (_myos.ContainsKey (param)) {
			Debug.Log ("this myo is already connected");
			return;
		}

		foreach (MtbMyo myo in _myoChildren) {
			
			if (myo.macAddress == "") {
				if (myo.desiredMacAddress == "" || myo.desiredMacAddress == param) {
					myo.macAddress = param;
					_myos.Add (param, myo);
					myo.InvokeConnectEvent ();
					Debug.Log ("Myo connected! : " + param + ", @: " + myo.gameObject.name);
					break;
				}
			} 
		}
	}


	private void disconnectCB(string param)
	{
		if (_myos.ContainsKey (param)) {
			_myos [param].macAddress = "";
			_myos [param].InvokeDisconnectEvent ();
			_myos.Remove (param);
			Debug.Log ("Lost Myo : " + param);
		}
	}
}
