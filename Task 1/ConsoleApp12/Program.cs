using System;
using System.IO;

class Program
{
    static void Main()
    {
       
        string[] text = File.ReadAllLines("input.txt");
        int n = int.Parse(text[0]);
        double[,] G = new double[n, n];
        double[] vector = new double[n];
        for (int i = 0; i < n; i++)
        {
            string[] row = text[i + 1].Split(' ');
            for (int j = 0; j < n; j++)
            {
                G[i, j] = double.Parse(row[j]);
            }
        }
        string[] vectorD = text[n + 1].Split(' ');
        for (int i = 0; i < n; i++)
        {
            vector[i] = double.Parse(vectorD[i]);
        }
        if (Symmetry(G,n)==false)
        {
            Console.WriteLine("Матрица не симметрична");
            return;
        }
        double length = Calculate(G, vector);
        Console.WriteLine("Длина вектора: "+length);
       
    }

    static bool Symmetry(double[,] matrix,int n)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (matrix[i, j] != matrix[j, i])
                {
                    return false;
                }
            }
        }
        return true;
    }
    static double Calculate(double[,] G, double[] vector)
    {
        int n = vector.Length;
        double[] temp = new double[n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                temp[i] += vector[j] * G[j, i];
            }
        }
        double result = 0;
        for (int i = 0; i < n; i++)
        {
            result += vector[i] * temp[i];
        }

        return Math.Sqrt(result);
    }
}