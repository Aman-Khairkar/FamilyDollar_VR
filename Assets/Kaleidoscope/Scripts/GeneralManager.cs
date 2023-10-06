using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//using VRTK;

public class GeneralManager : Singleton<GeneralManager>
{

    public NetworkingManager networkMan;
    public StrategiesManager stratMan;
    //This script is intended to ease both ease the process of adding new objects by removing the need to add references to needed components on each project
    // and to make it easier to find some of the functionality
    //this is only for builds where there is a cart, don't worry about it for this
    /*public bool isCartBeingUsed = true;
    public GameObject cart;
    public GameObject cartTrigger;*/

    public Transform playerSpawn;
    public bool interactableBuild = false;

    public bool isPresenterMode = false;


    //things used in selectableProduct that usually are required to be placed manually
    public Transform selectingPlayerRoot;
    //   public VRTK_ControllerEvents ce;

    public DesktopMicMenu micMenu;
    
    public Canvas debugAndInstructions;

    public DataManager m_dataManager;

    public void Awake()
    {
#if UNITY_ANDROID
        networkMan.enabled = true;
#else
        networkMan.enabled = false;
        m_dataManager = this.GetComponent<DataManager>();
        networkMan = this.GetComponent<NetworkingManager>();
        m_dataManager.Load();
        Invoke("Load", 1);
#endif
    }

    public void Load()
    {
        interactableBuild = m_dataManager.config.interactableBuild;
        networkMan.testingRoom = m_dataManager.config.testingRoom;
        networkMan.enabled = true;
    }

    public void Save()
    {
        m_dataManager.config.interactableBuild = interactableBuild;
        m_dataManager.config.testingRoom = networkMan.testingRoom;
        
    }
}
