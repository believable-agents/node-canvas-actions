
using NodeCanvas.BehaviourTrees;
using NodeCanvas;
using UnityEngine;
using UnityEngine.EventSystems;
using ViAgents.Actions;

namespace ViAgents.Unity.Actions
{
    using System;

    /// <summary>
    /// Action that executes NodeCanvases behaviour tree
    /// </summary>
	[Serializable]
    public class ActionBt : ViAgents.Actions.Action
    {
        private static Transform parentObject;

        public bool runForever;

        public BehaviourTree BT;

        void Awake()
        {
            if (parentObject == null)
            {
                parentObject = GameObject.Find("@RuntimeTrees").transform;

                // clear existing trees
                if (parentObject != null && parentObject.childCount > 0)
                {
                    for (var i = parentObject.childCount; i >= 0; i++)
                    {
                        GameObject.DestroyImmediate(parentObject.GetChild(i));
                    }
                    Debug.Log("Destroyed trees: " + parentObject.childCount);
                }
            }
        }

        public override void Execute(object oagent)
        {
            var agent = (ViAgent)oagent;
            var bt = agent.GetComponent<BehaviourTreeOwner>();

            if (bt == null)
            {
                Debug.LogError("Target agent needs to have component BehaviourTreeOwner: " + agent.gameObject.name);
                return;
            }

            // get bb
            var bb = bt.blackboard;

            // copy all values from the blackboard of the BT
            if (BT.blackboard != null)
            {
                bb.Merge(BT.blackboard);
            }

            // new
            bt.blackboard = bb;

            // switch to new
            // agent.Log(LogSource.Action, "BT Start");
            if (bt.graph != null && BT.name == bt.graph.name)
            {
                // agent.Log(LogSource.Action, "Same graph, restarting");
                bt.StartBehaviour((result) =>
                {
                    agent.Log(LogLevel.Debug, LogSource.Action, "BT Finished");
                    agent.ActionFinished(this);
                });
            }
            else
            {
                bt.SwitchBehaviour(BT, (result) =>
                {
                    agent.Log(LogLevel.Debug, LogSource.Action, "BT Finished");
                    agent.ActionFinished(this);
                });
            }

            bt.repeat = runForever;
        }

        public override void Pause(object oagent)
        {
            var agent = (ViAgent)oagent;
            var bt = agent.GetComponent<BehaviourTreeOwner>();
            bt.PauseBehaviour();
        }

        public override void Resume(object oagent, object context)
        {
            var bt = (BehaviourTree)context;
            var agent = (ViAgent)oagent;
            var bto = agent.GetComponent<BehaviourTreeOwner>();
            bto.behaviour = bt;
            bt.repeat = runForever;
            // TODO: Investigate the effect of the parameter
            bt.StartGraph(agent, bto.blackboard, true, (result) => agent.ActionFinished(this));
        }

        public override void Abort(object oagent)
        {
            var agent = (ViAgent)oagent;
            var bt = agent.GetComponent<BehaviourTreeOwner>();
            // stop previous actions
            bt.StopBehaviour();
        }

        public override string ToString()
        {
            return string.Format("[{0}: {1}] Tree '{2}'", sensor, sensorRequest, BT.name);
        }
    }
}

