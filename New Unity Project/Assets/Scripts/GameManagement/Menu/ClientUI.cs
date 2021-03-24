using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement.Menu
{
    public class ClientUI : MonoBehaviour
    {

        private TMP_InputField _inputField;
    
        // Start is called before the first frame update
        void Start()
        {
            _inputField = gameObject.GetComponent<TMP_InputField>();
            _inputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        }

        void AttemptJoin()
        {
            PlayerPrefs.SetInt("ShouldStartClient", 1);
            PlayerPrefs.SetString("HostIp", _inputField.text);
            SceneManager.LoadScene(1);
        }
    
    }
}
