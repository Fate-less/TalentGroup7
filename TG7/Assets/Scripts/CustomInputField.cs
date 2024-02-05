using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomInputField : TMP_InputField
{
    //disable copy paste
    protected override void Append(string input){}
}
