
using NodeCanvas;
using UnityEngine;
using System.Collections;

//[ScriptName("Create Look At Vector")]
//[ScriptCategory("Uruk/Transform")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;


[Category("★ User")]
public class CreateLookAtVector : ActionTask<Transform> {
	
	public BBParameter<GameObject> lookAt;
	public BBParameter<Vector3> vector; 
	
	protected override void OnExecute() {
		if (lookAt.value == null) {
			Debug.LogError(agent.name + " was unable to look at object. Object not initialised");
			return;
		}

		vector.value = new Vector3 (
			lookAt.value.transform.position.x, 
			agent.transform.position.y, 
			lookAt.value.transform.position.z);
		EndAction (true);
	}

	protected override string info{
		get {return "Get " + lookAt + " position as " + vector;}
	}
}