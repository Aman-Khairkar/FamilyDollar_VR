using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
//using VRTK.Controllables.ArtificialBased;

public class AvatarColor : MonoBehaviour {

	//[SerializeField]
	//public List<Color> palette;

	private Renderer avatarCol;
    private GameObject pickedCol;


    // Use this for initialization
	void Start () { //avatar grabs a random color from the provided palette - FigJ

        pickedCol = GameObject.Find("General Manager");

        avatarCol = GetComponent<Renderer>();
        avatarCol.material.color = pickedCol.GetComponent<AvatarPalette>().PickColor(0);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
