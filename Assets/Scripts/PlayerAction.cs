using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAction : ObjectAction {
	public List<ComboKeys> comboKeys;
	public int jamCost = 10;
	[HideInInspector]
	public int consecutive = 0;

	public bool HandleKey(ComboKeys key) {
		if(comboKeys[consecutive] == key) {
			consecutive ++;
		} else {
			consecutive = 0;
		}

		return (consecutive >= comboKeys.Count);
	}
}
