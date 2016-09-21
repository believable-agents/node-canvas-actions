using UnityEngine;
using NodeCanvas;

//[ScriptCategory("Uruk")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;


public class ClockCheck : ConditionTask {

	private static DayNightCycle dayNightCycle;

	[RequiredField]
	public BBParameter<float> hourStart;
	public BBParameter<float> hourEnd;

	protected override string OnInit ()
	{
		if (dayNightCycle == null) {
			var go = GameObject.Find("DayNight");
			if (go == null) {
				Debug.LogError("There is no 'DayNight' object in the scene!");
				return null;
			}
			dayNightCycle = go.GetComponent<DayNightCycle>();

		}
		return base.OnInit ();
	}

	protected override bool OnCheck()
	{		
		Debug.Log (string.Format("{0} < {1} < {2}", (int) (hourStart.value * 100f), (int) (dayNightCycle.SunTime * 100f), (int) (hourEnd.value * 100f)));
		return ((int) (hourStart.value * 100f)) <= ((int) (dayNightCycle.SunTime * 100f)) &&
			((int) (hourEnd.value * 100f)) >= ((int) (dayNightCycle.SunTime * 100f));
	}
}