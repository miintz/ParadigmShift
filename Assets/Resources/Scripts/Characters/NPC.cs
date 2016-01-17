using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {
    
    public GameObject character;
    
    private AnimateState Animation;
    private int AnimationTime;
    
    private Conversation NPCConversation;

    private bool InteractedWith = false;

         GameObject NPCConvText;

	void Start () {
        AnimationTime = (int)Random.Range(2.0f, 4.0f);
        character = this.gameObject;

        NPCConversation = this.GetComponent<Conversation>();
        NPCConvText = GameObject.Find("NPCConvText"); //dit werkt alleen voor 1 object...
	}
	
	// Update is called once per frame
	void Update () {
        Animate();	
	}

    public bool Interact()
    {   
        InteractedWith = true;
        //actie? hoe ga ik met *generieke acties* om

        //voor nu alleen conversaties
        if (NPCConversation != null)
        {
            NPCConversation.Trigger();
            return true;
        }

        return false;   
    }

    void Animate()
    {
        if (transform.localScale.z >= 0.194f) //dit moet wel ff anders..
            Animation = AnimateState.Down;       
        else if (transform.localScale.z <= 0.161f)
            Animation = AnimateState.Up;

        if (Animation == AnimateState.Up)
            transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(transform.localScale.x, transform.localScale.y, 0.195f), Time.deltaTime * AnimationTime);
        else
            transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(transform.localScale.x, transform.localScale.y, 0.160f), Time.deltaTime * AnimationTime);
    }

    internal void NextConversationLine()
    {
        NPCConversation.Next();
    }

    internal string GetConversationLine()
    {
        return NPCConversation.getLine();
    }

    internal void SetConversationLine(bool empty = false)
    {
        if (!empty)
            NPCConvText.GetComponent<TextMesh>().text = GetConversationLine();
        else
            NPCConvText.GetComponent<TextMesh>().text = "";
    }
}
