using UnityEngine;
using System.Collections;

namespace ViAgents.Unity {

	public class PhysiologyUI : MonoBehaviour {
		private float hungerT;
		private float thirstT;
		private float energyT;
		private Transform player;
		
		// aram code
		GameObject hungerBar ;
		GameObject thirstBar;
		GameObject energyBar;
		
		public bool show;
		
		private Physiology physiology;
	    private Canvas canvas;

		void Start() {
			physiology = this.GetComponentInParent<Physiology>();
			
			hungerBar = transform.Find("HungerValue").gameObject;
			thirstBar = transform.Find("ThirstValue").gameObject;
			energyBar = transform.Find("EnergyValue").gameObject;
			
			hungerT = physiology.Hunger / 100f;
			thirstT = physiology.Thirst / 100f;
			energyT = physiology.Energy / 100f;
			
			hungerBar.transform.localScale = new Vector3(hungerT, 1f, 1f);
			thirstBar.transform.localScale = new Vector3(thirstT, 1f, 1f);
			energyBar.transform.localScale = new Vector3(energyT, 1f, 1f);	
			
			player = GameObject.FindWithTag("MainCamera").transform;
		    canvas = this.GetComponent<Canvas>();
		}
		
		void Update() {
		    this.canvas.enabled = this.show;


			hungerT = physiology.Hunger / 100f;
			thirstT = physiology.Thirst / 100f;
			energyT = physiology.Energy / 100f;
			
			hungerBar.transform.localScale = new Vector3(hungerT, 1f, 1f);
			thirstBar.transform.localScale = new Vector3(thirstT, 1f, 1f);
			energyBar.transform.localScale = new Vector3(energyT, 1f, 1f);

            var lookPos = player.position - transform.position;
            lookPos.y = 0;
            this.transform.rotation = Quaternion.LookRotation(lookPos);
		}
	}
}
