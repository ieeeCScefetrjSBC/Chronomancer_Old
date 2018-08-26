using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour {

    public float vida;
    private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
	
	public void descongelar (int tempo) {
        Invoke("descongelar2", tempo);
	}

    public void descongelar2(){
        rb.isKinematic = false;
        Transform t = transform.GetChild(0);
        if (t != null) Destroy(t.gameObject);
    }
}
