using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR;

public class SequenceManager : Singleton<SequenceManager> {

    public GameObject[] strategyObjs;
    public GameObject[] heatMaps;
    public VideoClip[] clips;
    public Texture[] labelTexts;
    public GameObject[] TPLocationParents;
    public Texture[] exceptionSplashScreens; 

    public Texture heatmapOn, heatmapOff;

    public int stratNum = 0;
    public Transform origin;
    public Transform origin10ft;
    public Transform origin20ft;
    public Transform playerRoot;
    //public VRTK.VRTK_HeightAdjustTeleport teleporter;

    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject videoPlane;
    public Texture videoMaterial;
    public ForwardButton forwardButton;
    public HeatmapButton heatMapButton;
    public VideoPlayer vplayer;
    //public Renderer labelPanelRenderer;
    public Text stratName;


    bool paused = false;
    bool videoStartedForCurrentSequence = false;
    float waitTime = 0;

	void Start ()
    {
        stratName.text = labelTexts[stratNum].name;
        vplayer.loopPointReached += EndReached;
      //  ReassignStrategyArray();
      //  DetermineTPLocOrigin();
      //  XRSettings.eyeTextureResolutionScale = 1.25f;
    }
    /*
    //Keyboard Commands
    void Update()
    {
        //enable keyboard commands
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            JumpToStrategyNum(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            JumpToStrategyNum(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            JumpToStrategyNum(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            JumpToStrategyNum(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            JumpToStrategyNum(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            JumpToStrategyNum(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            JumpToStrategyNum(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            JumpToStrategyNum(8);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHeatMap(!heatMapButton.heatmapIsOn);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (vplayer.isPlaying)
                vplayer.Pause();
            else
                vplayer.Play();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //forwardButton.DoAction();
            Next();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //backButton.DoAction();
            Previous();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //reset player pos
            //playerRoot.position = origin.position;


            //VRTK.DestinationMarkerEventArgs args = new VRTK.DestinationMarkerEventArgs();
            //args.destinationPosition = origin.position;
            //teleporter.Teleport(args);
            teleporter.ForceTeleport(origin.position, origin.rotation);
        }




        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }*/
    /*
    void ReassignStrategyArray()
    {
     //   strategyObjs = FindObjectOfType<StrategiesManager>().strategies;

    }

    private void OnLevelWasLoaded(int level)
    {
        ReassignStrategyArray();
    }*/

    void JumpToStrategyNum(int num)
    {
        strategyObjs[stratNum].SetActive(false);
        stratNum = num - 1;
        if (stratNum >= strategyObjs.Length)
            stratNum = strategyObjs.Length - 1;

        strategyObjs[stratNum].SetActive(true);
    }

    //method to fire when video is over, ending the sequence and awaiting 'next' button clicked
    public void EndReached(VideoPlayer vp)
    {
        vp.Stop();
        heatMaps[stratNum].SetActive(false);
    }

    public void StartSequence()
    {
        //        StartCoroutine(Sequence());
        if (IsExceptionStratNum(stratNum))
        {
            vplayer.SetDirectAudioMute(0, false);
            vplayer.GetTargetAudioSource(0).mute = false;

            videoPlane.GetComponent<Renderer>().material.mainTexture = videoMaterial;
            vplayer.Play();
        }
        else
        {
            vplayer.SetDirectAudioMute(0, true);
            vplayer.GetTargetAudioSource(0).mute = true;
            videoPlane.GetComponent<Renderer>().material.mainTexture = exceptionSplashScreens[stratNum];
        }
    }
    /*
    //Main sequence
    IEnumerator Sequence()
    {
        waitTime = 0;
        if (IsExceptionStratNum(stratNum))
        {
            vplayer.SetDirectAudioMute(0, false);
            vplayer.GetTargetAudioSource(0).mute = false;

            videoPlane.GetComponent<Renderer>().material.mainTexture = videoMaterial;
            vplayer.Play();
            while (waitTime < 2.0f || paused)
            {

                waitTime += Time.deltaTime;

                yield return null;
            }
            waitTime = 0;
            vplayer.Pause();
        }
        else
        {
            vplayer.SetDirectAudioMute(0, true);
            vplayer.GetTargetAudioSource(0).mute = true;
            videoPlane.GetComponent<Renderer>().material.mainTexture = exceptionSplashScreens[stratNum];
        }
        //turn off the heatmap if it's active
        //ToggleHeatMap(false);
        //Wait blocks are used for pausing in the middle of the sequence
        while (waitTime < 3.0f || paused)
        {

            waitTime += Time.deltaTime;

            yield return null;
        }
        waitTime = 0;

        //after 5 secs, show heatmap
        //ToggleHeatMap(true);

        while (waitTime < 5.0f || paused)
        {

            waitTime += Time.deltaTime;

            yield return null;
        }
        waitTime = 0;

        //after 7 secs, show video
            vplayer.Play();
            videoStartedForCurrentSequence = true;


        while (waitTime < 5.0f || paused)
        {

            waitTime += Time.deltaTime;

            yield return null;
        }
        waitTime = 0;

        //5 secs after video starts, turn off heatmap
        //if(heatMapButton.heatmapIsOn)
            //ToggleHeatMap(false);

        yield return null;
        
    }
    */
    public void Pause()
    {
        if(vplayer.isPlaying)
            vplayer.Pause();
        paused = true;
        TogglePlayPause(paused);
        Debug.Log("Video is paused.");
    }

    //called only once, from the inital "forward/next" button press
    public void Play()
    {
        StartSequence();
        paused = false;
        TogglePlayPause(paused);
        Debug.Log("Video is playing.");
    }

    public void Resume()
    {
        if(videoStartedForCurrentSequence)
            vplayer.Play();
        paused = false;
        TogglePlayPause(paused);
    }

    //TODO: rewrite these scripts so that they are explicit about what shelf to go to, probably going to end up removing next and previous since the buttons will call an rpc to explicitly say "currentSequence-1" or "currentsequence+1" or something
    public void Next()
    {
        StopAllCoroutines();
        videoStartedForCurrentSequence = false;
        Resume();
        if (vplayer.isPlaying)
            vplayer.Stop();
        CycleStrategy(true);
       // StartSequence();
    }

    public void Previous()
    {
        StopAllCoroutines();
        videoStartedForCurrentSequence = false;
        Resume();
        CycleStrategy(false);
      //  StartSequence();
    }

    public void ChangeStrategy(int i)
    {
        //i is the strategy that we are planning on changing to
        StopAllCoroutines();
        videoStartedForCurrentSequence = false;
        Resume();
        if (vplayer.isPlaying)
            vplayer.Stop();
        heatMaps[stratNum].SetActive(false);
        //turn this script into an explicit cast as well
        // GeneralManager.Instance.stratMan.SwitchStrats(true);
        stratNum = i;
        if (stratNum > strategyObjs.Length - 1)
            stratNum = 0;
        else if (stratNum < 0)
            stratNum = strategyObjs.Length - 1;

        GeneralManager.Instance.stratMan.SwitchStrats(stratNum);

        vplayer.clip = clips[stratNum];
        //labelPanelRenderer.material.mainTexture = labelTexts[stratNum];
        stratName.text = labelTexts[stratNum].name;
        //strategyObjs[stratNum].SetActive(true);
        heatMapButton.heatmapIsOn = false;
        heatMapButton.GetComponent<Renderer>().materials[1].mainTexture = heatmapOn;
        TogglePlayPause(true);
    }

    public void TogglePlayPause(bool isPaused)
    {
        playButton.SetActive(isPaused);
        pauseButton.SetActive(!isPaused);
    }

    public void ToggleHeatMap(bool toggleOn)
    {
        if (toggleOn)
        {
            if (IsExceptionStratNum(stratNum))
            {
                heatMapButton.heatmapIsOn = true;
                heatMaps[stratNum].SetActive(true);
                heatMaps[stratNum].GetComponent<Fader>().Fade(true);
                heatMapButton.GetComponent<Renderer>().materials[1].mainTexture = heatmapOff;
            }
        }
        else
        {
            heatMapButton.heatmapIsOn = false;
            /*if (heatMaps[stratNum].activeSelf == true)
            {
                heatMaps[stratNum].GetComponent<Fader>().Fade(false);
            }*/
            heatMaps[stratNum].SetActive(false);
            heatMapButton.GetComponent<Renderer>().materials[1].mainTexture = heatmapOn;

        }
    }

    public void CycleStrategy(bool forward)
    {
        //strategyObjs[stratNum].SetActive(false);
        heatMaps[stratNum].SetActive(false);

        if (forward)
        {
            stratNum += 1;
            GeneralManager.Instance.stratMan.SwitchStrats(true);

        }
        else
        {
            stratNum -= 1;
            GeneralManager.Instance.stratMan.SwitchStrats(false);
        }

        if (stratNum > strategyObjs.Length - 1)
            stratNum = 0;
        else if (stratNum < 0)
            stratNum = strategyObjs.Length - 1;

        vplayer.clip = clips[stratNum];
        //labelPanelRenderer.material.mainTexture = labelTexts[stratNum];
        stratName.text = labelTexts[stratNum].name;
        //strategyObjs[stratNum].SetActive(true);
        heatMapButton.heatmapIsOn = false;
        heatMapButton.GetComponent<Renderer>().materials[1].mainTexture = heatmapOn;
        TogglePlayPause(true);

    }

    void DetermineTPLocOrigin()
    {
        foreach (GameObject go in TPLocationParents)
        {
            if (go.activeSelf == true)
            {
                if (go.name.Contains("20"))
                {
                    origin = origin20ft;
                }
                else
                    origin = origin10ft;
            }
        }
    }

    IEnumerator DelayForSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
    }

    bool IsExceptionStratNum(int num)
    {
        if (num < 4)
        {
            return true;
        }
        else
            return false;
    }

}
