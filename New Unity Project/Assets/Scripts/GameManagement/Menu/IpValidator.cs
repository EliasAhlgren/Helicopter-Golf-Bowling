using System;
using UnityEngine;

namespace GameManagement.Menu
{
    [CreateAssetMenu(fileName = "Input Field Validator", menuName = "Input Field Validator")]
    public class IpValidator : TMPro.TMP_InputValidator
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
}