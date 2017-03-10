using UnityEngine;
using System.Collections;
using ViAgents.Physiology;
using ViAgents;
using System.Collections.Generic;
using ViAgents.Actions;
using NodeCanvas.BehaviourTrees;
using ViAgents.Schedules;
using NodeCanvas;
using NodeCanvas.Framework;
using System.Linq;
using ViAgents.Unity.Actions;
using ViAgents.Unity;
using ViAgents.Unity.Schedules;

public class Spawn : MonoBehaviour {

    public enum PropHandling
    {
        Everyone,
        Mixed,
        None
    }

	public GameObject model;
    public bool male;
	public List<ActionSet> actionSets;
	public Inventory inventory;
	public ViAgents.Unity.Schedules.ViAgentSchedule schedule;
	public RuntimeAnimatorController animationController;
	public string agentName;
	public string agentTag = "Crowd";
    public Vector3 spawnOffset;
    public float scaleMin;
    public float scaleMax;
    public PropHandling propHandling;

    // crowd related

    public int clones = 1;
    public ConvexBounds bounds;
    public float minHeight;
    public float maxHeight;
    public bool addUI;

	private static int counter;
	private static Transform clonesParent;

	// in here we'll keep all action sets
	// we keep a string refernce so that prefabs can access gameobjects
	// private static Dictionary<string, List<ActionSet>> actionSetDictionary = new Dictionary<string, List<ActionSet>>();

	// Use this for initialization
	void Start () {

		if (!Settings.Main.ShowClones)
			return;

		SpawnNPC();
	}

	void SpawnNPC() {
		StartCoroutine (SpawnCoroutine());
	}

	IEnumerator SpawnCoroutine() {
	    if (this.clones == 1)
	    {
	        yield return SpawnHuman(this.transform.position);
	    }
	    else
	    {
	        for (var i = 0; i < this.clones; i++)
	        {
	            yield return SpawnHuman(this.bounds.RandomPosition(0, minHeight, maxHeight));
                yield return new WaitForEndOfFrame();
	        }
	    }
	}

    IEnumerator SpawnHuman(Vector3 position)
    {
        yield return new WaitForEndOfFrame();

        // spawn a new instance of the 
        var currentModel = model;
        if (currentModel == null)
        {
            var models = male ? ResourceLoader.Males : ResourceLoader.Females;
            if (this.propHandling == PropHandling.Everyone )
            {
                models = models.Where(w => w.name.Contains("Prop")).ToArray();
            } else if (this.propHandling == PropHandling.None)
            {
                models = models.Where(w => !w.name.Contains("Prop")).ToArray();
            }
            
            currentModel = (GameObject) models.Random();
            Debug.Log("Model: " + currentModel.name);
        }

        // in case this is an interactive object, we spawn object at an offset position
        if (GetComponent<InteractiveObjectBT>() != null)
        {
            position = GetComponent<InteractiveObjectBT>().OffsetPosition;
        }

        // spawn model
        var spawn = Instantiate(currentModel, position, transform.rotation) as GameObject;
        //spawn.transform.parent = transform;

        // set scale
        if (scaleMin >= 0.00001 && scaleMax >= 0.00001)
        {
            var scale = Random.Range(scaleMin, scaleMax);
            spawn.transform.localScale = new Vector3(scale, scale, scale);
        }

        // set name
        spawn.name = (agentName == null ? "Clone" : agentName) + counter++;
        spawn.tag = this.agentTag;

        // set parent
        if (clonesParent == null)
        {
            clonesParent = GameObject.Find("@Clones").transform;
        }
       
        spawn.transform.parent = this.clones == 1 ? clonesParent : this.transform;

        yield return new WaitForEndOfFrame();

        // animator
        var anim = spawn.GetComponent<Animator>();
        anim.runtimeAnimatorController = animationController;
        anim.speed = Random.Range(0.35f, 1f);

        // navigation
        var navigation = spawn.AddComponent<UnityEngine.AI.NavMeshAgent>();
        navigation.speed = Random.Range(0.15f, 0.4f);
        navigation.stoppingDistance = 1.5f;
        navigation.acceleration = 2f;
        navigation.height = 1.5f;
        navigation.angularSpeed = 500;

        // viagent
        var viAgent = spawn.AddComponent<ViAgent>();
        viAgent.actions = actionSets; // GetActionSet(actionSets);

        // collider
        var collider = spawn.GetComponent<CapsuleCollider>();
        collider.height = 1.5f;

        // add behaviour tree
        var bt = spawn.AddComponent<BehaviourTreeOwner>();
        //bt.executeOnStart = false;
		// bt.repeat = true;

        // add blackboard
        bt.blackboard = spawn.AddComponent<Blackboard>();

        if (GetComponent<Blackboard>() != null)
        {
            var bbt = GetComponent<Blackboard>();
			bt.blackboard.Merge(bbt);
            yield return new WaitForEndOfFrame();
        }
        bt.blackboard.SetValue("Bed", gameObject);


        // add bounds if needed
        if (this.bounds)
        {
            bt.blackboard.SetValue("Bounds", this.bounds);
        }

        // destroy rigidnbody for now
        Destroy(spawn.GetComponent<Rigidbody>());

        // sensors

        // inventory
        if (inventory)
        {
            var inv = spawn.AddComponent<Inventory>();
            inv.ItemList = inventory.ItemList;
        }

        // physiology
        var physiology = spawn.AddComponent<Physiology>();
        physiology.hungerModifier = Random.Range(0.25f, 1.5f);
        physiology.thirstModifier = Random.Range(0.25f, 1.5f);
        physiology.energyModifier = Random.Range(0.25f, 1.5f);

        // scheduler
        var scheduler = spawn.AddComponent<Scheduler>();
        scheduler.schedule = schedule;

        // ui
        if (this.addUI)
        {
            var ui = spawn.AddComponent<ViAgentUi>();
            ui.enabled = true;
        }
    }

    static Vector3 positionSize = new Vector3(0.3f, 0.05f, 0.3f);

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(GetPosition(transform.position, spawnOffset), positionSize);
    }

    Vector3 GetPosition(Vector3 position, Vector3 offset)
    {
        var p = position + offset;
        return AffineUtility.RotateAroundPoint(p, transform.position, transform.rotation);
    }

    //    private List<ActionSet> GetActionSet(List<string> actionSet) {
    //		var key = string.Join(";", actionSet.ToArray());

    //		if (!actionSetDictionary.ContainsKey(key))
    //		{
    ////			Debug.Log(name + " is Iniitalising action set: " + key);
    //			// init action seets
    //			var actions = new List<ActionSet>();
    //			foreach (var ass in this.actionSets)
    //			{
    //				var assetName = ass;
    //				if (!assetName.EndsWith("ActionSet")) {
    //					assetName += "ActionSet";
    //				}
    //				actions.Add(GameObject.Find(assetName).GetComponent<ActionSet>());
    //			}
    //			actionSetDictionary [key] = actions;
    //		}
    //		return actionSetDictionary [key];
    //	}
}
