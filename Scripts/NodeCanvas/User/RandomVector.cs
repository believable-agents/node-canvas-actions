using UnityEngine;
using System.Collections;
using NodeCanvas;

//[ScriptName("Random 3D Vector")]
//[ScriptCategory("Uruk/Interop")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("★ User")]
public class RandomVector : ActionTask<UnityEngine.AI.NavMeshAgent>{

	public float xMin, xMax, yMin, yMax, zMin, zMax;

	public BBParameter<Vector3> vector;

	protected override void OnExecute(){
		vector.value = new Vector3 (
			Random.Range(xMin, xMax),
			Random.Range(yMin, yMax),
			Random.Range(zMin, zMax));
		EndAction();
	}
}
