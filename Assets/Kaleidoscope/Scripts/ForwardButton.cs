using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardButton : SelectableButton {

    bool firstShot = true;

	// Use this for initialization
	void Start () {
		
	}
	
    public override void DoAction()
    {
        base.DoAction();
        StartCoroutine(TranslateCR());
        SequenceManager.Instance.Next();
        Debug.Log("forward clicked");
    }
}
