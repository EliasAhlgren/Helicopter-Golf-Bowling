using System;
using MLAPI;
using System.Net;
using UnityEngine;

public class NetManagerHud : MonoBehaviour
{

    public bool isAtStartup = true;
    
    public void StartHost()
    {
        NetworkingManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkingManager.Singleton.StartClient();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && isAtStartup)
        {
            StartHost();
            isAtStartup = false;
            return;
        }
        if (Input.GetKeyDown(KeyCode.C) && isAtStartup)
        {
            StartClient();
            isAtStartup = false;
        }
        
    }
}
