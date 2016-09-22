using NodeCanvas;
using UnityEngine;

//[ScriptName("Toggle Pushing")]
//[ScriptCategory("Uruk/Movement")]
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

[Category("★ User")]
public class WaitRandom : ActionTask<Transform>
{

    public float min = 0f;
    public float max = 0f;
    private float waitTime = 0f;
    public CompactStatus finishStatus = CompactStatus.Success;

    protected override string info
    {
        get { return "Wait Random " + min + " --> " + max; }
    }

    protected override void OnUpdate()
    {
        if (elapsedTime >= waitTime)
            EndAction(finishStatus == CompactStatus.Success ? true : false);
    }

    protected override void OnExecute()
    {
        waitTime = Random.Range(min, max);
    }
}

