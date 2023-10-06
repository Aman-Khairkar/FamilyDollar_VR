using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour {

    Renderer rend;
	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("DissolveByDistance");
	}
	
	// Update is called once per frame
	void Update () {
        float dist = Mathf.PingPong(Time.time, 1.0f);
        rend.material.SetFloat("_Distance", dist);
	}
}
