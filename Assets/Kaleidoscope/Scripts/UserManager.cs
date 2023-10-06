using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using TMPro;
using Oculus.Platform.Models;

public class UserManager : MonoBehaviourPunCallbacks
{
    // 🕷 <- spider to take care of any bugs
    public GameObject playerMesh;
    public GameObject headsetUserMesh;
    public GameObject deskUserMesh;

    public GameObject rightHand;
    public MeshRenderer rightCont;
    public LaserPointerController laserController;
    public Camera centerCamera;

    public string selectedObject;
    private Vector3 origPos;
    private Vector3 origAngle;

    public GameObject heldObject;
    public bool isLeader;
    public bool isInteractableUser;
    public SpriteRenderer leaderTag;
    public SpriteRenderer leaderSelfTag;
    private MeshRenderer controllerCol;
    public Texture defColor;
    public Texture leaderColor;
    public AvatarPalette pickedAc;
    public TextMeshPro userId;
    public PhotonVoiceView playerMic;
    public VoiceConnection voiceCon;
    public SpriteRenderer loadTemp;

    private Recorder muteMic;
    public SpriteRenderer mutedUser;
    public SpriteRenderer mutedUserOverhead;
    public DesktopMicMenu micMenu;
    
    public bool viewDebug = false;
    
    
    public List<GameObject> controllerObjects;
    //this is used to pick what color from the array they should be
    public int userDisplayID = 1;
    public int userColorID = 0;

    void Start()
    {
        loadTemp.enabled = false;
        muteMic = voiceCon.PrimaryRecorder;
        micMenu = GeneralManager.Instance.micMenu;

        InitializePlayer();
    }

    public void InitializePlayer()
    {
        if (photonView.IsMine)
        {
#if UNITY_ANDROID
            GeneralManager.Instance.interactableBuild = true;
            //Quest mic wants to be contrarian to the mute function, so force it to start recording with the correct mic.
            Recorder.PhotonMicrophoneEnumerator.Refresh();
            muteMic.PhotonMicrophoneDeviceId = Recorder.PhotonMicrophoneEnumerator.IDAtIndex(0);
            muteMic.RestartRecording();
            muteMic.IsRecording = true;
            micMenu.gameObject.SetActive(false);
#else
            micMenu.gameObject.SetActive(true);

#endif
            if (!GeneralManager.Instance.interactableBuild)
            {
                gameObject.GetComponent<VRTK.SDK_InputSimulator>().enabled = true;
                laserController.maxDistance = 0.01f;
                photonView.RPC("SetPlayerMesh", RpcTarget.AllBuffered, true);
                GeneralManager.Instance.debugAndInstructions.renderMode = RenderMode.WorldSpace;
            }
            else
            {
                photonView.RPC("SetPlayerMesh", RpcTarget.AllBuffered, false);
                GeneralManager.Instance.debugAndInstructions.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            GeneralManager.Instance.debugAndInstructions.gameObject.SetActive(NetworkingManager.Instance.testingRoom);
            gameObject.AddComponent<OVRCameraRig>();
            gameObject.AddComponent<OVRManager>();
            rightHand.AddComponent<AnchorFlag>();

            GeneralManager.Instance.selectingPlayerRoot = transform;

            playerMic = GetComponent<PhotonVoiceView>();
            voiceCon.PrimaryRecorder = playerMic.RecorderInUse;
        }
        else
        {
            Destroy(centerCamera);
            Destroy(laserController);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            rightHand.AddComponent<LaserControllerNetworkedUser>();
            gameObject.GetComponent<VRTK.SDK_InputSimulator>().enabled = false;
        }
        //grabbing color here instead of AvatarColor - FigJ
        pickedAc = GeneralManager.Instance.GetComponent<AvatarPalette>();
        leaderTag = GetComponentInChildren<SpriteRenderer>(); //indicates who is leader - FigJ
        leaderSelfTag = rightHand.GetComponentInChildren<SpriteRenderer>(); //player can see if self is leader or not - FigJ
        controllerCol = rightHand.GetComponentInChildren<MeshRenderer>(); //to change color when leader - FigJ
        if (userId == null)
            userId = GetComponentInChildren<TextMeshPro>();
        userId.text = UpdateUserName();

        playerMesh.GetComponent<Renderer>().material.color = pickedAc.PickColor(userColorID);
        rightCont.material.color = pickedAc.PickColor(userColorID);
        //I don't like this being hardcoded but it does seem to work,,
        if (photonView.IsMine && isInteractableUser && LeaderManager.Instance.leaderExists == null)
            photonView.RPC("MakeClientLeader", RpcTarget.AllBufferedViaServer);
        else if (GeneralManager.Instance.isPresenterMode)
        {
            this.transform.parent = LeaderManager.Instance.leaderExists.GetComponent<UserManager>().deskUserMesh.transform.parent;
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.Euler(Vector3.zero);
            //hide controller
            userId.gameObject.SetActive(false);
            if(!isLeader)
                rightHand.SetActive(false);
        }
    }

    [PunRPC]
    void SetPlayerMesh(bool isCameraman, PhotonMessageInfo info)
    {
        // Defined here so the networked users know if a user is supposed to be interactable, 
        if (GeneralManager.Instance.isPresenterMode)
        {
            headsetUserMesh.SetActive(false);
            deskUserMesh.SetActive(false);
            playerMesh = headsetUserMesh;
        }
        {
            isInteractableUser = !isCameraman;
            if (!isInteractableUser)
            {
                playerMesh = deskUserMesh;
                headsetUserMesh.SetActive(false);
                //'rightHand' must remain active in order for inputs like mute to work.
                //But you can disable its mesh renderer and all child objects. - FigJ.
                rightHand.GetComponent<LineRenderer>().enabled = false;

                foreach (GameObject o in controllerObjects)
                    o.SetActive(false);
                //if it is NOT a rift build, we want this menu active
            }
            else
            {
                playerMesh = headsetUserMesh;
                deskUserMesh.SetActive(false);
            }
        }
        
    }

    [PunRPC]
    void SetProductToHand(string name, PhotonMessageInfo info)
    {
        {
                rightHand.GetComponent<LaserControllerNetworkedUser>().DisableAndEnableLine(false);
                //Name = unique name of product grabbed , Info = will contain ability to locate prefab that should be holding the item, Needs to set selectedObject to the object's name
                rightCont.gameObject.SetActive(false);

                if (!string.IsNullOrEmpty(selectedObject))
                {
                    //Make sure the user isn't alread holding something
                    Debug.Log("User is already holding something!!!!!!");
                    //it might be good to check if the value is different from name, which will let you know that there is a new object that needs to be deleted
                    //added this to possibly force the change to everyone
                    if (rightHand.GetComponentsInChildren<SelectableProduct>().Length > 0)
                    {
                        SelectableProduct[] p = rightHand.GetComponentsInChildren<SelectableProduct>();
                        for (int i = 0; i < p.Length; i++)
                        {
                            Destroy(p[i].gameObject);
                        }
                    }
                }
                Debug.Log("Creating " + name);
                //Find the object, using the name
                //TODO: Find a way to save the path to the products programatically, so this can be useful in the future with different shelves
                if (name.Contains("(Clone)"))
                {
                    name = name.Replace("(Clone)", "");
                }
                GameObject foundObject = GameObject.Find(GeneralManager.Instance.stratMan.strategiesPath[GeneralManager.Instance.stratMan.currentStrat] + name);
                Debug.Log("foundObject is: " + foundObject.name);
                //Make a duplicate of the object, Make it a child of rightHand, move it to the same position as righthand (maybe offset if it looks weird)
                heldObject = Instantiate(foundObject, rightHand.transform.position,
                    foundObject.transform.localRotation, rightHand.transform);
            heldObject.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                heldObject.GetComponent<SelectableProduct>().enabled = false;
                heldObject.GetComponent<Outline>().enabled = false;
                selectedObject = name;
        }
    }

    [PunRPC]
    void SetProductToShelf(PhotonMessageInfo info)
    {
        //if (isLeader || !GeneralManager.Instance.isPresenterMode)
        {
                //Info = will contain ability to locate prefab that should be holding the item
                if (string.IsNullOrEmpty(selectedObject))
                    Debug.Log("The selected object is null, but we're trying to drop something");
                else
                {
                    Debug.Log("Destroying " + selectedObject);
                    //Destroy the duplicate
                    Destroy(heldObject);
                    selectedObject = "";
                }
                if (rightHand.GetComponentsInChildren<SelectableProduct>().Length > 0)
                {
                    SelectableProduct[] p = rightHand.GetComponentsInChildren<SelectableProduct>();
                    for (int i = 0; i < p.Length; i++)
                    {
                        Destroy(p[i].gameObject);
                    }
                }
                if (!photonView.IsMine)
                {
                    rightHand.GetComponent<LaserControllerNetworkedUser>().DisableAndEnableLine(true);
                    rightCont.gameObject.SetActive(true);
                }
        }
    }   

    [PunRPC]
    private void MakeClientLeader()
    {
        isLeader = true;
        leaderTag.enabled = true;
        leaderSelfTag.enabled = true;
        LeaderManager.Instance.leaderExists = this.gameObject;
    }
    [PunRPC]
    private void MakeClientNotLeader()
    {
        isLeader = false;
        leaderTag.enabled = false;
        leaderSelfTag.enabled = false;
        LeaderManager.Instance.leaderExists = null;
    }

    [PunRPC]
    public void PickupObjectFromLeader(string objName, PhotonMessageInfo info)
    {
        //if (!GeneralManager.Instance.isPresenterMode || isLeader)
        {
            Debug.Log("PickupObjectFromLeader objName is " + objName);
            if (objName.Contains("(Clone)"))
            {
                objName = objName.Replace("(Clone)", "");
            }
            Debug.Log("path is " + GeneralManager.Instance.stratMan.strategiesPath[GeneralManager.Instance.stratMan.currentStrat] + objName);
            GameObject foundObject = GameObject.Find(GeneralManager.Instance.stratMan.strategiesPath[GeneralManager.Instance.stratMan.currentStrat] + objName);

            Debug.Log("foundObject is: " + foundObject.name);
            if (foundObject != null)
            {
                Debug.Log("Found object is NOT null!");
                GeneralManager.Instance.selectingPlayerRoot.GetComponent<UserManager>().rightHand.GetComponent<LaserPointerController>().PickupObjectFromLeader(foundObject);
            }
        }
    }
    [PunRPC]
    public void DropObjectFromLeader(PhotonMessageInfo info)
    {
        if (!GeneralManager.Instance.isPresenterMode || isLeader)
        {
            Debug.Log("DropObjectFromLeader");
            GeneralManager.Instance.selectingPlayerRoot.GetComponent<UserManager>().rightHand.GetComponent<LaserPointerController>().DropLeaderObject();
        }
    }

    [PunRPC]
    private void SwapContCol() //change the controller color when leader; set in Inspector - FigJ
    {
        //using textures instead of materials so as to not deal with multiple mats/shaders for optimization.
        if (leaderSelfTag.enabled)
            controllerCol.material.SetTexture("_MainTex", leaderColor);
        else
            controllerCol.material.SetTexture("_MainTex", defColor);
    }

    [PunRPC]
    public void PlayButton(int currentStrat)
    {
        if (currentStrat != GeneralManager.Instance.stratMan.currentStrat)
        {
            SequenceManager.Instance.ChangeStrategy(currentStrat);
        }
        SequenceManager.Instance.Play();
    }
    [PunRPC]
    public void PauseButton(int currentStrat)
    {
        if (currentStrat != GeneralManager.Instance.stratMan.currentStrat)
        {
            SequenceManager.Instance.ChangeStrategy(currentStrat);
        }
        SequenceManager.Instance.Pause();
    }
    [PunRPC]
    public void ChangeStrategy(int i)
    {
        SequenceManager.Instance.ChangeStrategy(i);
    }
    [PunRPC]
    public void HeatmapButton(int currentStrat)
    {
        if(currentStrat != GeneralManager.Instance.stratMan.currentStrat)
        {
            SequenceManager.Instance.ChangeStrategy(currentStrat);
        }
        SequenceManager.Instance.heatMapButton.DoAction();
    }

    bool isRotating = false;
    bool rotatingLeft = false;

    [PunRPC]
    public void StartRotation (bool isRotatingLeft)
    {
        isRotating = true;
        //Starts or swaps what direction it is rotating
        if (isRotatingLeft)
        {
            rotatingLeft = true;
        }
        else
        {
            rotatingLeft = false;
        }
    }

    [PunRPC]
    public void StopRotation()
    {
        //When rotating stops alltogether
        isRotating = false;
    }

    private void Update()
    {
        if (muteMic == null && photonView.IsMine)
        {
            voiceCon.PrimaryRecorder = GameObject.Find("PhotonRecorder").GetComponent<Recorder>();
            playerMic.RecorderInUse.Init(voiceCon);
            muteMic = voiceCon.PrimaryRecorder;
        }
        if (userId == null)
        {
            userId = GetComponentInChildren<TextMeshPro>();
            userId.text = UpdateUserName();
        }
        //Disabling for now because It seems like it was too inaccurate and we would need to really adjust how this works to work as intended I think
        //What we COULD do is on the recorder side, if the volume is above a threshold, send an rpc command to everyone to make the person's text light up and vice versa, but I'm not sure if that would be entirely worth it.
        /*
        if (this.GetComponent<Speaker>() != null && this.GetComponent<Speaker>().IsPlaying)
            ShowTalkingGlow();
        else if (this.GetComponent<Speaker>() != null && !this.GetComponent<Speaker>().IsPlaying)
            StopTalkingGlow();*/

        if (isRotating)
        {
            if (rotatingLeft)
                RotateObject(0.5f);
            else
                RotateObject(-0.5f);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Recorder status: \r\n" +
                      "initialized?: " + muteMic.IsInitialized + "\r\n" +
                      "recording?: " + muteMic.IsRecording + "\r\n" +
                      "transmit enabled?: " + muteMic.TransmitEnabled + "\r\n" +
                      "currently transmitting?: " + muteMic.IsCurrentlyTransmitting + "\r\n" +
                      "recording?: " + gameObject.GetComponent<Speaker>().IsLinked + "\r\n");
        }
        
        if (Input.GetKeyDown(KeyCode.Slash) && Input.GetKey(KeyCode.Quote))
        {
            Debug.Log("Question and quote keys pressed!");
            viewDebug = !viewDebug;
            GeneralManager.Instance.debugAndInstructions.gameObject.SetActive(viewDebug);
        }
    }

    void RotateObject(float yRotation)
    {
        heldObject.transform.GetChild(0).Rotate(new Vector3(0, yRotation, 0));

    }

    [PunRPC]
    private string UpdateUserName()
    {
        string userName = "";
        if (photonView.IsMine)
        {
            userName = PlayerPrefs.GetString("userId");
        }
        else if (userId.text != "")
        {
            userName = userId.text;
        }
        if (userName != null || userName != "")
        {
            Debug.Log("id is " + photonView.InstantiationId.ToString());
            userDisplayID = (int)((photonView.InstantiationId - 1) * 0.001f);  // rdrury - since the photonView instantiation ID's come out consistently, but at values of x001 (x being the number we actually want, like user 1, 2, etc) this is to remove the excess numbers without having to get the characters since that part was having issues when we had to add it
            userColorID = userDisplayID - 1; // rdrury - since this is for an array, need it to start at 0
            if (headsetUserMesh.activeSelf)
                userName = "Client " + userDisplayID;
            else
                //we may need to come up with a separate number/iD list for cameras; just testing this for now - FigJ    
                userName = "Camera " + userDisplayID;
        }
        return userName;
    }

    [PunRPC]
    public void SetUsername(string s)
    {
        userId.text = s;
        if (photonView.IsMine)
        {
            PlayerPrefs.SetString("userId", s);
        }
    }

    [PunRPC]
    void ShowTalkingGlow()
    {
        userId.color = Color.white;
        userId.fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, 0.15f);
        userId.fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.7f);
        userId.UpdateMeshPadding();
    }

    [PunRPC]
    void StopTalkingGlow()
    {
        userId.color = new Color(.99f, .99f, 1f, 1f);
        userId.fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, 0f);
        userId.fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0f);
        userId.UpdateMeshPadding();
    }

    [PunRPC]
    public void SwitchShelves(int i)
    {
        GeneralManager.Instance.stratMan.SwitchStrats(i);
    }

    private void OnApplicationQuit()
    {
        if (isLeader)
            photonView.RPC("LeaderDestroy", Photon.Pun.RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void LeaderDestroy()
    {
        LeaderManager.Instance.leaderExists = null;
    }
    
     [PunRPC]
    private void ToggleMutedUser()
    { 
        mutedUser.enabled = !mutedUser.enabled;
        mutedUserOverhead.enabled = !mutedUserOverhead.enabled;
    }
}