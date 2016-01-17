using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

    private AnimateState Animation;
    private int AnimationTime;
    public GameObject character;
     
	void Start () {
        AnimationTime = (int)Random.RandomRange(2.0f, 4.0f);
        character = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        Animate();	
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
}
