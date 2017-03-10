using UnityEngine;
using System.Collections;
using System;
using NodeCanvas;
using NodeCanvas.Framework;

public static class ClassExtensions
{
	public static object Default(this Type type)
	{
		object output = null;

		if (type.IsValueType)
		{
			output = Activator.CreateInstance(type);
		}

		return output;
	}

	// helper methods

	public static void Merge(this IBlackboard board1, IBlackboard board2) {
		foreach (var var2 in board2.variables) {
			if (!board1.variables.ContainsKey(var2.Key)) {
				// Debug.Log("Adding: " + val.dataName + " " + val.objectValue);
				board1.AddVariable(var2.Key, var2.Value.varType);
				board1.SetValue(var2.Key, var2.Value.value);
			}
		}
	}

	public static Vector3 RotateAround(this Vector3 point, Vector3 pivot, Quaternion angle) {
		return angle * (point - pivot) + pivot;
	}

	public static bool CompareWith(this Vector3 vectorOne, Vector3 vectorTwo, float precision = 0.01f) {
		return ((vectorOne - vectorTwo).sqrMagnitude <= (vectorOne * precision).sqrMagnitude );
	}

	public static object Random(this IList collection) {
		return collection[UnityEngine.Random.Range(0, collection.Count - 1)];
	}

	public static Transform Search(this Transform target, string name)
	{
		if (target.name == name) return target;		
		for (int i = 0; i < target.childCount; ++i)
		{
			var result = Search(target.GetChild(i), name);			
			if (result != null) return result;
		}		
		return null;
	}

	static Transform player;

	public static Vector3 PlayerPosition(this Transform target)
	{
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player").transform;
		}
		return player.position;
	}
}
