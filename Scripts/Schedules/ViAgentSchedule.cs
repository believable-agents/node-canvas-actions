namespace ViAgents.Unity.Schedules {
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using ViAgents.Schedules;

    [Serializable]
	public class ViAgentSchedule : ScriptableObject {
        public List<ScheduledItem> items;
    }
}
