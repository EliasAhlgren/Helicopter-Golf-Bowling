﻿using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameManagement.Menu
{

   /// <summary>
   /// controls how many players can join and whether easymode is on. Uses playerPrefs to carry values to scenes.
   /// PlayerPrefs are always reseted when menu scen starts
   /// </summary>
    public class HostingUI : MonoBehaviour
   {
       public TMP_InputField nameField;
        
        float animationExitTimer = -1;
        public int capacity = 1;

        public TextMeshProUGUI textMeshProUGUI;


        private void Start()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("EasyMode", 0);
            nameField.characterLimit = 10;
        }

        public void ChangeEasyMode(Toggle ugui)
        {
            PlayerPrefs.SetInt("EasyMode", ugui.isOn ? 1 : 0);
        }
        
        public void ModifyCapacity(bool add)
        {
            // capacity starts at 0
            
            if (add && capacity >= 1 && capacity < 3)
            {
                capacity++;
            }
            else if (!add && capacity > 1 && capacity <= 3)
            {
                capacity--;
            }

            textMeshProUGUI.text = (capacity + 1).ToString();
        }



        private void Update()
        {
            if (animationExitTimer >= 0)
            {
                animationExitTimer += Time.deltaTime;
                if (animationExitTimer >= 2.5)
                {
                    SceneManager.LoadScene(1);
                }
            }

        }

        public void StartServer()
        {
            GameObject.Find("crashBuilding").transform.position = new Vector3(GameObject.Find("crashBuilding").transform.position.x, 0, GameObject.Find("crashBuilding").transform.position.z);
            animationExitTimer = 0;
            PlayerPrefs.SetString("PlayerName", nameField.text);
            PlayerPrefs.SetString("IsOffline", "false");
            PlayerPrefs.SetInt("ShouldStartClient", 0);
            PlayerPrefs.SetInt("Capacity", capacity);
            
        }
    }
}