using NodeCanvas;


//[ScriptName("Toggle Pushing")]
//[ScriptCategory("Uruk/Movement")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("★ User")]
public class TogglePushing : ActionTask<UnityEngine.AI.NavMeshAgent>{
	
	public float radius = 0f;

	protected override string info{
		get {return "Toggle pushing: " + radius;}
	}
	
	protected override void OnExecute(){		
		((UnityEngine.AI.NavMeshAgent)agent).radius = radius; 
		EndAction();
	}
}

