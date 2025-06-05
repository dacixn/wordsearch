using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;

namespace WordSearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Matrix matrix = new Matrix(24);
            Console.WriteLine(matrix.Grid[1, 1]);

            for (int x = 0; x < matrix.Width; x++)
            {
                for (int y = 0; y < matrix.Width; y++)
                {
                    Console.Write(matrix.Grid[x, y] + " ");
                }
                Console.WriteLine();
            }
        }
    }

    public class Display
    {
        public void DisplayMatrix()
        {

        }
    }

    public class Game
    {

    }

    public class Word
    {
        public string Random()
        {
            return "foo";
        }
    }

    public class Matrix
    {
        public int Width { get; }
        public string[,] Grid { get; }

        public Matrix(int desiredWidth)
        {
            Width = desiredWidth;
            Grid = Generate();
        }

        public string[,] Generate()
        {
            string[,] matrix = new string[Width, Width];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    matrix[x, y] = ".";
                }
            }

            return matrix;
        }

        public string FindLetter()
        {
            return "foo";
        }

        public void InsertWord(string word)
        {

        }
    }
}