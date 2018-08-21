using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MaterialQuimico : ScriptableObject {

    
    public bool liquido;
    public bool molhabil;
    [Space]
    public float pontoIgnicao;
    public float pontoFusao;
    public float condTermica;
    [Range(0f, 1f)]
    public float massaCombustivel;
    public float calorQueima;
    [Space]
    public bool condutorEletrico;

}
