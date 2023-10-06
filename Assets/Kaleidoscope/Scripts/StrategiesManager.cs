using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class StrategiesManager : MonoBehaviourPunCallbacks {

    [Header("List of Strategies to affect")]
    [Tooltip("All child products of these strategies will be affected any time you use the PropagateTransform Script")]
    public GameObject[] strategies;
    public List<GameObject> strategiesProductFolder;
    public List<string> strategiesPath;

    public int currentStrat = 0;

    // Use this for initialization
	void Start ()
	{
		//strategies = new GameObject[strategies.Length];
		strategies[0].SetActive(true);
        GetPaths();
	}

    void GetPaths()
    {
        for(int i = 0; i < strategiesProductFolder.Count; i++)
        {
            //for each one, add a path for this object
            //GameObject foundObject = GameObject.Find("/Shelves_Fem_v3 (1)/Drug/Products/" + objName);
            strategiesPath.Add(GetGameObjectPath(strategiesProductFolder[i]));
        }
    }

    string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while(obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path + "/";
    }

    public void SwitchStrats(int i)
    {
        strategies[currentStrat].SetActive(false);
        currentStrat = i;
        if (currentStrat >= strategies.Length)
            currentStrat = 0;
        else if (currentStrat < 0)
            currentStrat = 0;
        // This will set the environment/background shelves + endcaps to be active if you aren't on the training strategy
        // and turn them off if you ARE on the training strategy
        strategies[currentStrat].SetActive(true);
        Debug.Log("Switching shelves...");
    }

    public void SwitchStrats(bool forward)
	{
		//switch strategies with this function
		strategies[currentStrat].SetActive(false);
        if (forward)
        {
            currentStrat++;
        }
        else
        {
            currentStrat--;
        }

        if (currentStrat >= strategies.Length)
            currentStrat = 0;
        else if (currentStrat < 0)
            currentStrat = strategies.Length - 1;
		strategies[currentStrat].SetActive(true);
		Debug.Log("Switching shelves...");
	}
}
