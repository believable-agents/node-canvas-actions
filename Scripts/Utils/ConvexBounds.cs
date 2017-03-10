using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConvexBounds : MonoBehaviour {

	public Transform[] Bounds;
	public Vector3 BottomLeft;
	public Vector3 TopRight;
    public Vector3 Center;

	private Vector2[] polygon;

	void Awake() 
	{
		var bl_x = float.MaxValue;
		var bl_z = float.MaxValue;
		var tr_x = float.MinValue;
		var tr_z = float.MinValue;
        var c_x = 0f;
        var c_y = 0f;
        var c_z = 0f;

        var p = new List<Vector2>();;
		// var i = 0;
		foreach (var tran in Bounds)
		{
		    if (tran == null) continue;
			p.Add(new Vector2 (tran.position.x, tran.position.z));

			if (tran.position.x < bl_x) bl_x = tran.position.x;
			if (tran.position.z < bl_z) bl_z = tran.position.z;
			if (tran.position.x > tr_x) tr_x = tran.position.x;
			if (tran.position.z > tr_z) tr_z = tran.position.z;
		    c_x += tran.position.x;
            c_y += tran.position.y;
            c_z += tran.position.z;
        }

	    polygon = p.ToArray();

		BottomLeft = new Vector3 (bl_x, 0, bl_z);
		TopRight = new Vector3 (tr_x, 0, tr_z);

        Center = new Vector3(c_x / Bounds.Length, c_y / Bounds.Length, c_z / Bounds.Length);
	}

    void CalculateCenter()
    {
        var c_x = 0f;
        var c_y = 0f;
        var c_z = 0f;

        int points = 0;

        foreach (var tran in Bounds)
        {
            if (tran == null) continue;
            c_x += tran.position.x;
            c_y += tran.position.y;
            c_z += tran.position.z;
            points++;
        }

        Center = new Vector3(c_x / points, c_y / points, c_z / points);
    }

	/// <summary>
	/// Determines if the given point is inside the polygon
	/// </summary>
	/// <param name="polygon">the vertices of polygon</param>
	/// <param name="testPoint">the given point</param>
	/// <returns>true if the point is inside the polygon; otherwise, false</returns>
	public bool IsPointInPolygon(Vector2 testPoint)
	{
		bool result = false;
		int j = polygon.Length - 1;
		for (int i = 0; i < polygon.Length; i++)
		{
			if (polygon[i].y < testPoint.y && polygon[j].y >= testPoint.y || polygon[j].y < testPoint.y && polygon[i].y >= testPoint.y)
			{
				if (polygon[i].x + (testPoint.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < testPoint.x)
				{
					result = !result;
				}
			}
			j = i;
		}
		return result;
	}

	public bool IsPointInPolygon(Vector3 testPoint)
	{
		return IsPointInPolygon (new Vector2 (testPoint.x, testPoint.z));
	}

    public static void DrawGizmos(ConvexBounds bounds)
    {
        bounds.CalculateCenter();
        Gizmos.color = new Color(1, 0, 0);

        if (bounds.Bounds.Length > 1)
        {
            for (var i = 1; i < bounds.Bounds.Length; i++)
            {
                if (bounds.Bounds[i] == null) return;
                Gizmos.DrawLine(bounds.Bounds[i - 1].position, bounds.Bounds[i].position);
            }
            Gizmos.DrawLine(bounds.Bounds[0].position, bounds.Bounds[bounds.Bounds.Length - 1].position);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(bounds.Center, 1);

        Gizmos.DrawLine(
            new Vector3(bounds.BottomLeft.x, 31, bounds.BottomLeft.y),
            new Vector3(bounds.BottomLeft.x + (bounds.TopRight.x - bounds.BottomLeft.x), 31, bounds.BottomLeft.y));

        Gizmos.DrawLine(
            new Vector3(bounds.BottomLeft.x, 31, bounds.BottomLeft.y),
            new Vector3(bounds.BottomLeft.x, 31, bounds.BottomLeft.y + (bounds.TopRight.y - bounds.BottomLeft.y)));

        Gizmos.DrawLine(
            new Vector3(bounds.TopRight.x, 31, bounds.TopRight.y),
            new Vector3(bounds.TopRight.x - (bounds.TopRight.x - bounds.BottomLeft.x), 31, bounds.TopRight.y));


        Gizmos.DrawLine(
            new Vector3(bounds.TopRight.x, 31, bounds.TopRight.y),
            new Vector3(bounds.TopRight.x, 31, bounds.TopRight.y - (bounds.TopRight.y - bounds.BottomLeft.y)));
    }

	// Called by the Unity Editor GUI every update cycle, but only when the object is selected.
	// Draws a sphere showing spawnRange's setting visually.
	void OnDrawGizmosSelected ()
	{
		DrawGizmos(this);
	}


    public Vector3 RandomPosition(float distance = 0f, float minHeight = -1000, float maxHeight = 1000)
    {
        Vector3 randomPos;
        if (distance <= float.Epsilon)
        {
            distance = ((this.Center - this.BottomLeft).magnitude + (this.Center - this.TopRight).magnitude)/2;
//            Debug.Log("Calculated distance: " + distance);
        }
        int checker = 0;
        do
        {
            randomPos = new Vector3(
                this.Center.x + Random.Range(-distance, distance),
                200,
                this.Center.z + Random.Range(-distance, distance)
                );
            if (checker++ > 20)
            {
                Debug.LogError("Safe exit");
                throw new UnityException("Retry limit reached, position not found");
            }

            // find y
            RaycastHit hit;
            if (Physics.Raycast(randomPos, Vector3.down, out hit))
            {
                randomPos = hit.point;
            }

        } while (!this.IsPointInPolygon(randomPos) || randomPos.y <= minHeight || randomPos.y >= maxHeight);
//        Debug.Log("Found position: " + randomPos);
        return randomPos;
    }
}
