using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberButton : SelectableButton
{
    public override void DoAction()
    {
        base.DoAction();
        Debug.Log("Number " + System.Int32.Parse(this.name));
        transform.root.GetComponent<EntryScreen>().AddNumber(System.Int32.Parse(this.name));
    }
}
