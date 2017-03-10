using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;

namespace NodeCanvas.Actions{
	public abstract class MecanimMoveTo : ActionTask<UnityEngine.AI.NavMeshAgent>{

//		public BBFloat speed = new BBFloat();

		private const float slowDownDistance = 3f;


		private Vector3 lastRequestedPosition;
		private Animator animator;
		private float currentSpeed;
		private float remainingDistance;
		private bool rotatingTowardsTarget;
		private float speed;
		public bool forcePosition;

        private bool finished;

		//for faster access
		private UnityEngine.AI.NavMeshAgent navAgent{
			get {return (UnityEngine.AI.NavMeshAgent)agent; }
		}

		protected abstract Vector3 Target { get; }

		protected virtual Vector3 LookAt {
			get { return Target + Vector3.forward; }
		}
		
		protected override string info{
			get { return "GoTo " + Target.ToString();}
		}
		
		protected override string OnInit ()
		{
			this.animator = agent.GetComponent<Animator> ();
			this.speed = agent.GetComponent<UnityEngine.AI.NavMeshAgent>().speed;

			if (this.animator == null) {
				Debug.LogError("To do mecanim move, agent has to have the Animator component");
			}
			return null;
		}
		
		protected override void OnExecute(){
			this.currentSpeed = 0;
//			Debug.Log ("Set speed to: " + this.currentSpeed);
			
			if ( (navAgent.transform.position - Target).magnitude < navAgent.stoppingDistance){
				animator.SetFloat("Speed", 0f);
//				Debug.Log("Already there");
				EndAction(true);
				return;
			}
			
			if (!navAgent.SetDestination (Target)) {
				animator.SetFloat("Speed", 0f);
//				Debug.Log("Bad destination 1");
				EndAction (false);
			}

			remainingDistance = float.MaxValue;
		}
		
		protected override void OnUpdate(){
			if (finished || rotatingTowardsTarget)
				return;

			// set speed
			if (!navAgent.pathPending && this.currentSpeed != this.speed)
			{
				animator.SetFloat("Speed", this.speed);
				this.currentSpeed = this.speed;
			}

			if (lastRequestedPosition != Target){
				
				if (!navAgent.SetDestination(Target)){
					animator.SetFloat("Speed", 0f);
//					Debug.Log("Bad destination");
					EndAction(false);
					return;
				}				
				lastRequestedPosition = Target;
			}
			
			if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance){
				animator.SetFloat("Speed", 0f);
//				Debug.Log("Done")

				// we may request that position and rotation is forced on player
				if (forcePosition) {
					// TODO: Tween movement
					navAgent.transform.position = Target;
					StartCoroutine(RotateTowardsTarget(LookAt, true));
				} else {
                    if (Success())
                    {
                        EndAction(true);
                    }
				}
                finished = true;
				return;
			}
			
			// adjust speed and slow down when reaching target
			if (!navAgent.pathPending && navAgent.remainingDistance < slowDownDistance) {
				this.currentSpeed = Mathf.Clamp (navAgent.remainingDistance / slowDownDistance, 0.0f, this.currentSpeed);
				this.animator.SetFloat ("Speed", this.currentSpeed);
			}

			// check if we have passed the object
			if (!navAgent.pathPending && navAgent.remainingDistance < slowDownDistance && remainingDistance < navAgent.remainingDistance) {
				// turn towards object
				navAgent.Stop ();
				this.animator.SetFloat ("Speed", 0f);
				StartCoroutine(RotateTowardsTarget(Target));				
			} 
			remainingDistance = navAgent.remainingDistance;

            //Debug.Log(this.currentSpeed);
		}

		IEnumerator RotateTowardsTarget(Vector3 target, bool finish = false) {
			rotatingTowardsTarget = true;
//			Debug.Log("Rotating towrds target");
			yield return StartCoroutine (AffineUtility.RotateTowards (agent.transform, target, 1f));
//			Debug.Log("Resume");
			navAgent.Resume ();

			// after rotation we may finish
			if (finish) {
				Debug.Log("Shall finish");
                if (Success())
                {
                    EndAction(true);
                }
			}

			rotatingTowardsTarget = false;
		}
		
		protected override void OnStop(){
			lastRequestedPosition = Vector3.zero;
			if (navAgent.gameObject.activeSelf) {
				animator.SetFloat("Speed", 0f);
				navAgent.ResetPath ();
			}
		}

		protected virtual bool Success() {
            return true;
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
