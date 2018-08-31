using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq;
using System;

using Random = UnityEngine.Random;

public class ProceduralGeneration : MonoBehaviour
{

    private static class Gaussian
    {
        public static double Mu { get; set; }
        public static double Sigma { get; set; }

        static Gaussian()
        {
            Mu = 0f;
            Sigma = 1f;
        }

        public static void SetGaussParam(double mu, double sigma)
        {
            Mu = mu;
            Sigma = sigma;
        }

        public static double Val(double x)
        {
            double Z = (x - Mu) / Sigma;
            Z = Math.Pow(Z, 2);

            double exp = Math.Exp(-Z / 2);
            double gauss = (1 / (Sigma * Math.Sqrt(2 * Math.PI))) * exp;

            return gauss;
        }
    }

    public class Statistics
    {
        public float min;
        public float max;
        public float average;
        public float std;          // standard deviation
        //public float distortion;

        public static double stdIntegStep = 0.000001f;

        public Statistics(float min, float max, float average, float std)
        {
            this.min = min;
            this.max = max;
            this.average = average;
            this.std = std;
        }
    }

    public class Node
    {
        public Node prevNode;
        public Node nextNode;

        public int idx;
    }

    public class Room : Node
    {
        List<Path> forks = new List<Path>();

        public int height;
        public int width;
        public int area;
    }

    public class Corridor : Node
    {
        List<Path> forks = new List<Path>();

        public int lenght;
        public int width;
        public int numTurns;
    }

    public class Path
    {
        private Statistics numRoomStats;
        private Statistics numForkStats;

        public List<Node> NodeList { get; set; }                      // the list of nodes that constitute this path
        public List<Path> Forks { get; set; }                      // the list of paths that fork from this path

        public Node Source { get; set; }                              // the node from which the path is constructed

    }


    public float standardSTD;       // 1f
    public float minFork2LenRatio = 0.5f;  // 0f
    public float maxFork2LenRatio = 2f;  // (stageLength + 2) * 2
    public int minStageLength = 1;    // 1
    public int maxStageLength = 15;    // 10

    public float averageStageLength = 7f;
    public int stageLength;

    public float averageFork2LenRatio = 2f;
    public float fork2LenRatio;

    public float forkLenParam;

    public float numForks;

    public Statistics topNumRoomStats = new Statistics(3f, 12f, 7f, 1f);
    public Statistics topNumForkStats = new Statistics(3f, 12f, 7f, 1f);

    public float tileLenght = 5f;

    private int GenUniformRand(float average, float min, float max)
    {
        float fnum = Random.Range(min, max + 1);
        if (fnum == max + 1)
            fnum = max;

        return Mathf.FloorToInt(fnum);
    }

    void Start()
    {
        //Debug.Log(Integral(Math.Cos, Mathf.PI / 4, 0f, 0.0001f));
        //Debug.Log(Integral(Gaussian.Val, 2f, 0f, Statistics.stdIntegStep));
        //Debug.Log(GaussSectionWidth(0.95f, 0f, 0.4162f));

        //stageLength = GenGaussRand(averageStageLength, minStageLength, maxStageLength);
        //numForks = GenGaussRand(averageFork2LenRatio * stageLength,
        //                            (int)(minFork2LenRatio * stageLength),
        //                            (int)(maxFork2LenRatio * stageLength));

        int[] results = new int[100];
        for (int i = 0; i < 100; i++)
        {
            results[i] = 0;
        }

        int[] range = new int[maxStageLength - minStageLength + 1];

        for (int i = 0; i < 100; i++)
        {
            stageLength = GenUniformRand(averageStageLength, minStageLength, maxStageLength);

            float min = Mathf.Floor(minFork2LenRatio * stageLength);
            float max = Mathf.Floor(maxFork2LenRatio * stageLength);

            numForks = GenUniformRand(averageFork2LenRatio * stageLength, min, max);

            results[i] = stageLength;
            range[stageLength - minStageLength]++;

            Debug.Log("stageLength: " + stageLength);
            Debug.Log("numForks:    " + numForks);

            //Debug.Log("min:    " + min);
            //Debug.Log("max:    " + max);
            //Debug.Log("ratio:    " + maxFork2LenRatio);
        }

        foreach (int freq in range)
        {
            int num = minStageLength + Array.IndexOf(range, freq);
            Debug.Log(num + ": " + freq);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private double Integral(Func<double, double> f, double b, double a, double step)
    {
        if (a + step >= b)
            return (f(a) + f(a)) * (b - a) / 2;

        double F = f(a) * step / 2;

        double x;
        for (x = a + step; x + step < b; x += step)
            F += f(x) * step;

        F += (f(x) + f(b)) * (b - x) / 2;

        return F;
    }

    private double GaussSectionWidth(double prob, double mu, double sigma, double precision = 0.01)
    {
        double width;
        Gaussian.SetGaussParam(mu, sigma);

        double integStep = Statistics.stdIntegStep;
        double iterStep = sigma / 2;

        double halfArea = 0;
        double pivot = mu;

        double error = -prob;
        double increment = 0;

        float time = Time.time;

        int numIters = 0;
        while (numIters < 100 && error < 0)
        {
            increment = Integral(Gaussian.Val, pivot + iterStep, pivot, integStep);
            halfArea += increment;
            pivot += iterStep;

            error = 2 * halfArea - prob;

            if (error > 0 && error > precision)
            {
                pivot -= iterStep;
                iterStep /= 2;

                halfArea -= increment;
                error = 2 * halfArea - prob;
            }
            numIters++;
        }
        width = pivot - mu;

        Debug.Log("width: " + width);
        return width;
    }

    private int GenGaussRand(float average, int min, int max)
    {
        float gaussSliceProb = Random.value;
        float sliceWidth = (float)GaussSectionWidth(gaussSliceProb, average, 3);

        if (average - sliceWidth > min)
            min = Mathf.FloorToInt(average - sliceWidth);
        if (average + sliceWidth < max)
            max = Mathf.CeilToInt(average + sliceWidth);

        return Mathf.RoundToInt(Random.Range(min, max));
    }
}