
//using ExitGames.Demos.DemoPunVoice;
using Photon.Voice;
using System.Collections;
using System.Collections.Generic;
using Oculus.Platform.Models;
using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class LaserPointerController : MonoBehaviour
{
    public LineRenderer laser;
    public Transform particlePoint;

    public float maxDistance = 100f;
    public GameObject selectedObject = null;
    public GameObject grabbedObject = null;
    public LayerMask Mask;
    public LayerMask Teleportation;
    public LayerMask UI;
    public LayerMask Shelf;

    public Transform teleport = null;
    public Transform player;
    public Transform playerCam;

    public SplashScreenFade faderCanvas;

    public bool leaderIsHoldingObject = false;
    private bool isTheLeader;
    private Recorder muteMic;
    private float timePassed;
    public bool delay = false;
    private float heldThreshold = 1.0f;

    public SpriteRenderer muteGuide;
    public List<Sprite> muteGuideToggle;

    [SerializeField]
    public List<GameObject> deskHints;

    private bool deskHintsEnabled = true;

    UserManager userManager;


    float userHeight;

    //public MeshRenderer teleportHint;

    //public SpriteRenderer mutedUser;
    //public SpriteRenderer mutedUserOverhead;

    //private StrategiesManager stratMan;

    // Start is called before the first frame update
    void Start()
    {
        userManager = GetComponentInParent<UserManager>();
        isTheLeader = player.GetComponent<UserManager>().isLeader;
        muteMic = GetComponentInParent<VoiceConnection>().PrimaryRecorder;
        this.transform.localPosition = new Vector3(0.1f, 0, 0.31f);
    }

    public GameObject MoveCollider(GameObject orig, GameObject endResult)
    {
        if (endResult.GetComponent<BoxCollider>() == null)
        {
            endResult.AddComponent<BoxCollider>();
            endResult.GetComponent<BoxCollider>().center = orig.GetComponent<BoxCollider>().center;
            endResult.GetComponent<BoxCollider>().size = orig.GetComponent<BoxCollider>().size;
        }
        //orig.GetComponent<BoxCollider>().enabled = false;
        return endResult;
    }

    // Update is called once per frame
    void Update()
    {
        float rayDistance = maxDistance;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance,
            Shelf))
        {
            rayDistance = hit.distance;
            if (hit.collider.gameObject != selectedObject && selectedObject != null)
                selectedObject.SendMessage("UnSelect", SendMessageOptions.DontRequireReceiver);

            selectedObject = hit.collider.gameObject;
            selectedObject.SendMessage("Select", SendMessageOptions.DontRequireReceiver);
        }

        if (grabbedObject != null)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Z))
            {
                //rotate
                //TODO: have this speec be 0.2 on quest and editor, and 0.1 on desktop
                userManager.photonView.RPC("StartRotation", Photon.Pun.RpcTarget.AllViaServer, true);
            }
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.X))
            {
                userManager.photonView.RPC("StartRotation", Photon.Pun.RpcTarget.AllViaServer, false);
            }
            if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch) || Input.GetKeyUp(KeyCode.X) || OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch) || Input.GetKeyUp(KeyCode.Z))
            {
                //Release
                userManager.photonView.RPC("StopRotation", Photon.Pun.RpcTarget.AllViaServer);
            }
            //Previous method
            /*
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch) || Input.GetKey(KeyCode.Z))
            {
                //rotate
                //TODO: have this speec be 0.2 on quest and editor, and 0.1 on desktop
                grabbedObject.transform.GetChild(0).Rotate(new Vector3(0, 0.1f, 0));
            }
            else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch) || Input.GetKey(KeyCode.X))
            {
                grabbedObject.transform.GetChild(0).Rotate(new Vector3(0, -0.1f, 0));
            }*/
        }

        if (!leaderIsHoldingObject || player.GetComponent<UserManager>().isLeader)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance, Mask))
            {
                rayDistance = hit.distance;
                if (hit.collider.gameObject != selectedObject && selectedObject != null)
                    selectedObject.SendMessage("UnSelect", SendMessageOptions.DontRequireReceiver);

                selectedObject = hit.collider.gameObject;
                selectedObject.SendMessage("Select", SendMessageOptions.DontRequireReceiver);

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Space))
                {
                    if(selectedObject.GetComponent<SelectableProduct>() == null)
                    {
                        selectedObject = MoveCollider(selectedObject, selectedObject.transform.parent.parent.gameObject);
                    }
                    grabbedObject = selectedObject.GetComponent<SelectableProduct>().Duplicate();
                    maxDistance = 0.01f;
#if UNITY_EDITOR || UNITY_STANDALONE
                    this.gameObject.transform.localEulerAngles = Vector3.zero;
#endif
                    userManager.rightCont.gameObject.SetActive(false);
                    userManager.heldObject = grabbedObject;
                    grabbedObject.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                    userManager.photonView.RPC("SetProductToHand", Photon.Pun.RpcTarget.Others, grabbedObject.name);

                    if (player.GetComponent<UserManager>().isLeader)
                    {
                        Debug.Log("Grabbed object name is " + grabbedObject.name);
                        userManager.photonView.RPC("PickupObjectFromLeader", Photon.Pun.RpcTarget.AllViaServer, grabbedObject.name);
                    }
                }
            }
            else
            {
                if (selectedObject != null)
                {
                    selectedObject.SendMessage("UnSelect", SendMessageOptions.DontRequireReceiver);
                    selectedObject = null;
                }
            }

            if (grabbedObject != null)
            {
                if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || Input.GetKeyUp(KeyCode.Space))
                {
                    userManager.rightCont.gameObject.SetActive(true);
                    if (player.GetComponent<UserManager>().isLeader)
                    {
                        userManager.photonView.RPC("DropObjectFromLeader", Photon.Pun.RpcTarget.AllViaServer);
                    }
                    else
                    {
                        Debug.LogError("Deleting object at line 183");
                        grabbedObject.SendMessage("DestroyMe", SendMessageOptions.DontRequireReceiver);
                    userManager.heldObject = null;
                        maxDistance = 100f;
                        userManager.photonView.RPC("SetProductToShelf", Photon.Pun.RpcTarget.AllViaServer);
                    }
#if UNITY_EDITOR || UNITY_STANDALONE
                    if (!GeneralManager.Instance.interactableBuild)
                    {
                        //Force the controller's x rotation back to 0
                        this.gameObject.transform.localEulerAngles = Vector3.zero;
                    }
#endif
                }
            }
        }



        //teleportation will be controlled here; headset ver only - FigJ
        if (GeneralManager.Instance.interactableBuild)
        {
            if (delay)
            {
                timePassed += Time.deltaTime;
                if (timePassed >= heldThreshold)
                {
                    timePassed = 0;
                    delay = false;
                }
            }
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance, Teleportation))
            {
                rayDistance = hit.distance;
                if (hit.collider.gameObject != selectedObject && selectedObject != null)
                    selectedObject.SendMessage("UnSelect", SendMessageOptions.DontRequireReceiver);

                selectedObject = hit.collider.gameObject;
                selectedObject.SendMessage("Select", SendMessageOptions.DontRequireReceiver);
                if (selectedObject.GetComponent<SelectableTeleport>() != null) // Added by aman
                { 
                userHeight = selectedObject.GetComponent<SelectableTeleport>().height;
                teleport = selectedObject.transform;
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || Input.GetKeyDown(KeyCode.Space))
                    faderCanvas.GetComponentInParent<Animator>().Play("fade");
                }

            }

            laser.SetPosition(1, Vector3.forward * rayDistance);
            if (particlePoint != null)
                particlePoint.localPosition = Vector3.forward * rayDistance;
            //becoming leader is limited to headset users only -- FigJ
            // functionality for changing if you're a leader or not-- basically, by pressing the button, you toggle your leadership status
            if (player.GetComponent<UserManager>().isLeader)
            {
                if (!delay && ((OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && OVRInput.Get(OVRInput.RawButton.B)) || (Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.K))))
                    ToggleLeadership();
                else if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.L))
                {
                    ToggleLoad();
                    Invoke("LeaderSwitchShelves", 0.5f);
                    Invoke("ToggleLoad", 1.0f);
                    delay = false;
                }
            }
            else if (!delay && ((OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && OVRInput.Get(OVRInput.RawButton.B)) || (Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.K))))
                ToggleLeadership();
        }
        laser.SetPosition(1, Vector3.forward * rayDistance);
        if (particlePoint != null)
            particlePoint.localPosition = Vector3.forward * rayDistance;

        if (Input.GetKeyDown(KeyCode.M) || OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (muteMic == null)
            {
                muteMic = GetComponentInParent<VoiceConnection>().PrimaryRecorder;
            }
            muteMic.IsRecording = !muteMic.IsRecording;
            userManager.photonView.RPC("ToggleMutedUser", RpcTarget.AllBuffered);
        }
    }

    public void PickupObjectFromLeader(GameObject g)
    {
        if (!GeneralManager.Instance.isPresenterMode && !userManager.isLeader)
        {
            Debug.Log("Picking up object from leader!");

            userManager.rightCont.gameObject.SetActive(false);
            if (!player.GetComponent<UserManager>().isInteractableUser)
                return;
            leaderIsHoldingObject = true;
            if (grabbedObject == null && GetComponentInChildren<ProductDoppelGanger>() != null)
                grabbedObject = GetComponentInChildren<ProductDoppelGanger>().gameObject;
            
            // drop the object!
                        Debug.LogError("Deleting object at line 279");
            if (grabbedObject != null)
                Destroy(grabbedObject);
            if (selectedObject != null)
                selectedObject.SendMessage("UnSelect", SendMessageOptions.DontRequireReceiver);

            // if (g.GetComponent<SelectableProduct>() == null)
            //  {
            //      selectedObject = MoveCollider(selectedObject);
            //  }

            if (g.GetComponentInParent<ProductDoppelGanger>() != null)
            {
                g = MoveCollider(g, g.GetComponentInParent<ProductDoppelGanger>().gameObject);
            }else if(g.GetComponent<BoxCollider>() == null && g.GetComponentInChildren<BoxCollider>() != null)
            {
                g = MoveCollider(g.GetComponentInChildren<BoxCollider>().gameObject, g);
            }

            grabbedObject = g.GetComponent<SelectableProduct>().Duplicate();
            maxDistance = 0.01f;
#if UNITY_EDITOR || UNITY_STANDALONE
            this.gameObject.transform.localEulerAngles = Vector3.zero;
#endif
            userManager.photonView.RPC("SetProductToHand", Photon.Pun.RpcTarget.Others, grabbedObject.name);
        }
    }

    public void DropLeaderObject()
    {
        if (!player.GetComponent<UserManager>().isInteractableUser || grabbedObject == null)
            return;
        userManager.rightCont.gameObject.SetActive(true);
        leaderIsHoldingObject = false;
                        Debug.LogError("Deleting object at line 313");
        grabbedObject.SendMessage("DestroyMe", SendMessageOptions.DontRequireReceiver);
        userManager.photonView.RPC("SetProductToShelf", Photon.Pun.RpcTarget.AllViaServer);
        selectedObject = null;
        grabbedObject = null;
        maxDistance = 100f;
#if UNITY_EDITOR || UNITY_STANDALONE
        //Force the controller's x rotation back to 0
        this.gameObject.transform.localEulerAngles = Vector3.zero;
#endif
    }

    void StopVibration()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    void ToggleLoad()
    {
        SpriteRenderer loadShelf = userManager.loadTemp;
        loadShelf.enabled = !loadShelf.enabled;
    }

    void ToggleLeadership()
    {
        delay = true;
        timePassed = 0;
        if (LeaderManager.Instance.leaderExists == player.gameObject)
        {
            userManager.photonView.RPC("MakeClientNotLeader", Photon.Pun.RpcTarget.AllBufferedViaServer);
            Debug.Log("There is no leader.");
        }
        else if (LeaderManager.Instance.leaderExists == null)
        {
            userManager.photonView.RPC("MakeClientLeader", Photon.Pun.RpcTarget.AllBufferedViaServer);
            Debug.Log("Leader is now " + LeaderManager.Instance.leaderExists.name);
        }
        else
            Debug.Log("Leader is still " + LeaderManager.Instance.leaderExists.name);
    }

    void LeaderSwitchShelves()
    {
        //Commenting out for this because there's only one strategy...
         userManager.photonView.RPC("SwitchShelves", Photon.Pun.RpcTarget.AllBufferedViaServer, GeneralManager.Instance.stratMan.currentStrat + 1);
    }

    public void FadeTeleport()
    {
        Vector3 distanceToChange = new Vector3(
            player.position.x - playerCam.position.x,
            player.position.y,
            player.position.z - playerCam.position.z
        );
        player.position = new Vector3(teleport.position.x + distanceToChange.x,
            userHeight,
            teleport.position.z + distanceToChange.z
        );
        Debug.Log("Moved player!");

        teleport = null;
        Debug.Log("Teleport: " + teleport);
    }
}
