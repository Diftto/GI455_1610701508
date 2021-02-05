using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public InputField fieldport;
    public Text showstatus;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectProgramChat()
    {
        int portnumber = int.Parse(fieldport.text);

        if (portnumber == 65000)
        {
            SceneManager.LoadScene("ProgramChat");
        }
    }

}
