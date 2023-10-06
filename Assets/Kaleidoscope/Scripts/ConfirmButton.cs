using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmButton : SelectableButton
{
    public override void DoAction()
    {
        base.DoAction();
        transform.root.GetComponent<EntryScreen>().CompareValues();
        Debug.Log("confirm clicked");
    }
}
