
using NodeCanvas;
using UnityEngine;
using System.Collections;
using ViAgents;

//[ScriptName("Command Agent")]
//[ScriptCategory("Uruk/ViAgents")]
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace ViAgents.Unity
{

    [Category("★ ViAgents")]
    public class CommandAgent : ActionTask<Transform>
    {

        public BBParameter<GameObject> commandedAgent;
        public Sensor sensor;
        public string request;
        public int priority;

        protected override void OnExecute()
        {
            var viagent = commandedAgent.value.GetComponent<ViAgent>();
            if (viagent == null)
            {
                EndAction();
                return;
            }

            viagent.Sense(new SensorData(sensor, request, priority));
            EndAction();
        }

        protected override string info
        {
            get { return string.Format("Command '{0}[1]' from '{2}'", request, sensor, priority); }
        }
    }
}