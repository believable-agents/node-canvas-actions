using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NavMeshNavigator : NavigatorBase {

	UnityEngine.AI.NavMeshAgent navigation;
	UnityEngine.AI.NavMeshPath path;

	public void Awake() 
	{
		navigation = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		if (navigation == null) {
			Debug.LogError ("NavMeshAgent is required!");
		}
	}

	#region implemented abstract members of NavigatorBase

	public override bool IsDestinationNavigable (Vector3 destination)
	{
//		Debug.Log (destination + ":" + navigation);
		UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath ();
		navigation.CalculatePath (destination, path);
	
		return path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete;
	}

	public override bool SetDestination (Vector3 destination)
	{
		UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath ();
		navigation.CalculatePath (destination, path);
		
		if (path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete) {
			navigation.path = path;
			animator.SetFloat ("Speed", MovementSpeed);
			return true;
		}
		return false;
	}

	public override void Pause ()
	{
		navigation.Stop ();
		animator.SetFloat ("Speed", 0);
	}

	public override void Resume ()
	{
		navigation.Resume ();
		animator.SetFloat ("Speed", MovementSpeed);
	}

	public override bool DestinationReached {
		get {
			return navigation.remainingDistance < float.Epsilon;
		}
	}

	public override float DistanceToTarget {
		get {
			return navigation.remainingDistance;
		}
	}

	#endregion



}
