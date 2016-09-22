
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
    public enum PhysiologyModifier
    {
        Hunger,
        Thirst,
        Energy
    }

    [Category("★ ViAgents")]
    public class SetPhysiology : ActionTask<Transform>
    {

        public BBParameter<float> value;
        public PhysiologyModifier modifier;

        protected override void OnExecute()
        {
            var viagent = agent.GetComponent<Physiology>();
            if (modifier == PhysiologyModifier.Energy)
            {
                viagent.Energy = value.value;
            }
            if (modifier == PhysiologyModifier.Hunger)
            {
                viagent.Hunger = value.value;
            }
            if (modifier == PhysiologyModifier.Thirst)
            {
                viagent.Thirst = value.value;
            }
            Debug.Log("Setting " + modifier + " to " + value.value);
            EndAction();
        }

        protected override string info
        {
            get { return string.Format("Setting '{0}' to '{1}'", modifier, value); }
        }
    }
}