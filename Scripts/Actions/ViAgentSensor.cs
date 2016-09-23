using UnityEngine;
using System.Collections;
using ViAgents;
using ViAgents.Unity;

public class ViAgentSensor : MonoBehaviour
{
    public Sensor sensor;
    public int priority;

    private ViAgent agent;

    void Start()
    {
        agent = GetComponent<ViAgent>() ?? GetComponentInParent<ViAgent>() ?? GetComponentInChildren<ViAgent>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Sensable"))
        {
            // find the topmost parent of the gameobject
            var go = collider.transform;
            while (go.parent != null)
            {
                go = go.parent;
            }
            agent.agent.Sense(new SensorData(sensor, go.gameObject.name, priority));
        }
    }
}
