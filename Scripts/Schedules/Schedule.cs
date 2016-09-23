using UnityEngine;
using ViAgents.Schedules;

namespace ViAgents.Unity.Schedules {

	using System.Collections.Generic;
	using System.Linq;

	[System.Serializable]
	public class Schedule : ScriptableObject {
		public List<ScheduledItem> items;
	}
}
