using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement.Menu
{
    public class ClientUI : MonoBehaviour
    {
        float animationExitTimer = -1;

        public TMP_InputField _inputField;
        public TMP_InputField nameField;
        public IpValidator character;
        
        // Start is called before the first frame update
        void Start()
        {

            PlayerPrefs.DeleteAll();
            _inputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
            _inputField.inputValidator = character;

            nameField.characterLimit = 10;
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

        public void AttemptJoin()
        {
            if (nameField.text.Length > 1)
            {
                animationExitTimer = 0;
                GameObject.Find("crashBuilding").transform.position = new Vector3(GameObject.Find("crashBuilding").transform.position.x, 0,
                    GameObject.Find("crashBuilding").transform.position.z);

                PlayerPrefs.SetString("IsOffline", "false");
                PlayerPrefs.SetInt("ShouldStartClient", 1);
                PlayerPrefs.SetString("HostIp", _inputField.text);
                PlayerPrefs.SetString("PlayerName", nameField.text);
            }
            
            
        }
    
    }
}


