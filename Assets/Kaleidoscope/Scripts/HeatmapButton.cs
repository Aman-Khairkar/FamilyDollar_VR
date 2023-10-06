using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapButton : SelectableButton
{
    public bool heatmapIsOn;
    // Use this for initialization
    void Start()
    {

    }


    public override void DoAction()
    {
        base.DoAction();
        StartCoroutine(TranslateCR());

        if (heatmapIsOn)
            SequenceManager.Instance.ToggleHeatMap(false);
        else
            SequenceManager.Instance.ToggleHeatMap(true);
        Debug.Log("play clicked");
    }
}