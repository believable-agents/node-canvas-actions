using NodeCanvas;
using UnityEngine;

//[ScriptName("Toggle Pushing")]
//[ScriptCategory("Uruk/Movement")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("★ User")]
public class TogglePushing : ActionTask<NavMeshAgent>{
	
	public float radius = 0f;

	protected override string info{
		get {return "Toggle pushing: " + radius;}
	}
	
	protected override void OnExecute(){		
		((NavMeshAgent)agent).radius = radius; 
		EndAction();
	}
}

