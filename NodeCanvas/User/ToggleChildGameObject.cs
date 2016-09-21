using NodeCanvas;
using UnityEngine;

//[ScriptName("Toggle Child")]
//[ScriptCategory("Uruk/GameObject")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("★ User")]
public class ToggleChildGameObject : ToggleGameobject{

	public BBParameter<string> childName;

	protected override string OnInit ()
	{
		var obj = agent.transform.Search (childName.value);
		if (obj == null) {
			Debug.LogError(agent.transform.name + " does not have a child of name: " + childName.ToString());
		}

		target.value = obj.gameObject;
		return null;
	}

	protected override string info{
		get {return "Set Visibility of '" + childName + "' To '" + SetTo + "'";}
	}
}