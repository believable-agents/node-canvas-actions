using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Actions
{
	//[ScriptName("Mecanim Move To Position")]
	[Category("★ Uruk")]
	public class MecanimMoveToPosition : MecanimMoveTo
	{
		[RequiredField]
		public BBParameter<Vector3> target;
		
		[System.NonSerialized]
		Vector3 position;
		
		protected override string info{
			get {return "GoTo " + target.ToString();}
		}
		
		protected override Vector3 Target {
			get {
				return target.value;
			}
		}
		
		protected override void OnExecute(){
			
			if (target.value == null){
				Debug.LogError("Target location is not set correctly on Move To Position Action", agent.gameObject);
				EndAction(false);
				return;
			}
			
			base.OnExecute ();
		}
	}
}

