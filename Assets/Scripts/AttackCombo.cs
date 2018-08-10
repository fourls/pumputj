using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackCombo {
	public List<ComboKeys> comboKeys;
	public GameObject prefab;
	public int jamCost = 10;
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
