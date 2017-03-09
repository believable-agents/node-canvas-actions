using UnityEngine;
using System.Collections;

public enum SenseType {
	PersonalSpace,
	Vision,
	Hearing,
	Smell
}

public class Sense : MonoBehaviour {

	public delegate IEnumerator SenseEventHandler (Collider col);

	public event System.Action<Collider> Sensed;
	public event SenseEventHandler SensedCoroutine;
	public event System.Action<Collider> SensedPlayer;
	public event SenseEventHandler SensedPlayerCoroutine;
	public event System.Action<Collider> LostPlayer;
	public event SenseEventHandler LostPlayerCoroutine;

	public SenseType SenseType;
	public float Angle;
	[HideInInspector]
	public Transform PlayerDetected;
	[HideInInspector]
	public Transform OtherDetected;


	// Use this for initialization
	void Start () {
		// get all behaviours
		// wdDebug.Log (transform.parent.GetComponent<NavigationBase> ());
	}

	public static void AddToObject(Transform parent, SenseType type, float radius, float angle = 0f) 
	{
		var senseObject = new GameObject (type.ToString());
		senseObject.transform.parent = parent;
		var col = senseObject.AddComponent<SphereCollider> ();
		col.radius = radius;
		col.isTrigger = true;
		var sense = senseObject.AddComponent<Sense> ();
		sense.Angle = angle;
		sense.SenseType = type;
		senseObject.transform.localPosition = Vector3.zero;
		senseObject.transform.localRotation = Quaternion.identity;
	}

	void OnTriggerExit(Collider collision) {
		if (collision.gameObject.tag == "Player") {
			PlayerDetected = null;

			if (LostPlayer != null) {
				LostPlayer (collision);
			}
			if (LostPlayerCoroutine != null) {
				SenseEventHandler func = LostPlayerCoroutine;
				StartCoroutine (func (collision));
			}
		} else {
			if (OtherDetected == collision.transform) {
				OtherDetected = null;
			}
		}
	}

	void OnTriggerEnter(Collider collision) {

		// check viewing angle
		if (Angle > Mathf.Epsilon && Mathf.Abs(Vector3.Angle (transform.forward, (collision.transform.position - transform.forward))) > Angle) {
			return;
		}


//		Debug.Log (SenseName);

		if (collision.gameObject.tag == "Player") {
			PlayerDetected = collision.transform;

			if (SensedPlayer != null) {
				SensedPlayer (collision);
			}
			if (SensedPlayerCoroutine != null) {
				SenseEventHandler func = SensedPlayerCoroutine;
				StartCoroutine (func(collision));
			}
		} else {
			OtherDetected = collision.transform;
			if (Sensed != null) {
				Sensed (collision);
			}
			if (SensedCoroutine != null) {
				SenseEventHandler func = SensedCoroutine;
				StartCoroutine (func (collision));
			} 
		}

//		Debug.DrawRay(transform.position, collision.transform.position - transform.position, Color.white);
//
//		Debug.Log ("Sense triggered");
//
//
//		if (collision.gameObject.tag == "Crowd") {
//			Debug.Log ("Collided with crowd");
//
//			if (Random.value < 0.05) 
//			{
//				Debug.Log ("Starting to chat");
//			}
//		}
//
//		if (collision.gameObject.tag == "Player") {
//			Debug.Log ("Player triggered");
//
//
//			_lookAt = collision.gameObject.transform;
//
//			// transform.LookAt (collision.transform.position);
//
//			if (Random.value < 0.05) 
//			{
//				Debug.Log ("Starting to chat");
//			}
//		}
	}

	// Called by the Unity Editor GUI every update cycle, but only when the object is selected.
	// Draws a sphere showing spawnRange's setting visually.
	void OnDrawGizmosSelected ()
	{
		if (transform == null || GetComponent<SphereCollider> () == null) 
			return;

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (transform.position, GetComponent<SphereCollider>().radius);
	}




}
