using UnityEngine;
using System.Collections;

namespace ViAgents.Unity.Behaviours
{
    public class RandomWaypointsSwim : RandomWaypointNavigation
    {
        public float waterLevel = 18.8f;

        private float originalOffset;
        private Vector3[] waypointsArray;

        new void Start()
        {
            base.Start();

            originalOffset = navAgent.baseOffset;
            waypointsArray = new Vector3[base.waypoints.Length];
            for (var i = 0; i < waypointsArray.Length; i++)
            {
                waypointsArray[i] = base.waypoints[i].transform.position;
            }
        }

        // Update is called once per frame
        new void Update()
        {
            if (trans.position.y > waterLevel)
            {
                navAgent.baseOffset -= trans.position.y - waterLevel;

                //				Debug.Log ("Lowering: " + navAgent.baseOffset + " " + trans.position);

            }
            else if (navAgent.baseOffset < originalOffset)
            {
                navAgent.baseOffset = Mathf.Clamp(navAgent.baseOffset + 0.02f, 0, originalOffset);

                //				Debug.Log ("Raising: " + navAgent.baseOffset + " " + trans.position);
            }



            base.Update();
        }

        protected override Vector3 GetNewPosition()
        {
            // sort waypoints by distance
            System.Array.Sort<Vector3>(waypointsArray, delegate (Vector3 x, Vector3 y) {
                return Vector3.Distance(trans.position, x) > Vector3.Distance(trans.position, y) ? 1 : -1;
            });

            return waypointsArray[UnityEngine.Random.Range(0, 5)];
        }
    }
}
