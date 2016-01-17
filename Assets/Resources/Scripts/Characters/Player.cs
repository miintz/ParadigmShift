using UnityEngine;
using System.Collections;

public enum FacingDirection { 
    Left,Right,Front,Back
}

enum AnimateState { 
    Up, Down
}

enum ConversationState
{
    NPC, Player
}

public class Player : MonoBehaviour {

    public float MoveSpeed = 5.0f;
  
    Camera PlayerCamera;    
    Vector3 TargetPosition;

    AnimateState Animation;
    FacingDirection Facing;
  
    private Texture[] textures;

    public bool InteractingWith = false;
    NPC InteractingNPC = null;

    private ConversationState ConvState = ConversationState.NPC;

    private GameObject PlayerConvText;

	// Use this for initialization
	void Start () {
        Facing = FacingDirection.Left;
        Animation = AnimateState.Down;
        
        PlayerCamera = Camera.main;
        TargetPosition = transform.position;

        PlayerConvText = GameObject.Find("PlayerConvText");

        textures = new Texture[4];

        textures[0] = (Texture)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets\\Resources\\Drawings\\Characters\\theblind_back.png", typeof(Texture));
        textures[1] = (Texture)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets\\Resources\\Drawings\\Characters\\theblind_front.png", typeof(Texture));
        textures[2] = (Texture)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets\\Resources\\Drawings\\Characters\\theblind_left.png", typeof(Texture));
        textures[3] = (Texture)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets\\Resources\\Drawings\\Characters\\theblind_right.png", typeof(Texture));       
	}
	
	// Update is called once per frame
	void Update () {        
        HandleKeyPress();
        Animate();

        if (!InteractingWith)
        {
            if (transform.position != TargetPosition)
            {
                bool valid = true;
                Vector3 newpos = Vector3.Lerp(this.transform.position, TargetPosition, Time.deltaTime * 10);
                foreach (NPC npc in (NPC[])GameObject.FindObjectsOfType<NPC>())
                {
                    float dist = Vector3.Distance(newpos, npc.gameObject.transform.position);
                    if (dist < 3.0f)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                    this.transform.position = newpos;
            }
        }
        else
        {
            string n = InteractingNPC.GetConversationLine();
            if (n != "END")
                DisplayText(n);
            else
            {
                InteractingWith = false;
                InteractingNPC.SetConversationLine(true);
                PlayerConvText.GetComponent<TextMesh>().text = "";
            }
        }
	}

    private void DisplayText(string n)
    {
        if (ConvState == ConversationState.NPC)
        {
            InteractingNPC.SetConversationLine();
            PlayerConvText.GetComponent<TextMesh>().text = "";
        }
        else
        {
            PlayerConvText.GetComponent<TextMesh>().text = InteractingNPC.GetConversationLine();
            InteractingNPC.SetConversationLine(true);
        }
    }
    
    void Animate()
    {        
        if (transform.localScale.z >= 0.194f) //dit moet wel ff anders..
            Animation = AnimateState.Down;   
        else if (transform.localScale.z <= 0.161f)
            Animation = AnimateState.Up;    

        if (Animation == AnimateState.Up)
            transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(transform.localScale.x, transform.localScale.y, 0.195f), Time.deltaTime * 2);
        else                    
            transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(transform.localScale.x, transform.localScale.y, 0.160f), Time.deltaTime * 2);       
    }

    void HandleKeyPress()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (!InteractingWith)
            {
                //interactie met iets
                NPC closest = null;
                float c = float.MaxValue;

                bool looping = true;
                while (looping)
                {
                    foreach (NPC npc in (NPC[])GameObject.FindObjectsOfType<NPC>())
                    {
                        bool nothit = true;
                        float dist = Vector3.Distance(this.transform.position, npc.gameObject.transform.position);
                        if (dist < c)
                        {
                            closest = npc;
                            c = dist;

                            nothit = true;
                        }

                        if (nothit)
                            looping = false;
                    }
                }
                if (c < 3.5f) //interact range!!!
                {
                    if (closest.Interact())
                    {                        
                        //zoom in camera
                        InteractingWith = true;
                        InteractingNPC = closest;
                    }
                }
            }
            else
            { 
                //volgende regel?
                if (ConvState == ConversationState.NPC)
                    ConvState = ConversationState.Player;
                else
                    ConvState = ConversationState.NPC;

                InteractingNPC.NextConversationLine();
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            TargetPosition += new Vector3(0, 0, 1.0f / MoveSpeed);
            if (Facing != FacingDirection.Front)
            {
                GetComponent<Renderer>().material.mainTexture = textures[0];
                Facing = FacingDirection.Front;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            TargetPosition += new Vector3(0, 0, -1.0f / MoveSpeed);
            if (Facing != FacingDirection.Back)
            {
                //change sprite
                GetComponent<Renderer>().material.mainTexture = textures[1];
                Facing = FacingDirection.Back;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            TargetPosition += new Vector3(-1.0f / MoveSpeed, 0, 0);

            if (Facing != FacingDirection.Left)
            {
                GetComponent<Renderer>().material.mainTexture = textures[2];
                Facing = FacingDirection.Left;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            TargetPosition += new Vector3(1.0f / MoveSpeed, 0, 0);
            if (Facing != FacingDirection.Right)
            {
                GetComponent<Renderer>().material.mainTexture = textures[3];
                Facing = FacingDirection.Right;
            }
        }              
    }
}
