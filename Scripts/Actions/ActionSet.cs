using System;
using UnityEngine;
using System.Collections.Generic;


namespace ViAgents.Unity.Actions
{
    [Serializable]
	public class ActionSet : ScriptableObject
	{
		public List<ActionBt> actions;
	}
}

