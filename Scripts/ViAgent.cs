using System;
using UnityEngine;
using System.Collections.Generic;
using ViAgents.Unity.Actions;

namespace ViAgents.Unity
{
    public class ViAgent : MonoBehaviour
    {
        const float thinkIntervalInSeconds = 1;
        static DayNightCycle timeControl;

        public List<ActionSet> actions;

        [HideInInspector]
        public PriorityPlanningAgent agent;

        void Awake()
        {
            // copy all actions into array
            var allActions = new List<ActionBt>();
            actions.ForEach((a) =>
            {
                if (a == null)
                {
                    Debug.LogError("No actions assigned to: " + gameObject.name);
                    return;
                }
                allActions.AddRange(a.actions);
            });

            // initialise agent
            agent = new PriorityPlanningAgent(this.gameObject.name, allActions.ToArray(), () => timeControl.SunTime);
            agent.logger = this.UnityLog;

            // we need to keep time in order to filter actions by expiration
            if (timeControl == null)
            {
                timeControl = GameObject.FindObjectOfType<DayNightCycle>();
            }
        }

        private float elapsedTime = float.MaxValue;
        void Update()
        {
            // we only think every once in a while
            elapsedTime += Time.deltaTime;
            if (elapsedTime < thinkIntervalInSeconds)
            {
                return;
            }
            elapsedTime = 0;

            // Debug.LogWarning("Reasoning at: " + timeControl.CurrentTime.ToString());

            var action = this.agent.Reason() as ActionBt;
            if (action == null)
            {
                return;
            }

            // we are requested to execute an action

            if (action.BT == null)
            {
                throw new Exception(string.Format("Action for '{0}:{1}' does not have a behaviour tree", action.sensor,
                    action.sensorRequest));
            }

            // set that current item is the requested item
            this.Log(LogLevel.Info, LogSource.Action, action + " started");

            // execute it
            action.Execute(this);

        }


        // proxy methods

        public void UnityLog(LogMessage message)
        {
            Debug.Log(message);
        }

        public void Log(LogLevel level, LogSource source, string message)
        { 
            this.agent.Log(level, source, message);
        }

        public void ActionFinished(ActionBt action)
        {
            this.agent.ActionFinished(action);
        }
    }
}
