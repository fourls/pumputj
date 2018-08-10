using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Music : ScriptableObject {
	public AudioClip clip;
	public int bpm;
	public float offset = 0f;
}
