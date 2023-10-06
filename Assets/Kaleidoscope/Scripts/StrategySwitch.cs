using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategySwitch : MonoBehaviour {

	public GameObject[] strategyObjs;
    public string[] titleTexts;
    public string[] subtitleTexts;

    public Text titleText;
	public Text subtitleText;

    public int stratNum = 0;

    public void CycleStrategy()
    {
      

        strategyObjs[stratNum].SetActive(false);

        stratNum += 1;
        if (stratNum == 4)
            stratNum = 0;

        //titleText.text = titleTexts[stratNum];
        //subtitleText.text = subtitleTexts[stratNum];

        strategyObjs[stratNum].SetActive(true);
    }

}
