using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour {

    private static List<List<Mapa>> MatMapa;
    private static List<Mapa> destruir;
    private static bool purge;

    [SerializeField]
    [Range(0f,1f)]
    private float probSaida;
    [SerializeField]
    private int maxX;
    [SerializeField]
    private int maxY;
    [SerializeField]
    private float disBlocos;
    private int x;
    private int y;

    private bool saidaD;
    private bool saidaB;

    [SerializeField]
    private GameObject caminho;

    void Awake () {
        if (MatMapa == null){
            MatMapa = new List<List<Mapa>>();
            destruir = new List<Mapa>();

            for (int i = 0; i < maxX; i++){
                MatMapa.Add(new List<Mapa>());
                for (int j = 0; j < maxY; j++) {
                    if ((i == 0) && (j == 0)){
                        MatMapa[j].Add(this);
                        x = i;
                        y = j;

                        saidaB = Random.value < probSaida;
                        saidaD = Random.value < probSaida;
                    }
                    else{
                        GameObject g = Instantiate(gameObject, transform.position + disBlocos * j * Vector3.down + disBlocos * i * Vector3.right, Quaternion.identity);
                        Mapa M = g.GetComponent<Mapa>();
                        MatMapa[i].Add(M);
                        M.x = i;
                        M.y = j;


                        if (M.y != (maxY - 1)) M.saidaB = Random.value < probSaida;
                        if (M.x != (maxX - 1)) M.saidaD = Random.value < probSaida;
                        
                    }
                    
                    
                }
            }
            
        }

       

    }
	
	void Start () {
        if (saidaB) Instantiate(caminho, transform.position + (disBlocos / 2) * Vector3.down, Quaternion.identity);
        if (saidaD) Instantiate(caminho, transform.position + (disBlocos / 2) * Vector3.right, Quaternion.identity);


        if (x == 0 && y == 0) {
            if (!saidaB && !saidaD) destruir.Add(this);
        }
        else
        {
            if (x == 0)
            {
                if (!saidaB && !saidaD && !MatMapa[x][y - 1].saidaB) destruir.Add(this);
            }
            else if (y == 0)
            {
                if (!saidaB && !saidaD && !MatMapa[x - 1][y].saidaD) destruir.Add(this);
            }
            else
            {
                if (!saidaB && !saidaD && !MatMapa[x][y - 1].saidaB && !MatMapa[x - 1][y].saidaD) destruir.Add(this);
            }
        }
        
    }

    void Update(){
        if (!purge) { foreach (Mapa M in destruir) Destroy(M.gameObject); purge = true;}
    }
}
