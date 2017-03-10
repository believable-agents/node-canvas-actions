using System;
using ViAgents.Schedules;

namespace ViAgents.Unity.Schedules {

	using UnityEngine;
     
	[RequireComponent(typeof(ViAgent))]
	public class Scheduler : MonoBehaviour {

		static DayNightCycle timeControl;

		#region Fields
	    private ScheduleManager scheduleManager;
	    public ViAgentSchedule schedule;
		#endregion

		void Start() {
			var viAgent = gameObject.GetComponent<ViAgent> ();
			if (viAgent == null) {
				Debug.LogError("Scheduler works only with PriorityPlanningAgent");
			}

			if (schedule == null) {
				Debug.LogError("Agent has no schedule!");
			}

            // create the schedule and scheduler
		    var agentSchedule = new Schedule(schedule.items);
		    this.scheduleManager = new ScheduleManager(viAgent.agent, agentSchedule);

			if (timeControl == null) {
				timeControl = GameObject.Find (DayNightCycle.GameObjectName).GetComponent<DayNightCycle> ();
			}
		}

	    private float currentTime = 24f;
	    void Update()
	    {
	        if (Math.Abs(timeControl.SunTime - currentTime) > 0.25)
	        {
                currentTime = timeControl.SunTime;
                this.scheduleManager.Check(timeControl.Hour, timeControl.Minute);
            }
	    }
	}

}
