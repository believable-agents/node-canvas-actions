using UnityEngine;
using System.Collections;

public class ClickCoordinates : MonoBehaviour {

	private GameObject hitObject;
	private Vector3 hitPos;
	private Vector3 hitObjectPos;
	private RaycastHit hit;

	void Update(){
		if(Input.GetMouseButtonDown(0)){
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition) ;
			if(Physics.Raycast(ray , out hit)){
//				hitObject = hit.transform.gameObject;
				hitPos = hit.point ;
				hitObjectPos = hit.transform.position ;
			}
		}
	}

	//I guess you're wanting to use GUI to display this stuff??? 
	void OnGUI(){
		//The world position of the ray's contact point->
		GUI.Box(new Rect(5,5,100,50), "Ray Hit Vector3 = " +hitPos) ;
		// The world position of the object the ray hits->
		GUI.Box(new Rect(5,105,100,50), "Hit Object Vector3 = " +hitObjectPos) ;
	}
}