using Oculus.Platform.Samples.VrHoops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityAlert : MonoBehaviour {

	private Renderer colorSet;
	private MeshRenderer ring;
	//private bool playerCollision = false;

	// Use this for initialization
	void Start () {

		GetComponent<Collider>();
		ring = GetComponent<MeshRenderer>();
		colorSet = GetComponent<Renderer>();

		ring.enabled = false;

		colorSet.material.color = Color.red;

	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	private void OnTriggerEnter(Collider other) 
	{
		//only change color when it's another player
		if (other.tag == "Proximity")
		{
			//colorSet.material.color = Color.red;
			ring.enabled = true;
			//colorSet.material.color = Color.red;
			Debug.Log("Players are too close!");
			//StartCoroutine("HoldAlert");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Proximity")
		{
			//colorSet.material.color = Color.red;
			ring.enabled = true;
			//colorSet.material.color = Color.red;
			Debug.Log("Players are still too close!");
			//StartCoroutine("HoldAlert");
		}
	}


	private void OnTriggerExit()
	{	
			//colorSet.material.color = new Color(0f, 1f, 1f, 0.4f);
			ring.enabled = false;
	}

	/*private IEnumerable HoldAlert()
	{
		colorSet.material.color = Color.red;
		yield return new WaitForSeconds(3.0f);
	}*/
}
