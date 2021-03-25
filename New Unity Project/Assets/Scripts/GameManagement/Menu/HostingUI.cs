using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameManagement.Menu
{

   
    public class HostingUI : MonoBehaviour
    {
        public int capacity = 2;

        public TextMeshProUGUI textMeshProUGUI;


        private void Start()
        {
            PlayerPrefs.SetInt("EasyMode", 0);
        }

        public void ChangeEasyMode(Toggle ugui)
        {
            PlayerPrefs.SetInt("EasyMode", ugui.isOn ? 1 : 0);
        }
        
        public void ModifyCapacity(bool add)
        {
            if (add && capacity >= 2 && capacity < 4)
            {
                capacity++;
            }
            else if (!add && capacity > 2 && capacity <= 4)
            {
                capacity--;
            }

            textMeshProUGUI.text = capacity.ToString();
        }
        
        public void StartServer()
        {
            PlayerPrefs.SetInt("ShouldStartClient", 0);
            PlayerPrefs.SetInt("Capacity", capacity);
            SceneManager.LoadScene(1);
        }
    }
}