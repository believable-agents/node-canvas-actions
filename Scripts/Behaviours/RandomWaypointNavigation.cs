using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using SWS;

namespace ViAgents.Unity.Behaviours
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    public class RandomWaypointNavigation : MonoBehaviour
    {

        public PathManager path;

        protected Transform trans;
        protected UnityEngine.AI.NavMeshAgent navAgent;

        protected Vector3? lastRequest;
        protected Transform[] waypoints;

        // Use this for initialization
        protected void Start()
        {
            trans = transform;
            navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            waypoints = path.waypoints; // blackboard.GetValue<List<GameObject>> ("Waypoints");

            if (waypoints == null || waypoints.Length == 0)
            {
                Debug.LogError("There were no waypoints!: " + waypoints);
            }
        }

        // Update is called once per frame
        protected void Update()
        {

            if (waypoints == null)
                return;

            if (lastRequest == null)
            {
                lastRequest = GetNewPosition();
                navAgent.SetDestination(lastRequest.Value);
                //				Debug.Log("Moving to: " + lastRequest.Value);
            }

            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                lastRequest = null;
            }
        }

        protected virtual Vector3 GetNewPosition()
        {
            return waypoints[UnityEngine.Random.Range(0, waypoints.Length - 1)].transform.position;
        }

    }
}
