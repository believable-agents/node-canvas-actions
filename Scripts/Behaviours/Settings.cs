using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

//	public NavigationType NavigationType;
	public GUIStyle ComfortStyle;
	public GUIStyle HungerStyle;
	public ConvexBounds RandomWalkBounds;
	public bool ShowPhysiologyGUI;
	public bool ShowClones;

	public static Settings Main;

	void Awake() {
//		Debug.Log("Camera: " + Camera.main.name);
		var Settings = GameObject.FindGameObjectWithTag("Player");
        if (Settings == null)
        {
            Main = GameObject.Find("@Settings").GetComponent<Settings>();
        } else
        {
            Main = Settings.GetComponent<Settings>();
        }
//		Debug.Log("Bounds: " + Main.RandomWalkBounds);
	}

	/* Following is a list oa GestureIDs and ActionIDs used in Mecanim system 
	 * 
	 * GESTURES
	 * 
	 * 0 - 
	 * 1 -
	 * 2 - Wave (Layer2)
	 * 3 - avatar_salute (Layer2)
	 * 4 - avatar_blowkiss (Layer2)
	 * 
	 * ACTIONS
	 * 
	 * 0 - Sit
	 * 1 - SitOnGround
	 * 2 - StandFromSit
	 * 3 - Market
	 * 4 - Put On Head
	 * 5 - Wake Up
	 * 6 - Crouch
	 * 7 - Lie Down
	 * 8 - Lie to crouch
	 * 9 - Stand from crouch
	 * 10 - Stand from lie down
	 * 11 - Pickup Down
	 * 12 - Pickup Up
	 * 
	 * URUK ID
	 * 
	 * 0 - Stop Action
	 * 1 - MakePot
	 * 2 - Beg
	 * 3 - PraiseKing
	 * 4 - Pray
	 * 5 - Market
	 * 6 - PriestPray
	 */
}
