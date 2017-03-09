using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public abstract class NavigatorBase : MonoBehaviour {

	#region Fields
	private const float MinimumDistancetoTurn = 10f;
	protected Animator animator;
	public float MovementSpeed = 0.5f;
	#endregion

	#region Abstract Methods
	public abstract bool IsDestinationNavigable(Vector3 destination);
	public abstract bool SetDestination (Vector3 destination);
	public abstract void Pause ();
	public abstract void Resume ();
	public abstract bool DestinationReached { get; }
	public abstract float DistanceToTarget { get; }
	#endregion

	public void Start()
	{
		this.animator = GetComponent<Animator> ();
		if (this.animator == null) {
			Debug.LogError("Navigation requires animator");
		}
	}

	public IEnumerator Navigate(Vector3 destination) {
		var remainingDistance = Mathf.Infinity;

		// if destination is too little we rotate towards target first
		if (Vector3.Distance (transform.position, destination) < MinimumDistancetoTurn) {
			yield return StartCoroutine (AffineUtility.RotateTowards (transform, destination));
		}
		
		// set the new destination and resume the navigation
		if (!SetDestination (destination)) {
			yield break;
		}
		
		// now wait till we reach the destination
		while (!DestinationReached) 
		{
			// sometimes, due to the animation we may pass the target instead of reaching it
			// as a result agent then spins around the final target
			// therefore we turn agent towards target in case it passes it
			if (remainingDistance < DistanceToTarget) {
				// turn towards object
				Pause ();
				yield return StartCoroutine (AffineUtility.RotateTowards (transform, destination));
				Resume ();
				
			} 
			remainingDistance = DistanceToTarget;
			yield return null;
		}
		yield break;
	} 
}
