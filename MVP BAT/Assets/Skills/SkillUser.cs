using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUser : MonoBehaviour {

    public readonly string[] skills = { "Meteor", "Ice_Block", "Heavy_Rain", "Chain_Lightning", "Crippling_Oil" };

    [SerializeField]
    private GameObject Gelo;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void Ice_Block(Vector2 pos, Vector2 dir) {
        RaycastHit2D r = Physics2D.Raycast(pos, dir);
        //Debug.Log(r.transform.name);
        if (r != null){
            Quimica qui = r.transform.GetComponent<Quimica>();
            qui.humidade = 1;
            Inimigo i = r.transform.gameObject.GetComponent<Inimigo>();
            if (i != null){
                Rigidbody2D b = r.transform.gameObject.GetComponent<Rigidbody2D>();
                b.isKinematic = true;
                b.velocity = Vector2.zero;
                b.angularVelocity = 0;
                Instantiate(Gelo, r.transform);
                i.descongelar(3);
                //Causar Dano
            }
        }
    }
}
