using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Textboxmanager_script : MonoBehaviour
{
    //VARIABLeS
    [SerializeField]
    private GameObject textbox;
    [SerializeField]
    private TextMeshProUGUI theTextUI;
    [SerializeField]
    private TextAsset textFile;
    private string[] textLines;
    private int currentLine;
    private int endLine;
    public GameObject player;
    [SerializeField]
    private bool isActive;
    
    // Start is called before the first frame update
    void Start()
    {   
        //The Player starts the Maze scene in a Monolouge, so they are instantly locked in place
        if(SceneManager.GetActiveScene().name == "Maze")
        {
            player.GetComponent<Td_Player_script>().setLocked(true);
        } else {
            player.GetComponent<Player_script>().setLocked(true);
        }
        
        if(textFile != null)
        {
            textLines = (textFile.text.Split("\n"));
        }   

        //If there is no specifically defined end line, it reads until the end of the file.
        if(endLine == 0)
        {
            endLine = textLines.Length -1;
        }

        if(isActive)
        {
            EnableTextbox();
        } else {
            DisableTextbox();
        }
    }
    void Update()
    {
        //If the textbox isn't active no updateing is required
        if(!isActive)
        {
            return;
        }
        theTextUI.text = textLines[currentLine];

        // The Player uses the left mouse button to further the dialogue.
        if(Input.GetMouseButtonDown(0))
        {
            if(currentLine == endLine)
            {   
                //Scene-dependent, as there is a seperate player skript for the Maze scene
                if(SceneManager.GetActiveScene().name == "Maze")
                {
                    player.GetComponent<Td_Player_script>().setLocked(false);
                    //Edge case where closing the textbox causes the scene to change
                    if(player.transform.position.y > 20f)
                    {
                        SceneManager.LoadScene("Transition");
                    }
                } else {
                    player.GetComponent<Player_script>().setLocked(false);
                } 
                DisableTextbox();

            } else {
                currentLine += 1;
            }
        }
        
    }

    public void EnableTextbox()
    {
        Debug.Log("Enabled");
        textbox.SetActive(true);
        setIsActive(true);
    }
    public void DisableTextbox()
    {
        Debug.Log("Disabled");
        textbox.SetActive(false);
        setIsActive(false);
    }

    public void ReloadScript(TextAsset newText)
    {//changes the text currently reffered to in the script.
        if(newText != null)
        {
            textLines = new string[1];
            textLines = (newText.text.Split("\n"));
        }
    }

    // Misc setter functions
    public void setIsActive(bool state)
    {
        this.isActive = state;
    }

    public void SetCurrentLine(int n)
    {
        currentLine = n;
    }

    public void SetEndLine(int n)
    {
        endLine = n;
    }
}
