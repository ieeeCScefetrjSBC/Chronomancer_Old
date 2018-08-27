using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour {

    public float vida;
    public float danoFogo;
    public float danoGelo;
    public float danoEletricidade;
    public float danoAgua;
    private Rigidbody2D rb;
    private Quimica qui;
    private SpriteRenderer spr;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        qui = GetComponent<Quimica>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (qui.tensaoEle) spr.color = new Color(0,0.3f,0.3f);
        else spr.color = new Color(0.1f, 0.1f, 0.1f);
        if (vida <= 0) Destroy(gameObject);
    }

    public void descongelar (float tempo) {
        Invoke("descongelar2", tempo);
	}

    public void descongelar2()
    {
        rb.isKinematic = false;
        foreach (Transform t in transform)
        {
            if (t.tag == "Gelo")
            {
                Destroy(t.gameObject);
                break;
            }
        }
    }
}
