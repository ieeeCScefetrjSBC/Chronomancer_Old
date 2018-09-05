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
    public bool emChamas;
    public bool fonteTensao;
    public bool tensaoEle;
    private bool frameEle;
    public float calor = 25;
    [Range(0f, 1f)]
    public float humidade;
    [Range(0f, 1f)]
    public float combustivel;


    void Start()
    {
        Collider2D[] p = GetComponents<Collider2D>();
        foreach (Collider2D c in p) { if (!c.isTrigger) coli = c; break; }
        ps = GetComponent<ParticleSystem>();

        
    }

    void Update()
    {
        if ((fonteTensao || frameEle) && (material.condutorEletrico || humidade > 0.5f)) tensaoEle = true;
        else tensaoEle = false;

        frameEle = false;

        if (transform.localScale.x <= 0) Destroy(gameObject);

        if ((calor > material.pontoIgnicao * (1 + humidade - combustivel)) && !emChamas && transform.localScale.x >= material.massaCombustivel)
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
            if(coli != null) coli.enabled = false;
        }
        else
        {
            material.liquido = false;

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
                qui.humidade += 0.001f * (1 - material.massaCombustivel);
                qui.combustivel += 0.001f * material.massaCombustivel;
                transform.localScale -= Vector3.one * 0.001f;
            }

            // Eletricidade
            if (qui.tensaoEle) frameEle = true;
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!fonteTensao) tensaoEle = false;
    }

    public void deseltrizar(float tempo){
        Invoke("deseltrizar2", tempo);
    }

    private void deseltrizar2(){
        fonteTensao = false;
        tensaoEle = false;
    }
}

