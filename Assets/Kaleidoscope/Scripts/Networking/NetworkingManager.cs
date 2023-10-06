using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    public int connectAttempts;

    #region Public Fields

    static public NetworkingManager Instance;

    #endregion

    #region Private Fields

    [SerializeField]
    private GameObject playerPrefab;

    public bool testingRoom = false;
    

    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool isConnecting;

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "2";

    #endregion

    // Use this for initialization
    void Start()
    {
        Instance = this;

        Connect();

        /* // in case we started this demo with the wrong scene being active, simply load the menu scene
         if (!PhotonNetwork.IsConnected)
         {
             //SceneManager.LoadScene("PunBasics-Launcher");

             return;
         }*/

        //Connect();

        /*if (playerPrefab == null)
        { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

            Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {


            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {

                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }*/
    }

    #region Public Methods

    /// <summary>
    /// Start the connection process. 
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        isConnecting = true;

        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Joining Room...");
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("Connecting...");

            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = this.gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    #endregion


    #region MonoBehaviourPunCallbacks CallBacks
    // below, we implement some callbacks of PUN
    // you can find PUN's callbacks in the class MonoBehaviourPunCallbacks

    public override void OnConnected()
    {
        Debug.Log("Connected");
        connectAttempts = 0;
    }

    /// <summary>
    /// Called after the connection to the master is established and authenticated
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // we don't want to do anything if we are not attempting to join a room. 
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (isConnecting)
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()

            //We want a different room specifically for testing; non-testers shouldn't be able to join - FigJ
            if (testingRoom)
            {
                PhotonNetwork.JoinOrCreateRoom("Testing Room", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
                Debug.Log("Now in testing room.");
            }else if (!testingRoom)
            {
                PhotonNetwork.JoinOrCreateRoom("Client Room", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
                Debug.Log("Now in client room.");
            }
            else //in case of NRE - FigJ
                PhotonNetwork.JoinRandomRoom();
        }
    }

    /// <summary>
    /// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
    /// </summary>
    /// <remarks>
    /// Most likely all rooms are full or no rooms are available. <br/>
    /// </remarks>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
        //create the test room if it doesn't exist for whatever reason - FigJ
        if (testingRoom)
        {
            PhotonNetwork.CreateRoom("Testing Room", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
            Debug.Log("Created testing room.");
        }

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        if (!testingRoom)
        {
            PhotonNetwork.CreateRoom("Client Room", new RoomOptions { MaxPlayers = 10 });
            Debug.Log("Created client room.");
        }
        else //in case of NRE, but we also don't want testing or client meetings getting interrupted - FigJ
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 5 }, TypedLobby.Default);
    }


    /// <summary>
    /// Called after disconnecting from the Photon server.
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");
        isConnecting = false;
        //TODO: figure out which disconnect causes should reconnect and which shouldn't
        if (connectAttempts < 3)
        {
            Invoke("Connect", 5);
            connectAttempts++;
        }
        else
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
    /// </summary>
    /// <remarks>
    /// This method is commonly used to instantiate player characters.
    /// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
    ///
    /// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
    /// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
    /// enough players are in the room to start playing.
    /// </remarks>
    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

        //PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(-0.9f, 1.6f, 0f), Quaternion.identity, 0);
        PhotonNetwork.Instantiate(this.playerPrefab.name, GeneralManager.Instance.playerSpawn.position, Quaternion.identity, 0);
    }

    #endregion

}
