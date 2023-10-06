using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackspaceButton : SelectableButton
{
    public override void DoAction()
    {
        base.DoAction();
        transform.root.GetComponent<EntryScreen>().RemoveNumber();

        Debug.Log("Backspace clicked");
    }
}
