using NodeCanvas;
using UnityEngine;

//[ScriptName("Toggle GameObject")]
//[ScriptCategory("Uruk/GameObject")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("★ User")]
public class ToggleGameobject : ActionTask<Transform>{
	
	public enum SetMode {Invisible, Visible, Toggle}
	public SetMode SetTo= SetMode.Toggle;
	public BBParameter<GameObject> target;


	protected override string info{
		get {return "Set Visibility of '" + (target.value == null ? "<not set>" : target.value.name) + "' To '" + SetTo + "'";}
	}
	
	protected override void OnExecute(){
		
		bool value;
		
		if (SetTo == SetMode.Toggle){
			
			value = !target.value.activeSelf;
			
		} else {
			
			value = (int)SetTo == 1;
		}
		
		target.value.SetActive(value);
		EndAction();
	}
}