﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

[Serializable]
public class MyEvent : UnityEvent { }


public class Keyboard : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField objectiveInputField;
    public static Keyboard KB;

    public MyEvent acceptEvent;


    void Start()
    {
        KB = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This is called by other inputfields, inside OnSelect
    public void OnSelect_FocusKeyboard(TMP_InputField inputField)
    {
        objectiveInputField = inputField;
    }

    //adding a letter to the input
    public void AddChar(string c)
    {
        objectiveInputField.text +=c;
    }

    //removing a letter from the input
    public void RemoveChar()
    {
        string actualText = objectiveInputField.text;

        if (actualText.Length > 0)
        {
            objectiveInputField.text = actualText.Remove(actualText.Length - 1);
        }
    }

    
    public void Accept()
    {
        acceptEvent.Invoke();
    }


}
