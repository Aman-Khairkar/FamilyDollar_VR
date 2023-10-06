using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapController : MonoBehaviour
{
    public Texture HeatMapImage;
    float offset = 0.1f;
    private void OnEnable()
    {
        //Find shelf fixture to set distance of Heatmap plane from shelf
        GameObject shelf = GameObject.FindWithTag("Shelf");
        float x = transform.localPosition.x;
        float y = transform.localPosition.y;
        float z = shelf.transform.localPosition.z - shelf.transform.localScale.z / 2 - offset;
        this.transform.localPosition = new Vector3(x, y, z);
        if (HeatMapImage != null)
            this.GetComponent<MeshRenderer>().material.mainTexture = HeatMapImage;
    }
}
