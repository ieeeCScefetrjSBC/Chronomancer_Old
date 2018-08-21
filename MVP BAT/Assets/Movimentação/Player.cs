using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D rb;
    public static Vector3 position;
    public static Vector3 vel;
    public float Vel;
    public float RunVel;
    public float maxVel;
    public float maxRunVel;
    private Animator anim;
    private SpriteRenderer spr;
    private Sprite sp;


    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        sp = spr.sprite;
	}
	
	void Update () {
        position = transform.position;
        vel = rb.velocity;
	}

    void FixedUpdate(){
        float x = 0, y = 0, v = rb.velocity.magnitude;
        if (Input.GetKey(KeyCode.LeftShift)){
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && v < maxRunVel) y += RunVel;
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && v < maxRunVel) y -= RunVel;
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && v < maxRunVel) x += RunVel;
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && v < maxRunVel) x -= RunVel;
            
        }
        else {
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && v < maxVel) y += Vel;
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && v < maxVel) y -= Vel;
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && v < maxVel) x += Vel;
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && v < maxVel) x -= Vel;
        }
        rb.velocity += new Vector2(x, y);

        if (v != 0) anim.enabled = true;
        else {
            anim.enabled = false;
            spr.sprite = sp;
        }

        Vector3 a = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - 90);


    }
}
