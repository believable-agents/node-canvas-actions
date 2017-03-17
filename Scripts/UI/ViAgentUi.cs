using UnityEngine;
using System.Collections;
using System.Linq;
using ViAgents.Physiology;
using ViAgents;

namespace ViAgents.Unity
{
    //[ExecuteInEditMode]
    public class ViAgentUi : MonoBehaviour
    {
        public bool showUI = false;

        //public GUIStyle progressNormal;
        //public GUIStyle progressWarning;

        private DayNightCycle dayNight;

        private string timeOverride = "00:00";

        const int panelWidth = 200;
        const int logWidth = 500;

        Physiology agentPhysiology;
        ViAgent viAgent;

        private bool showLog = true;
        private Vector2 scrollPosition = Vector2.zero;

        private bool showActions = true;
        // private bool showQueue = true;
        private bool showSensor = true;
        private bool showDebug = false;
        // Use this for initialization
        void Start()
        {
            dayNight = GameObject.FindObjectOfType<DayNightCycle>();
            viAgent = GetComponent<ViAgent>();
            agentPhysiology = GetComponent<Physiology>();

        }

        // Update is called once per frame
        void OnGUI()
        {
            if (!showUI)
            {
                viAgent.agent.keepLog = false;
                return;
            }
            viAgent.agent.keepLog = true;

            // Check that the Delivery Quest is in progress
            //if(MQ.InProgress) {
            // Draw the "Overall" objective
            GUI.Box(new Rect(10, 10, panelWidth, 20), string.Format("{0:00}:{1:00}", dayNight.Hour, dayNight.Minute));

            GUI.BeginGroup(new Rect(10, 40, panelWidth, 180));
            GUI.Box(new Rect(0, 0, panelWidth, 170), "Controls");
            GUI.Label(new Rect(10, 25, 40, 20), "Time: ");
            timeOverride = GUI.TextField(new Rect(50, 25, 40, 20), timeOverride);
            if (GUI.Button(new Rect(95, 25, 95, 20), "Set"))
            {
                var previousTime = dayNight.SunTime;
                var split = timeOverride.Split(':');
                int hours, minutes;
                if (split.Length == 2 && int.TryParse(split[0], out hours) && int.TryParse(split[1], out minutes))
                {
                    dayNight.SetTime(hours + minutes / 60f);
                }
                else
                {
                    timeOverride = "00:00";
                }

                // make sure that everyone wakes up
                if ((previousTime < 7 || previousTime > 18) && dayNight.SunTime > 7 && dayNight.SunTime < 18)
                {
                    if (viAgent != null && viAgent.agent.IsSleeping)
                    {
                        viAgent.agent.Sense(new SensorData(Sensor.Schedule, "wakeUp", 10));
                    }

                    foreach (var member in GameObject.FindGameObjectsWithTag("Crowd"))
                    {
                        var vi = member.GetComponent<ViAgent>();
                        if (vi != null && vi.agent.IsSleeping)
                        {
                            vi.agent.Sense(new SensorData(Sensor.Schedule, "wakeUp", 10));
                        }
                    }
                }
            }

            //if (GUI.Button(new Rect(10, 50, panelWidth - 20, 25), "Select Player"))
            //{
            //	camera.enabled = false;
            //	agentPhysiology = null;
            //	ctrl.enabled = true;
            //}

            //if (GUI.Button(new Rect(10, 80, panelWidth - 20, 25), "Select Beggars"))
            //{
            //	SelectAgent("Crowd", "Beggar");
            //}
            //if (GUI.Button(new Rect(10, 110, panelWidth - 20, 25), "Select Walkers"))
            //{
            //	SelectAgent("Crowd", "Walker");
            //}
            //if (GUI.Button(new Rect(10, 140, panelWidth - 20, 25), "Select Priest"))
            //{
            //	SelectAgent("Priests", "Priest");
            //}
            GUI.EndGroup();


            GUI.BeginGroup(new Rect(10, 220, panelWidth, 275));

            // physiology and viagent
            if (agentPhysiology != null)
            {
                GUI.Box(new Rect(0, 0, panelWidth, 275), "Agent: " + agentPhysiology.name);

                // Hunger
                GUI.Label(new Rect(10, 25, 60, 20), "Hunger: ");
                GUI.Box(new Rect(65, 25, 60, 20), agentPhysiology.Hunger.ToString("00") + "%");
                if (agentPhysiology.Hunger > 15)
                {
                    GUI.Box(new Rect(65, 25, 60 * (agentPhysiology.Hunger / 100), 20), "");
                }
                if (GUI.Button(new Rect(130, 25, 60, 20), "Hungry"))
                {
                    agentPhysiology.Hunger = 100;
                }

                // Thirst
                GUI.Label(new Rect(10, 50, 60, 20), "Thirst: ");
                GUI.Box(new Rect(65, 50, 60, 20), agentPhysiology.Thirst.ToString("00") + "%");
                if (agentPhysiology.Thirst > 15)
                {
                    GUI.Box(new Rect(65, 50, 60 * (agentPhysiology.Thirst / 100), 20), "");
                }
                if (GUI.Button(new Rect(130, 50, 60, 20), "Thirsty"))
                {
                    agentPhysiology.Thirst = 100;
                }

                // fatigue
                GUI.Label(new Rect(10, 75, 60, 20), "Energy: ");
                GUI.Box(new Rect(65, 75, 60, 20), agentPhysiology.Energy.ToString("00") + "%");
                if (agentPhysiology.Energy > 15)
                {
                    GUI.Box(new Rect(65, 75, 60 * (agentPhysiology.Energy / 100), 20), "");
                }
                if (GUI.Button(new Rect(130, 75, 60, 20), "Tired"))
                {
                    agentPhysiology.Energy = 0;
                }
            }

            // runtime info

            if (viAgent.agent.CurrentAction != null)
            {
                GUI.Label(new Rect(10, 100, 300, 20),
                    string.Format("Action: {0} with {1}", viAgent.agent.CurrentAction.sensorRequest,
                        ((Actions.ActionBt)viAgent.agent.CurrentAction).BT.name));
            }

            GUI.Box(new Rect(5, 125, 190, 120), "Queue");

            var str = "";
            foreach (var d in viAgent.agent.WorkQueue)
            {
                str += string.Format("[{0}] {1} - {2}\n", d.Sensor, d.SensorRequest, d.Priority);
            }

            GUI.Label(new Rect(10, 145, 180, 100), string.IsNullOrEmpty(str) ? "Empty" : str);

            // toggle log
            showLog = GUI.Toggle(new Rect(10, 250, 300, 20), showLog, "Show Log");

            GUI.EndGroup();

            if (showLog)
            {

                GUI.BeginGroup(new Rect(Screen.width - logWidth - 10, 10, logWidth, Screen.height - 20));
                GUI.BeginScrollView(new Rect(0, 0, logWidth, Screen.height - 20), scrollPosition, new Rect(0, 0, 220, 200));
                GUI.Box(new Rect(0, 0, logWidth, Screen.height - 20), "");
                GUI.Label(new Rect(10, 5, 60, 25), "Agent Log");
                showActions = GUI.Toggle(new Rect(120, 5, 80, 25), showActions, "Actions");
                showSensor = GUI.Toggle(new Rect(200, 5, 80, 25), showSensor, "Sensor");
                //showQueue = GUI.Toggle(new Rect(280, 5, 80, 25), showQueue, "Queue");
                showDebug = GUI.Toggle(new Rect(380, 5, 80, 25), showDebug, "Debug");

                var level = showDebug ? LogLevel.Debug : LogLevel.Info;
                var filtered = viAgent.agent.log.Where((l) =>
                    l.Level >= level && (
                    showActions && l.Source == LogSource.Action ||
                    //showQueue && l.Source == LogSource.Queue ||
                    showSensor && l.Source == LogSource.Sensor)).Take(30).Select(l => l.ToString()).ToArray();

                var message = string.Join("\n", filtered);

                GUI.Label(new Rect(10, 25, logWidth - 20, Screen.height - 50), message);
                GUI.EndScrollView();
                GUI.EndGroup();
            }

            // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
            //		if (GUI.Button (new Rect (20,40,80,20), "Level 1")) {
            //			Application.LoadLevel (1);
            //		}




        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

    }
}
