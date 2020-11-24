using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Removes trees on terrain within a specified radius
/// Saves trees on startup and restores them afterwards
/// Add it as Component to a GameObject. 
/// The position of the GameObject is the center.
/// If you select the GameObject a Gismo Sphere shows the radius visually.
/// 
/// Call the method RemoveTreesInRadius *or* change the value removeTreesInRadius in the Inspector
/// 
/// Attention : Make a backup of your terrain, just in case, and do not use it in Editor Mode (yet).
/// 
/// 11 / 2020 Tom Trottel
/// This is CC0, but if you want to support me, well, have fun while using it ;)
/// </summary>

public class TreeOnTerrainRemover : MonoBehaviour
{

	#region Public Fields

	public bool debugMode = false;

	public bool removeTreesInRadius = false;

	public Terrain terrainToWorkOn;

	[Range(1,200)]
	public float removeTreeRadius = 3f;

	#endregion

	#region Private Fields

	TreeInstance[] savedTrees;

	#endregion

	#region Unity Methods

	void Start()
	{
		savedTrees = terrainToWorkOn.terrainData.treeInstances;
	}

	private void OnApplicationQuit()
	{
		terrainToWorkOn.terrainData.treeInstances = savedTrees;
	}

	private void OnValidate()
	{
		if (removeTreesInRadius)
			RemoveTreesInRadius();
	}

	public void RemoveAllTrees()
	{
		terrainToWorkOn.terrainData.treeInstances = new List<TreeInstance>().ToArray();
	}

	public void RemoveTreesInRadius()
	{
		TreeInstance[] trees = terrainToWorkOn.terrainData.treeInstances;

		List<TreeInstance> treeList = new List<TreeInstance>();

		for (int i = 0; i < trees.Length; i++)
		{

			Vector3 treeWorldPos = Vector3.Scale(trees[i].position, terrainToWorkOn.terrainData.size);

			float distance = Vector3.Distance(treeWorldPos, this.transform.position);

			if (distance >= removeTreeRadius)
				treeList.Add(trees[i]);

		}

		terrainToWorkOn.terrainData.treeInstances = treeList.ToArray();
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, removeTreeRadius);
	}

	#endregion

	#region Class Methods

	protected void dbg(string message, bool error=false)
	{
		if (debugMode & !error)
			Debug.Log("[ "+this.GetType().Name+" (" + Time.time + ")] " + message);

		if (error)
			Debug.LogError("<color=\"red\">[" + this.GetType().Name + " (" + Time.time + ")] " + message + "</color>");
	}
	
	#endregion

}
