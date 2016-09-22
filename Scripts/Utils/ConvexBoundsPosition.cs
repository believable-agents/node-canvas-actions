using UnityEngine;
using System.Collections;

public class ConvexBoundsPosition : MonoBehaviour {

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawSphere (transform.position, 1f);

        // paint all
	    var p = this.transform.parent.GetComponent<ConvexBounds>();
	    if (p != null)
	    {
	        ConvexBounds.DrawGizmos(p);
	    }
	}


}
