using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyRepertoire : ScriptableObject {
	public ObjectAction leftMovement;
	public ObjectAction rightMovement;
	public ObjectAction upMovement;
	public ObjectAction downMovement;
	public ObjectAction attackAction;
}
