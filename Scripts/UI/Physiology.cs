using System;
using UnityEngine;
using System.Threading;

using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using ViAgents.Physiology;


namespace ViAgents.Unity
{
	[RequireComponent(typeof(ViAgent))]
	public class Physiology : MonoBehaviour
	{
		[Range(0f,4f)]
		public float SpeedMultiplier = 1;
		[Range(0f,5f)]
		public float hungerModifier = 1;
		[Range(0f,5f)]
		public float thirstModifier = 1;
		[Range(0f,5f)]
		public float energyModifier = 1;

	    private PhysiologyModel physiology;
		static DayNightCycle dayNight;
		static Transform player;
		
		ViAgent agent;	
        
		
		// properties
		
		public bool IsThirsty { get { return this.physiology.IsThirsty; } }
		
		public bool IsHungry { get { return this.physiology.IsHungry; } }
		
		public bool IsTired { get { return this.physiology.IsTired; } }  
		
		public float Hunger { 
			get { return this.physiology != null ? this.physiology.Hunger : 0; }  
			set { this.physiology.Hunger = value; }
		}
		
		public float Thirst { 
			get { return this.physiology != null ? this.physiology.Thirst : 0; }  
			set { this.physiology.Thirst = value; }
		}
		
		public float Energy { 
			get { return this.physiology != null ? this.physiology.Energy : 0; }  
			set { this.physiology.Energy = value; }
		}
		
		void Awake() {
			this.agent = GetComponent<ViAgent> ();
			if (this.agent == null) {
				Debug.LogError (gameObject.name + " has no ViAgent component assigned!");
			}
			
			// find the time control component
			if (dayNight == null) {
                // find timer
				dayNight = GameObject.Find("DayNight").GetComponent<DayNightCycle>();
                // initialise player
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }

            this.physiology = new PhysiologyModel(dayNight.DayInMinutes * 60, this.agent);
        }

	    private float elapsedTimeInSeconds = 0f;
		
		void Update() {
		    if (this.physiology == null)
		    {
		        return;
		    }
            // update modifiers
		    this.physiology.speedMultiplier = this.SpeedMultiplier;
		    this.physiology.hungerModifier = this.hungerModifier;
		    this.physiology.energyModifier = this.energyModifier;
		    this.physiology.thirstModifier = this.thirstModifier;

			elapsedTimeInSeconds += Time.deltaTime;
			
			if (elapsedTimeInSeconds < 1f)
			{
				return;
			}

            // update modifiers
            this.physiology.Update(elapsedTimeInSeconds);

            // set fixed time back to 0
            elapsedTimeInSeconds = 0f;

			// notify agent 
			if (this.physiology.IsTired) {
				NotifyAgent("tired", 98);
			}
			if (this.physiology.IsHungry) {
				NotifyAgent("hungry", 99);
			}
			if (this.physiology.IsThirsty) {
				NotifyAgent("thirsty", 100);
			}
		}
		
		
		void NotifyAgent(string state, int priority) {
			this.agent.Sense(new SensorData(Sensor.Physiology, state, priority, -1, -1));
		}        
	}
}

