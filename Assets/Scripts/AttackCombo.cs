using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackCombo {
	public List<ComboKeys> comboKeys;
	public int damage;
	public int knockback;
	public float damageMultiplier = 1f;
	public float knockbackMultiplier = 1f;

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
