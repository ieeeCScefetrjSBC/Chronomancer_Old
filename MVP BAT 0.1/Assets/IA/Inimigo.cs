using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour {

    public float vida;
    [Space]
    public float danoFogo;
    public float danoTemp;
    public Vector2 tempSegura;
    [Space]
    public float danoEletricidade;
    [Space]
    public float danoAgua;
    private Color original;
    protected Rigidbody2D rb;
    private Quimica qui;
    private SpriteRenderer spr;

    

    protected void StartCode () {
        rb = GetComponent<Rigidbody2D>();
        qui = GetComponent<Quimica>();
        spr = GetComponent<SpriteRenderer>();
        original = spr.color;
    }

    protected void UpdateCode()
    {
        if (qui.tensaoEle) spr.color = new Color(0.2f,0.5f,0.5f);
        else spr.color = original;

        if (qui.calor < tempSegura.x || qui.calor > tempSegura.y) vida -= danoTemp * Time.deltaTime;
        if (qui.emChamas) vida -= danoFogo * Time.deltaTime;
        if (qui.humidade > 0.5f) vida -= danoAgua * Time.deltaTime;
        if (qui.tensaoEle) vida -= danoEletricidade * Time.deltaTime;

        if (vida <= 0) Destroy(gameObject);
    }

    public void descongelar (float tempo) {
        Invoke("descongelar2", tempo);
	}

    public void descongelar2()
    {
        rb.constraints = RigidbodyConstraints2D.None;
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
