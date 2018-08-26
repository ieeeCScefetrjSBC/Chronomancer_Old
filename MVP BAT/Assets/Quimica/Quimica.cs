using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Quimica : MonoBehaviour
{

    public const float tempAr = 25;

    private ParticleSystem ps;

    private Collider2D coli;
    public MaterialQuimico material;
    private bool emChamas;
    public bool fonteTensao;
    [SerializeField]
    private bool tensaoEle;
    public float calor = 25;
    [Range(0f, 1f)]
    public float humidade;


    void Start()
    {
        Collider2D[] p = GetComponents<Collider2D>();
        foreach (Collider2D c in p) { if (!c.isTrigger) coli = c; break; }
        ps = GetComponent<ParticleSystem>();

        if (fonteTensao) tensaoEle = true;
    }

    void Update()
    {
        if (transform.localScale.x <= 0) Destroy(gameObject);
        if ((calor > material.pontoIgnicao * (1 + humidade)) && !emChamas && transform.localScale.x >= material.massaCombustivel)
        {
            emChamas = true;
            ps.Play();
        }
        if (emChamas && Time.frameCount % 10 == 0)
        {
            transform.localScale -= Vector3.one * 0.01f;
            calor += material.calorQueima;
            if (calor < 0.7f * material.pontoIgnicao || humidade > 0.4F)
            {
                emChamas = false;
                ps.Stop();
            }
            if (transform.localScale.x <= material.massaCombustivel)
            {
                emChamas = false;
                ps.Stop();
            }
        }


        if (calor > material.pontoFusao)
        {
            material.liquido = true;
            coli.enabled = false;
        }
        else
        {
            material.liquido = false;
            coli.enabled = true;
        }

        int tempDif = (int)(calor - tempAr);
        float resfriAr = (1f + material.condTermica) * tempDif * tempDif * tempDif * Mathf.Abs(tempDif) * 0.0000000001f;
        
        if (Time.frameCount % 10 == 0)
        {
            calor -= resfriAr;
            if (calor > 100 && humidade > 0) humidade -= 0.01f;
        }

    }
    private void OnTriggerStay2D(Collider2D ou)
    {
        Quimica qui = ou.GetComponent<Quimica>();
        if (qui != null)
        {
            float dis = (ou.transform.position - transform.position).magnitude;
            
            float troca = (dis <= 0.1f)? 0.1f : (material.condTermica + qui.material.condTermica) / (2 * (dis));
            
            Debug.Log(troca);

            if (qui.calor > calor)
            {
                calor += troca;
                qui.calor -= troca;
            }
            else
            {
                calor -= troca;
                qui.calor += troca;
            }

            if (material.liquido && qui.material.molhabil)
            {
                qui.humidade += 0.001f * humidade;
                transform.localScale -= Vector3.one * 0.001f;
            }

        }


    }
    private void OnCollisionStay2D(Collision2D ou)
    {
        Quimica qui = ou.gameObject.GetComponent<Quimica>();

        if (qui.tensaoEle && (material.condutorEletrico || humidade > 0.5f))
            tensaoEle = true;
    }

    private void OnCollisionExit2D(Collision2D ou)
    {
        if(!fonteTensao) tensaoEle = false;
    }
}

