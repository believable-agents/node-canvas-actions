using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Actions{
	
	//[ScriptName("Mecanim Move To GameObject")]
	[Category("★ Uruk")]
	public class MecanimMoveToTarget : MecanimMoveTo{

		[RequiredField]
		public BBParameter<GameObject> target;

		[System.NonSerialized]
		Vector3 position;

		protected override string info{
			get {return "GoTo " + target.ToString();}
		}

		protected override Vector3 Target {
			get {
				return target.value.transform.position;
			}
		}

		protected override void OnExecute(){
			
			if (target.value == null){
				Debug.LogError("Target GameObject location is not set correctly on Move To GameObject Action", agent.gameObject);
				EndAction(false);
				return;
			}

			base.OnExecute ();
		}
	}
}

//using UnityEngine;
//using System.Collections;
//using NodeCanvas;
//using NodeCanvas.Variables;
//
//#if UNITY_EDITOR
////[ScriptName("Mecanim Navigate")]
//#endif
//#if UNITY_EDITOR
////[ScriptCategory("Movement")]
//#endif
//[Task.AgentType(typeof(Transform))]
//public class MecanimMoveToTarget : ActionTask {
//
//	private const float slowDownDistance = 3f;
//
//	#if UNITY_EDITOR
//	[RequiredField]
//#endif
//	public BBVector TargetPosition = new BBVector ();
//
//
//	protected NavMeshAgent navAgent;
//	protected Animator animator;
//
//	private Vector3 targetPosition;
//	private float remainingDistance = Mathf.Infinity;
//	private int retries;
//	private float currentSpeed;
//
//
//
//	protected override string OnInit ()
//	{
//		this.animator = agent.GetComponent<Animator> ();
//		this.navAgent = agent.GetComponent<NavMeshAgent> ();
//
//		return null;
//	}
//
//	protected override void OnExecute(){	
//
//		// remeber the position
//		targetPosition = TargetPosition.value;
//
//		// only asssign path if navmesh was initialized
//		if (this.navAgent != null) {
//			this.AssignPath();
//		}
//		this.currentSpeed = 0.1f;
//	}
//
//	protected override void OnUpdate ()
//	{
//		// agent is first speeding up, then slowing down
//		// when it is slowing down we check if it reached the destination
//
//		// slowing down
//		if (!navAgent.pathPending && navAgent.remainingDistance < slowDownDistance) {
//
//			// if slow down distance is e.g. 2, we need to divide by 2*2 to obtain 0.5, 
//			// which is the normal speed at the beggining of slowing down
//			this.currentSpeed = Mathf.Clamp(navAgent.remainingDistance / (slowDownDistance * 2f), 0.0f, this.currentSpeed);
//			this.animator.SetFloat ("Speed", this.currentSpeed);
//
//			// we stop, when agent has reached 1 meter from the target
//			if (this.currentSpeed < 0.1f || navAgent.remainingDistance <= navAgent.stoppingDistance) {
//				navAgent.Stop();
//				EndAction (true);
//			}
//		}
//		// speeding up
//		else 
//		{
//			this.currentSpeed = Mathf.Clamp(this.currentSpeed + 0.002f, 0.1f, 0.5f);
//			this.animator.SetFloat ("Speed", this.currentSpeed);
//		}
//
//		// maybe we have passed the target, therefore we fail, but we fail only after several attempts
//		if (navAgent.remainingDistance < 1f && this.remainingDistance < navAgent.remainingDistance && retries++ == 5) {
//			Debug.Log("FAILURE: " + this.remainingDistance  + "<" + navAgent.remainingDistance);
//			EndAction (false);
//			navAgent.Stop();
//		}
//
//		// remember the remaining distance to check if we passed the target
//		this.remainingDistance = navAgent.remainingDistance;
//	}
//
//	protected override void OnStop ()
//	{
//		if (navAgent.hasPath) {
//			navAgent.Stop ();
//			navAgent.ResetPath ();
//		}
//	}
//
//	// protected methods
//
//	protected void AssignPath() {
//		// if the position is within the stopping distance, we finish navigation with success
//		if ((this.navAgent.transform.position - targetPosition).magnitude < navAgent.stoppingDistance){
//			EndAction(true);
//			return;
//		}
//		
//		// we try to set the new destination
//		if (!this.navAgent.SetDestination (TargetPosition.value)) {
//			// in case navigation could not be set we finish with failure
//			EndAction (false);
//		} else {
//			retries = 0;
//			this.navAgent.Resume();
//		}
//	}
//
//	#if UNITY_EDITOR
//	protected override string info{
//		get {
//			return "Moving to: " + targetPosition + "\nRemaining: " + remainingDistance;
//		}
//	}
//	#endif
//}
