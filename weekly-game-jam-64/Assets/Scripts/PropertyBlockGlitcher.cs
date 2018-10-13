using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropertyGlitchSettings {
	[MinMax(0, 3)] public Vector2 GlitchedDuration = new Vector2(1, 1.5f);
	[MinMax(0, 3)] public Vector2 NormalDuration = new Vector2(1, 1.5f);
	[Range(0, 1)] public float LerpDuration = 0.0f;
	[Range(-1,1)]
	
	public float RandomMin;
	public float RandomMax;
	public string PropertyName;
}

public class PropertyBlockGlitcher : MonoBehaviour {
	public List<PropertyGlitchSettings> GlitchSettings = new List<PropertyGlitchSettings>();
	
	[MinMax(0, 1, ShowEditRange = true)]
	public Vector2 MyRange = new Vector2(0, 0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
