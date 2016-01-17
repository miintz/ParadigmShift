using UnityEngine;
using System.Collections;
using System;

public class Conversation : MonoBehaviour {

    [TextArea(3, 10)]
    public string ConversationText = "This text will appear in a text area that automatically expands";

    private string[] ConversationLines;
    private int ConversationIndex = -1;

    private bool Active = false;
    private float counter = 0.0f;
	void Start () {
        ConversationLines = ConversationText.Split(new string[] { "+" }, StringSplitOptions.None);       
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    internal void Trigger()
    {
        ConversationIndex++;
        Active = true;
    }

    public string getLine()
    {
        if (Active)
            return ConversationLines[ConversationIndex];
        else
            return "END";
    }

    internal void Next()
    {
        if (ConversationIndex != ConversationLines.Length - 1)
            ConversationIndex++;
        else
            Active = false;
            
    }
}
