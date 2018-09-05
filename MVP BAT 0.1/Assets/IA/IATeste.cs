using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IATeste : Inimigo {

    [Space]
    [SerializeField]
    private float vel;
    [SerializeField]
    private float distSegue;

	void Start () {
        StartCode();
	}
	
	void Update () {
        UpdateCode();

        Vector2 dif = (Player.position - transform.position);
        if (dif.magnitude < distSegue) rb.velocity = dif.normalized*vel;
    }
}
