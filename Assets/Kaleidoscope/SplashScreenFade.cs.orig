using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenFade : MonoBehaviour
{
    public LaserPointerController localLPC;

    public void ActivateTeleport() //animator event
    {
        localLPC.FadeTeleport();
    }

    public void NullifyTeleport() //animator event
    {
        localLPC.teleport = null;
        Debug.Log("Teleport: " + localLPC.teleport);
    }
}
