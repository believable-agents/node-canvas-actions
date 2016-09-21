using UnityEngine;
using System.Collections;
using NodeCanvas;

//[ScriptName("Throw At Object")]
//[ScriptCategory("Uruk")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("★ User")]
public class ThrowAtObject : ThrowObject {
	
	public BBParameter<GameObject> throwAt;
	private Transform throwAtPosition;

	protected override string OnInit ()
	{
		// by default we throw stuff at player
		if (throwAt.value == null) {
			throwAt.value = GameObject.FindGameObjectWithTag("Player");
		}
		throwAtPosition = throwAt.value.transform;		

		base.OnInit ();
			
		return null;
	}
	
	// throws object from the reference object
	protected override void OnExecute() {
		// rotate towards player + some offset infornt of him
		var position = throwAtPosition.position + throwAtPosition.forward * 2;
		Debug.Log (position);
		agent.transform.LookAt (new Vector3(position.x, agent.transform.position.y, position.z));

		base.OnExecute ();
	}
}
