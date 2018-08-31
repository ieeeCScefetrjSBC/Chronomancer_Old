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
    public float skill1CoolDown;
    public float skill2CoolDown;
    public float skill3CoolDown;
    private float skill1cdt;
    private float skill2cdt;
    private float skill3cdt;
    private Animator anim;
    private SpriteRenderer spr;
    private Sprite sp;
    private SkillUser skills;


    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        skills = GetComponent<SkillUser>();
        sp = spr.sprite;
	}
	
	void Update () {
        position = transform.position;
        vel = rb.velocity;
	}

    void FixedUpdate()
    {
        float x = 0, y = 0, v = rb.velocity.magnitude;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && v < maxRunVel) y += RunVel;
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && v < maxRunVel) y -= RunVel;
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && v < maxRunVel) x += RunVel;
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && v < maxRunVel) x -= RunVel;

        }
        else
        {
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && v < maxVel) y += Vel;
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && v < maxVel) y -= Vel;
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && v < maxVel) x += Vel;
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && v < maxVel) x -= Vel;
        }
        rb.velocity += new Vector2(x, y);

        if (v != 0) anim.enabled = true;
        else
        {
            anim.enabled = false;
            spr.sprite = sp;
        }

        Vector3 a = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - 90);

        //Skills
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        if (Input.GetMouseButton(0) && Time.time > skill1cdt + skill1CoolDown){
            skill1cdt = Time.time;
            skills.Ice_Block((Vector2)transform.position + dir, dir, 3, 10);
        }

        if (Input.GetMouseButton(1) && Time.time > skill2cdt + skill2CoolDown)
        {
            skill2cdt = Time.time;
            skills.Chain_Lightning((Vector2)transform.position, dir, 15, 10);
        }

        if (Input.GetMouseButton(2) && Time.time > skill3cdt + skill3CoolDown)
        {
            skill3cdt = Time.time;
            skills.Crippling_Oil((Vector2)transform.position, dir, 15, 10);
        }
    }
}
