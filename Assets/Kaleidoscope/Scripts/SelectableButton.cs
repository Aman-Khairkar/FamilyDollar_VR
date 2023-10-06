using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using VRTK;

//[RequireComponent(typeof(VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter))]
public class SelectableButton : MonoBehaviour {

    //private VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter vrtkHighlighter;

    private Outline outline;

    //TODO: combine rig offset + playspace to put the polayer on tp loc, not just playspace
    public Material mat;
    private Color originalColor;
    public Color highlightColor;// = new Color(0.375f, 0.375f, 0.375f);
    bool isHighlighted = false;

   // public SequenceManager manager;
   // public VRTK_ControllerEvents ce;

    private void OnEnable()
    {
        /*if (!ce)
            //Debug.LogError("No controller events script detected! Your inputs are broke.");
        else
        {
            ce.TriggerPressed += TriggerPress;
            //ce.TriggerReleased += TriggerRelease;
        }*/
        if(mat == null)
            mat = GetComponent<MeshRenderer>().materials[0];
       // vrtkHighlighter = GetComponent<VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter>();
        mat.EnableKeyword("_EMISSION");
        // StartCoroutine("TranslateCR");

        outline = GetComponent<Outline>();

    }
    // Use this for initialization
    public virtual void DoAction()
    {
        UnHighlight();
    }

    protected IEnumerator TranslateCR()
    {
        transform.Translate(0, 0, -0.05f);
        yield return new WaitForSeconds(0.15f);
        transform.Translate( 0, 0, 0.05f);  
    }
    /*
    public void TriggerPress(object sender, ControllerInteractionEventArgs e)
    {
        if (outline.isHighlighted)
        {
            DoAction();
        }
    }

    public void TriggerRelease(object sender, ControllerInteractionEventArgs e)
    {

    }*/



    public void Highlight()
    {
        if (isHighlighted)
            return;
        //outline.Highlight();

    }

    public void UnHighlight()
    {
        //outline.Unhighlight();

    }
}