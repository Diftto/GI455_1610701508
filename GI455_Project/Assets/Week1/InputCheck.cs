using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class InputCheck : MonoBehaviour
{
    public InputField inputField;
    public Text textOutput; 
    public string[] GameData = new string[5];
    string Input;
   
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Checkinput(string Input)
    {
        for(int i=0; i< GameData.Length; i++)
        {
            if (inputField.text == GameData[i])
            {
                textOutput.text = "[ " + "<color=green>" + inputField.text + "</color>" + " ] is found";
                break;
            }
            else
            {
                textOutput.text = "[ " + "<color=red>" + inputField.text + "</color>" + " ] is not found";
            }
        }
    }

}
