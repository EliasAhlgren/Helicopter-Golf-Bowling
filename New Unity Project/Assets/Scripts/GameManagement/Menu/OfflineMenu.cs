using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace GameManagement.Menu
{

    public class OfflineMenu : MonoBehaviour
    {
        float animationExitTimer = -1;
        

        public int capacity = 2;

        public TextMeshProUGUI textMeshProUGUI;

        private void Start()
        {
            PlayerPrefs.DeleteAll();
        }

        private void Update()
        {
            if(animationExitTimer >= 0)
            {
                animationExitTimer += Time.deltaTime;
                if(animationExitTimer >= 2.5)
                {
                    SceneManager.LoadScene(1);
                }
            }

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
        
        public void StartGame(Toggle toggle)
        {
            animationExitTimer = 0;
            GameObject.Find("crashBuilding").transform.position = new Vector3(GameObject.Find("crashBuilding").transform.position.x, 0, GameObject.Find("crashBuilding").transform.position.z);


            PlayerPrefs.SetString("IsOffline", "true");
            PlayerPrefs.SetInt("EasyMode", toggle.isOn ? 1 : 0);
            PlayerPrefs.SetInt("ShouldStartClient", 3);
            PlayerPrefs.SetInt("ShouldStartHost", 3);
            PlayerPrefs.SetInt("Capacity", capacity);
            
        }
    }
}