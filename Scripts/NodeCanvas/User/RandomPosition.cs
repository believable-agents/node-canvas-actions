using UnityEngine;
using System.Collections;
using NodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace ViAgents.NodeCanvas.Actions 
{
	[Name("Random Position")]
	[Category("★ ViAgents")]
	public class RandomPosition : ActionTask<Transform>
	{
		public BBParameter<Vector3> position;

		public float radius = 60f;
	    public BBParameter<ConvexBounds> convexBounds;

        //public ConvexBounds convexBounds;		
		static ConvexBounds bounds;
	    private ConvexBounds currentBounds;
  
//		protected override void OnAwake ()
//		{
//			// base.OnAwake ();
//
//			//if (bounds == null) {
//			//	bounds = GameObject.Find("@PeopleBounds").GetComponent<ConvexBounds>();
//			//	if (bounds == null || bounds.Bounds == null || bounds.Bounds.Length == 0) {
//			//		Debug.LogError("Random walk bounds were not initiated");
//			//	}
//			//}
//
//		 //   if (convexBounds.value == null)
//		 //   {
//		 //       currentBounds = bounds;
//		 //   }
//		 //   else
//		 //   {
//		 //       currentBounds = convexBounds.value.GetComponent<ConvexBounds>();
//		 //   }
//		 //   currentBounds = convexBounds.value.GetComponent<ConvexBounds>();
//		}

		protected override void OnExecute ()
		{
            currentBounds = convexBounds.value;
		    if (currentBounds == null)
		    {
		        Debug.LogError("Bounds not initialised!");
		    }

            try
		    {
		        position.value = this.currentBounds.RandomPosition();
		        EndAction(true);
		    }
		    catch (System.Exception ex)
		    {
                Debug.LogError("Error creating random position: " + ex.Message);
                EndAction(false);
            }
		
		}
	}
}

