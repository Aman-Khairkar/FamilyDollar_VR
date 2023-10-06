using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableTeleport : MonoBehaviour {
	
	Material mat;
	bool isSelected = false;
	bool materialHighlighted = false;
	public bool useOutlineHighlighter = true;

	public float height = 1.57f;


	public Outline highlighter;


	// Use this for initialization
	void Start()
	{

		if (!highlighter)
		{

			highlighter = GetComponent<Outline>();
	//		GetComponent<VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter>().enabled = false;

		}

    }

    public void Select()
	{
		materialHighlighted = true;
		highlighter.Highlight();

		//mat.SetColor("_EmissionColor", highlightColor);
	}

	public void UnSelect()
	{
		materialHighlighted = false;
		highlighter.Unhighlight();

		//mat.SetColor("_EmissionColor", Color.black);
	}

}
