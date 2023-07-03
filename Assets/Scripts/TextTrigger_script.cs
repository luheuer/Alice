using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger_script : MonoBehaviour
{
    //This script is simply to enable a trigger to give a new Text source and other necessary information for the Text Box.
    //VARIABLES
    [SerializeField]
    private TextAsset theText;
    [SerializeField]
    private int startLine;
    [SerializeField]
    private int endLine;
    

    public TextAsset GetText()
    {
        return theText;
    }
    public int GetStartLine()
    {
        return startLine;
    }
    public int GetEndLine()
    {
        return endLine;
    }
}
