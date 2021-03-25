using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameManagement.Menu
{
    public class OfflineMenu : MonoBehaviour
    {
        public int capacity = 2;

        public TextMeshProUGUI textMeshProUGUI;
        
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
            PlayerPrefs.SetInt("EasyMode", toggle.isOn ? 1 : 0);
            PlayerPrefs.SetInt("ShouldStartClient", 3);
            PlayerPrefs.SetInt("ShouldStartHost", 3);
            PlayerPrefs.SetInt("Capacity", capacity);
            SceneManager.LoadScene("Level 1");
        }
    }
}