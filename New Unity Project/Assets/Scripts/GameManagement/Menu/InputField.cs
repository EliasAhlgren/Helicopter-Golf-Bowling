using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputField : MonoBehaviour
{

    private TMP_InputField _inputField;
    
    // Start is called before the first frame update
    void Start()
    {
        _inputField = gameObject.GetComponent<TMP_InputField>();
    }

    void AttemptJoin()
    {
        PlayerPrefs.SetInt("ShouldStartClient", 1);
    }
    
    // Update is called once per frame
    void Update()
    {
        _inputField.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        PlayerPrefs.SetString("HostIp", _inputField.text);
    }
}
