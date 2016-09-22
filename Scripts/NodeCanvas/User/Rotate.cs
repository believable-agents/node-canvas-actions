using UnityEngine;

using NodeCanvas.Framework;
using ParadoxNotion.Design;


[Category("Transform")]
public class Rotate : ActionTask<Transform>
{

    public BBParameter<float> x;
    public BBParameter<float> y;
    public BBParameter<float> z;
    public BBParameter<float> time;
    public bool linear;

    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private float factor;
    

    protected override void OnExecute()
    {
        initialRotation = agent.rotation;
        targetRotation = agent.rotation * Quaternion.Euler(x.value, y.value, z.value);
        factor = 1f / time.value;
    }

    protected override void OnUpdate()
    {
        var timed = elapsedTime * factor;
        // Debug.Log(timed);
        if (linear)
        {
            agent.rotation = Quaternion.Lerp(initialRotation, targetRotation, timed);
        }
        else
        {
            agent.rotation = Quaternion.Slerp(initialRotation, targetRotation, timed);
        }
        // Debug.Log(agent.rotation.eulerAngles);

        if (timed >= 1)
        {
            EndAction(true);
        }
    }

    protected override string info
    {
        get { return "Rotating " + x.value + "," + y.value + "," + z.value; }
    }
}