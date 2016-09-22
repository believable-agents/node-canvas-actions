
using NodeCanvas;
using UnityEngine;
using System.Collections;

//[ScriptName("Smooth LookAt 2D")]
//[ScriptCategory("Uruk/Transform")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("★ Uruk")]
public class SmoothLookAt2D : ActionTask<Transform> {

	public BBParameter<GameObject> lookAt;
	public float speed = 2f; 

	protected override void OnExecute() {
		StartCoroutine(SmoothLook());
	}

	IEnumerator SmoothLook() {
		float time = 0;
		var trans = agent.transform;
		var startRotation = agent.transform.rotation;
		var targetPos = lookAt.value.transform.position;

		var targetRotation = Quaternion.LookRotation (new Vector3(targetPos.x, agent.transform.position.y, targetPos.z));
		while (time < 1) {
			trans.rotation = Quaternion.Slerp (startRotation, targetRotation, time);
			time += Time.deltaTime * speed;
			Debug.Log("Rotating");
			yield return null;
		}
		EndAction (true);
		yield return null;
	}
}