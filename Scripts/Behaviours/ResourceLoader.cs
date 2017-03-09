using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public class ResourceLoader {
	private static GameObject[] _males;
	private static GameObject[] _females;

	static List<GameObject> attachments;

	static string[] actionNames;
	static System.Type[] actionTypes;

	static string[] planNames;

    static ResourceLibrary library;

    static ResourceLibrary Library
    {
        get
        {
            if (!library)
            {
                library = (GameObject.Find("@Resources") as GameObject).GetComponent<ResourceLibrary>();
            }
            return library;
        }
    }

	//public static List<GameObject> Attachments {
	//	get {
	//		if (attachments == null) {
	//			var objs = Resources.LoadAll ("Objects/Attachments");
	//			attachments = new List<GameObject> ();
	//			foreach (var obj in objs)
	//				attachments.Add ((GameObject) obj);
	//		}
	//		return attachments;
	//	}
	//}

	public static GameObject[] Males {
		get {
			if (_males == null) {
                _males = Library.FindGroup("Male").resources;
			}
			return _males;
		}
	}

	public static GameObject[] Females {
		get {
			if (_females == null) {
                _females = Library.FindGroup("Female").resources;
            }
			return _females;
		}
	}


	// possible garbage
	/*
	static List<StaticPlan> staticPlans;

	public static string[] ActionNames {
		get {
			if (actionNames == null) {
				InitActions ();
			}
			return actionNames;
		}
	}
	
	
	public static System.Type[] ActionTypes {
		get {
			if (actionTypes == null) {
				InitActions ();
			}
			return actionTypes;
		}
	}

	public static string[] PlanNames {
		get {
			if (planNames == null) {
				InitPlans ();
			}
			return planNames;
		}
	}


	public static List<StaticPlan> StaticPlans {
		get {
			if (staticPlans == null) {
				InitPlans ();
			}
			return staticPlans;
		}
	}

	static void AddSkins(List<List<Material>> skins, object[] mats) {
		foreach (var omat in mats) {
			var mat = omat as Material;	
			var name = mat.name.Substring (0, mat.name.LastIndexOf ("_"));

			bool found = false;
			foreach (var set in skins) {
				if (set [0].name.StartsWith (name)) {
					set.Add (mat);
					found = true;
					break;
				} else continue;
			}

			if (!found) {
				var skinSet = new List<Material> ();
				skinSet.Add (mat);
				skins.Add (skinSet);
			}
		}
	}

	public static void InitPlans() {
//		Debug.Log ("Plan init");
		// find all plans
		var path = new DirectoryInfo("./Assets/Resources/Plans");
		if (!path.Exists)
		{
			path.Create();
		}

		var plans = path.GetFiles("*.xml");
		staticPlans = new List<StaticPlan> ();

		// load via paths
		if (plans.Length > 0) {
			// find all plans
			planNames = plans.Select(w => Path.GetFileNameWithoutExtension(w.Name)).ToArray();
			
			// load all plans

			foreach (var planFileInfo in plans) {
				var plan = StaticPlan.LoadFromText (File.ReadAllText(planFileInfo.FullName));
//				Debug.Log (string.Format("Loaded plan '{0}' with items '{1}'", plan.PlanName, plan.Items));
				StaticPlans.Add (plan);
			}
		}
		// load via resources
		else {
			// find all plans
			var planPaths = Resources.LoadAll("Plans");
			planNames = planPaths.Select(w => Path.GetFileNameWithoutExtension(w.name)).ToArray();
			
			// load all plans
			foreach (var planName in planPaths) {
				var plan = StaticPlan.LoadFromText (((TextAsset)planName).text);
				//			Debug.Log (string.Format("Loaded plan '{0}' with items '{1}'", plan.PlanName, plan.Items));
				StaticPlans.Add (plan);
			}

			Resources.UnloadUnusedAssets();
		}
//		Debug.Log ("Loaded plans: " + StaticPlans.Count);

	}

	static void InitActions() {
		// list all available actions
		var an = new List<string>();
		var at = new List<System.Type> ();

		Assembly ass = typeof(PlanAction).Assembly;
		foreach (System.Type type in ass.GetTypes())
		{
			if (type.IsSubclassOf(typeof(PlanAction)))
			{
				an.Add(type.Name);
				at.Add (type);
			}
		}

		// add parameters
		at.Add (typeof(GameObjectActionParameter));
		at.Add (typeof(IntActionParameter));
		at.Add (typeof(TagActionParameter));
		at.Add (typeof(LayerActionParameter));

		actionNames = an.ToArray();
		actionTypes = at.ToArray();
	}
	*/
}
