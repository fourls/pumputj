using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class DynamicCircle : MonoBehaviour {
	public Color color;
	[Range(0.01f, 15f)]
	public float radius = 1.0f;
	public float width = 1/16;

	[Range(3, 256)]
	public int numSegments = 128;


	void Start () {
		DoRenderer();
	}

	void Update() {
		if(!Application.isPlaying) {
			DoRenderer();
		}
	}

	public void DoRenderer () {
		LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.startColor = color;
		lineRenderer.endColor = color;
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;
		lineRenderer.positionCount = numSegments + 1;
		lineRenderer.useWorldSpace = false;

		float deltaTheta = (float) (2.0 * Mathf.PI) / numSegments;
		float theta = 0f;

		for (int i = 0 ; i < numSegments + 1 ; i++) {
			float x = radius * Mathf.Cos(theta);
			float y = radius * Mathf.Sin(theta);
			Vector3 pos = new Vector3(x, y, 0);
			lineRenderer.SetPosition(i, pos);
			theta += deltaTheta;
		}
	}
}
