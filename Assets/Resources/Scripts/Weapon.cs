using UnityEngine;
using System.Collections;


public class Weapon : Item {

    GameObject ItemSprite;

    WeaponType Weapontype;

    private bool Equiped = false;
    private float SizeMod = 2.0f;

    private Quaternion TargetRotation;
    
    private int TargetAngle = -10;

    public float AnimTimer = 0.0f;
    public float AnimTimerLimit = 1.7f;

    private int AnimAngleLow = -15;
    private int AnimAngleHigh = -10;
    private int AnimAngleMod = 0;


	// Use this for initialization
	public Weapon(WeaponType Type)
    {
        Weapontype = Type;
        //left?

        Facing = ItemFacing.Left;

        switch (Type)
        { 
            case WeaponType.Axe:
                break;
            case WeaponType.Pistol:
                break;
            case WeaponType.Rifle:
                ItemSprite = GameObject.CreatePrimitive(PrimitiveType.Plane);
                ItemSprite.transform.localScale = new Vector3(-(0.5f / SizeMod), 1.0f, -(0.2f / SizeMod));

                Texture tex = (Texture)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets\\Resources\\Drawings\\Weapons\\rifle.png", typeof(Texture));
                ItemSprite.GetComponent<Renderer>().material = (Material)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets\\Resources\\Drawings\\Weapons\\rifle.mat", typeof(Material));

                ItemSprite.transform.rotation = Quaternion.AngleAxis(TargetAngle, -Vector3.up);

                break;
        }
	}
	
	// Update is called once per frame
	public void UpdatePosition (Transform transform) {
        
        switch(Facing)
        {
            case ItemFacing.Left:
                ItemSprite.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y + 0.6f, transform.position.z);
                ItemSprite.transform.localScale = new Vector3(-(0.5f / SizeMod), 1.0f, -(0.2f / SizeMod));
                AnimAngleMod = 0;
                break;
            case ItemFacing.Right:
                ItemSprite.transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y + 0.6f, transform.position.z);
                ItemSprite.transform.localScale = new Vector3((0.5f / SizeMod), 1.0f, -(0.2f / SizeMod));
                AnimAngleMod = -15;
                break;
            case ItemFacing.Up:
                ItemSprite.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z + 0.5f);
                break;
            case ItemFacing.Down:
                ItemSprite.transform.position = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z - 1.0f);
                break;
        }

        if (AnimTimer >= AnimTimerLimit)
        {
            Animate();
            AnimTimer = 0.0f;
        }

        AnimTimer += Time.deltaTime;

        ItemSprite.transform.rotation = Quaternion.LerpUnclamped(ItemSprite.transform.rotation, TargetRotation, Time.deltaTime);

	}

    private void Animate()
    {
        if (Moving)
        {
            AnimAngleLow = -30 - AnimAngleMod;
            AnimAngleHigh = -5 - AnimAngleMod;
        }
        else 
        {
            AnimAngleLow = -15 - AnimAngleMod;
            AnimAngleHigh = -10 - AnimAngleMod;
        }
        Debug.Log(AnimAngleHigh + " " + AnimAngleLow);
        if (TargetAngle == AnimAngleHigh)
        {
            TargetAngle = AnimAngleLow;
            float a = Quaternion.Angle(TargetRotation, new Quaternion(0, 0, 0, 0));
            
            TargetRotation = Quaternion.AngleAxis(TargetAngle, -Vector3.up);
            
        }
        else
        {
            TargetAngle = AnimAngleHigh;
            float a = Quaternion.Angle(TargetRotation, new Quaternion(0, 0, 0, 0));

            TargetRotation = Quaternion.AngleAxis(TargetAngle, -Vector3.up);            
        }

    }

    public void Use()
    { 
        //eerst moet je het richten
    }

    public void Equip()
    { 
        
    }
}
