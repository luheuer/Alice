using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endscreen_script : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //If the Panel is clicked, the Game returns to the title-screen
        if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Title");
        }        
    }
}
