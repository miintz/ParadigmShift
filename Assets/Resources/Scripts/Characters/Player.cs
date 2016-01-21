using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    
    [HideInInspector()]
    public bool InteractingWith = false;

    Camera PlayerCamera;    
    Vector3 TargetPosition;

    AnimateState Animation;
    FacingDirection Facing;
  
    private Texture[] textures;

    NPC InteractingNPC = null;

    private ConversationState ConvState = ConversationState.NPC;

    private GameObject PlayerConvText;
    
    public bool GodMode = true;
    private List<Item> Inventory;
    private Item activeItem;

	// Use this for initialization
	void Start () {

        Inventory = new List<Item>();

        if (GodMode)
        {
            Weapon w = new Weapon(WeaponType.Rifle);
            Inventory.Add(w);
        }

        //set active (for now)
        activeItem = Inventory[0];

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

        GetComponent<Renderer>().material.mainTexture = textures[2];
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
                ConvState = ConversationState.NPC;

                InteractingWith = false;
                InteractingNPC.SetConversationLine(true);
                
                PlayerConvText.GetComponent<TextMesh>().text = "";
            }
        }
                       
        ((Weapon)activeItem).UpdatePosition(this.transform);
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

    private void SetItemFacing(ItemFacing facing)
    {
        activeItem.Facing = facing;
    }

    private void SetItemMoving(bool state)
    {
        activeItem.Moving = state;

        if (state && activeItem.GetType() == typeof(Weapon))        
            ((Weapon)activeItem).AnimTimerLimit = 0.6f;
        else
            ((Weapon)activeItem).AnimTimerLimit = 1.7f;                          
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

        SetItemMoving(false);

        if (Input.GetKey(KeyCode.W))
        {
            SetItemMoving(true);
            TargetPosition += new Vector3(0, 0, 1.0f / MoveSpeed);
            SetItemFacing(ItemFacing.Up);

            if (Facing != FacingDirection.Front)
            {
                GetComponent<Renderer>().material.mainTexture = textures[0];
                Facing = FacingDirection.Front;                
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            SetItemMoving(true);            
            TargetPosition += new Vector3(0, 0, -1.0f / MoveSpeed);
            SetItemFacing(ItemFacing.Down);

            if (Facing != FacingDirection.Back)
            {
                //change sprite
                GetComponent<Renderer>().material.mainTexture = textures[1];
                Facing = FacingDirection.Back;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            SetItemMoving(true);
            TargetPosition += new Vector3(-1.0f / MoveSpeed, 0, 0);
            SetItemFacing(ItemFacing.Left);

            if (Facing != FacingDirection.Left)
            {
                GetComponent<Renderer>().material.mainTexture = textures[2];
                Facing = FacingDirection.Left;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            SetItemMoving(true);
            TargetPosition += new Vector3(1.0f / MoveSpeed, 0, 0);
            SetItemFacing(ItemFacing.Right);

            if (Facing != FacingDirection.Right)
            {
                GetComponent<Renderer>().material.mainTexture = textures[3];
                Facing = FacingDirection.Right;
            }
        }                      
    }
}
