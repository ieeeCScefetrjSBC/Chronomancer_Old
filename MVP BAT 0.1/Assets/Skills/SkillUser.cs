using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUser : MonoBehaviour {

    public readonly string[] skills = { "Meteor", "Ice_Block", "Heavy_Rain", "Chain_Lightning", "Crippling_Oil" };

    [SerializeField]
    private GameObject Gelo;
    [SerializeField]
    private GameObject Oleo;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void Ice_Block(Vector2 pos, Vector2 dir,float tempo, float dano) {
        RaycastHit2D r = Physics2D.Raycast(pos, dir);
        if (r.transform != null){
            Quimica qui = r.transform.GetComponent<Quimica>();
            if (qui != null)  qui.humidade = 1;

            Inimigo i = r.transform.gameObject.GetComponent<Inimigo>();
            if (i != null){
                Rigidbody2D b = r.transform.gameObject.GetComponent<Rigidbody2D>();
                b.constraints = RigidbodyConstraints2D.FreezeAll;
                b.velocity = Vector2.zero;
                b.angularVelocity = 0;
                Instantiate(Gelo, r.transform);
                i.descongelar(tempo);


                i.vida -= dano;//Causar Dano
            }
        }
    }

    public void Chain_Lightning(Vector2 pos, Vector2 dir, float tempo, float dano)
    {
        RaycastHit2D r = Physics2D.Raycast(pos, dir);
        //Debug.Log(r.transform.name);
        if (r.transform != null)
        {
            Quimica qui = r.transform.GetComponent<Quimica>();
            if (qui != null){
                qui.fonteTensao = true;
                qui.deseltrizar(tempo);
            }
            Inimigo i = r.transform.gameObject.GetComponent<Inimigo>();
            if (i != null)
            {
                i.vida -= dano;
            }
        }
    }

    public void Crippling_Oil(Vector2 pos, Vector2 dir, float tempo, float dano)
    {
        Instantiate(Oleo, transform.position, Quaternion.identity);
    }
}
