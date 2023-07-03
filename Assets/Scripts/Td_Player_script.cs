using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Td_Player_script : MonoBehaviour
{
    //Seperate Player Script for the top-down view in the first playable scene.
    
    //VARIABLES
    [SerializeField]
    private float _speed = 12f;
    [SerializeField]
    private Rigidbody rb;

    private bool is_Locked = false;

    //--- for textbox ---
    [SerializeField]
    private TextAsset newText;
    [SerializeField]
    private Textboxmanager_script theTextbox;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(2.1f, -22.5f, -1f); 
    }

    // Update is called once per frame
    void Update()
    {
        //MOVEMENT
        if(!is_Locked)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 tempVect = new Vector3(h, v, 0);
            tempVect = tempVect.normalized * _speed * Time.deltaTime;
            rb.MovePosition(rb.transform.position + tempVect);
        }    
    }

    void OnTriggerEnter(Collider trigger)
    {
       //There is only one possible Trigger for the td_player, so the Text for reaching the goal is displayed.
        setLocked(true);
        theTextbox.ReloadScript(newText);
        theTextbox.SetCurrentLine(0);
        theTextbox.SetEndLine(7);
        theTextbox.EnableTextbox();
        
    }

    public void setLocked(bool state)
    { // locks the player movement
        this.is_Locked = state;
    }
}
