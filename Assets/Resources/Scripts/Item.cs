using UnityEngine;
using System.Collections;

public enum WeaponType
{
    Pistol, Rifle, Axe
}

public enum ItemFacing
{ 
    Left,Right,Up,Down
}

public abstract class Item {

    public virtual GameObject ItemSprite { get; set; }
    public virtual bool Moving { get; set; }

    public ItemFacing Facing;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void UpdatePosition(Transform transform) { }

    public virtual void Use() { }

    public virtual void Equip() { } //sound, animation etc
}