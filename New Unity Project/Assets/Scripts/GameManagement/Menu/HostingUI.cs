using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement.Menu
{

   
    public class HostingUI : MonoBehaviour
    {
        public int capacity = 2;
        
        void ModifyCapacity(bool add)
        {
            if (add && capacity >= 2)
            {
                capacity++;
            }
            else if (!add && capacity >= 2)
            {
                capacity--;
            }
        }
        
        void StartServer()
        {
            PlayerPrefs.SetInt("ShouldStartClient", 0);
            PlayerPrefs.SetInt("Capacity", capacity);
            SceneManager.LoadScene(1);
        }
    }
}