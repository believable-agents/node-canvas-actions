using UnityEngine;
using System.Collections;
using NodeCanvas;
using NodeCanvas.BehaviourTrees;


//[ScriptName("Throw Object")]
//[ScriptCategory("Uruk")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("★ User")]
public class ThrowObject : ActionTask<Transform> {

	public BBParameter<GameObject> throwableObject;
	public string throwFrom = "RightHand";
	
	private Transform referenceObject;
	public Transform parentTransform;

	Collider parentCollider;

	protected override string OnInit ()
	{
		referenceObject = agent.transform.Search (throwFrom);
		if (referenceObject == null) {
			Debug.LogError("Reference object not found: " + throwFrom);
		}

		parentTransform = referenceObject.transform;
		while (parentTransform.parent != null) {
			parentTransform = parentTransform.parent;
			if (parentTransform.GetComponent<BehaviourTreeOwner>() != null) break;
		}

		return null;
	}

	// throws object from the reference object
	protected override void OnExecute() {
		GameObject projectile = Object.Instantiate(throwableObject.value,
		                             referenceObject.position,
		                             referenceObject.rotation) as GameObject;
		Object.Destroy (projectile, 3f);
		if (projectile.GetComponent<Rigidbody>() == null) {
			Debug.Log("No rigid body");
		}

		var distance = Vector3.Distance (parentTransform.position, parentTransform.PlayerPosition());
		projectile.GetComponent<Rigidbody>().AddForce(parentTransform.forward * UnityEngine.Random.Range(0.7f * distance * 50, 1.3f * distance * 50));//cannon's x axis

		if (parentCollider != null) { 
			// Physics.IgnoreCollision (projectile.collider, parentCollider);
		}

		EndAction (true);
	}
}
