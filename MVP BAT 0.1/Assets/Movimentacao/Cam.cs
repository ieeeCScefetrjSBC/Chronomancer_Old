using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    public float Vel;

	void Update () {
        transform.position += (Vector3)((Vector2)(Player.position + Player.vel - transform.position) * Vel);
	}
}
