using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice;
using Photon.Voice.Unity;

public class DesktopMicMenu : MonoBehaviour {

	// Use this for initialization

	public Recorder rec;
	public List<AudioInEnumerator> mics;
	
	public GameObject canvas;
	//public GameObject button;
	public List<GameObject> micList;
	public Dropdown micDrop;

	//public UserManager player;
	void Start ()
	{
		rec = GameObject.Find("PhotonRecorder").GetComponent<Recorder>();
		mics = new List<AudioInEnumerator>(0);
		micList = new List<GameObject>(0);
		
		PopulateMicList();
	}
	

    private void PopulateMicList()
    {
        //from Photon manual, getting the list of available mics - FigJ
        Recorder.PhotonMicrophoneEnumerator.Refresh();

        var enumerator = Recorder.PhotonMicrophoneEnumerator;
        for (int i = 0; i < Recorder.PhotonMicrophoneEnumerator.Count; i++)
        {
            mics.Add(enumerator);

            micDrop.options.Add(new Dropdown.OptionData(Recorder.PhotonMicrophoneEnumerator.NameAtIndex(i)));
            if (rec.PhotonMicrophoneDeviceId == i)
            {
                micDrop.value = i;
            }
        }
    }

	public void ChangeMic(int i)
	{
        rec.UnityMicrophoneDevice = micDrop.options[i].text;
        //rec.PhotonMicrophoneDeviceId = Recorder.PhotonMicrophoneEnumerator.IDAtIndex(i); //this is Abyssal difficulty. +10 5* units only.
        Debug.Log("Current Microphone is: " + rec.PhotonMicrophoneDeviceId.ToString() + 
		          " and index ID is: " + mics[i].IDAtIndex(i));
        Recorder.PhotonMicrophoneEnumerator.Refresh();
        rec.RestartRecording(); //TIL I can't read
    }
}
