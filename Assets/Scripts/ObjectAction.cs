using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectAction {
	public List<Vector2Int> affectedLocations;
	public Vector2Int relocation;
	public GameObject prefab;
}
