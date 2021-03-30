using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement.Menu
{
    public class ClientUI : MonoBehaviour
    {

        public TMP_InputField _inputField;
        public IpValidator character;
        
        // Start is called before the first frame update
        void Start()
        {

            PlayerPrefs.DeleteAll();
            _inputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
            _inputField.inputValidator = character;
        }

        public void AttemptJoin()
        {
            PlayerPrefs.SetString("IsOffline", "false");
            PlayerPrefs.SetInt("ShouldStartClient", 1);
            PlayerPrefs.SetString("HostIp", _inputField.text);
            SceneManager.LoadScene(1);
        }
    
    }
}


