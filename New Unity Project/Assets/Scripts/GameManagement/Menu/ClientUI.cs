using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement.Menu
{
    public class ClientUI : MonoBehaviour
    {

        public TMP_InputField _inputField;
        public CustomValidator character;
        
        // Start is called before the first frame update
        void Start()
        {
            
            _inputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
            _inputField.inputValidator = character;
        }

        public void AttemptJoin()
        {
            PlayerPrefs.SetInt("ShouldStartClient", 1);
            PlayerPrefs.SetString("HostIp", _inputField.text);
            SceneManager.LoadScene(1);
        }
    
    }
}


[CreateAssetMenu(fileName = "Input Field Validator", menuName = "Input Field Validator")]
public class CustomValidator : TMPro.TMP_InputValidator
{
    
    public override char Validate(ref string text, ref int pos, char ch)
    {
        char dot = Char.Parse(".");
        if (char.IsNumber(ch) || ch == dot)
        {
            text = text.Insert(pos, ch.ToString());
            pos++;
            return ch;
        }
        else
        {
            return '\0';
        }
        
    }
    
}