using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapaCyber : MonoBehaviour
{

    private static List<List<MapaCyber>> MatMapaCyber;
    private static List<MapaCyber> sala;
    private static bool purge;

    [SerializeField]
    [Range(0f, 1f)]
    private float probSala;
    [SerializeField]
    private int maxX;
    [SerializeField]
    private int maxY;
    [SerializeField]
    private float disBlocos;
    [SerializeField]
    private int x;
    [SerializeField]
    private int y;

    [SerializeField]
    private GameObject caminho;

    public bool mantem;

    void Awake()
    {
        if (MatMapaCyber == null)
        {
            MatMapaCyber = new List<List<MapaCyber>>();
            sala = new List<MapaCyber>();

            for (int i = 0; i < maxX; i++)
            {
                MatMapaCyber.Add(new List<MapaCyber>());
                for (int j = 0; j < maxY; j++)
                {
                    if ((i == 0) && (j == 0))
                    {
                        MatMapaCyber[j].Add(this);
                        x = i;
                        y = j;

                        if ((x % 3 == 0) && (y % 3 == 0) && (Random.value < probSala)) sala.Add(this) ;
                    }
                    else
                    {
                        GameObject g = Instantiate(gameObject, transform.position + disBlocos * j * Vector3.down + disBlocos * i * Vector3.right, Quaternion.identity);
                        MapaCyber M = g.GetComponent<MapaCyber>();
                        MatMapaCyber[i].Add(M);
                        M.x = i;
                        M.y = j;

                        if((M.x%3 == 0) && (M.y % 3 == 0) && (Random.value < probSala)) sala.Add(M);

                    }


                }
            }

        }



    }

    void Start()
    {
        if (x == 0 && y == 0)
        {
            for (int n = 0; n < sala.Count - 1; n++)
            {
                //Debug.Log("n = (" + sala[n].x + ", " + sala[n].y + ")");
                //Debug.Log("n + 1 = (" + sala[n+1].x + ", " + sala[n+1].y + ")");
                sala[n].mantem = true;
                int iniciox = Mathf.Min(sala[n].x, sala[n+1].x);
                int inicioy = Mathf.Min(sala[n].y, sala[n + 1].y);
                int fimx = Mathf.Max(sala[n].x, sala[n + 1].x);
                int fimy = Mathf.Max(sala[n].y, sala[n + 1].y);
                if (Random.value < 0.5f)
                {
                    for (int i = iniciox; i <= fimx; i++)
                    {
                        MatMapaCyber[i][sala[n].y].mantem = true;
                    }
                    for (int j = inicioy; j <= fimy; j++)
                    {
                        MatMapaCyber[sala[n + 1].x][j].mantem = true;
                    }
                }
                else
                {
                    for (int i = iniciox; i <= fimx; i++)
                    {
                        MatMapaCyber[i][sala[n+1].y].mantem = true;
                    }
                    for (int j = inicioy; j <= fimy; j++)
                    {
                        MatMapaCyber[sala[n].x][j].mantem = true;
                    }
                }
            }
            foreach (List<MapaCyber> L in MatMapaCyber) {
                foreach (MapaCyber M in L)
                {
                    if (!M.mantem) Destroy(M.gameObject);
                }
            }
        }



    }

    void Update()
    {
    }
}
