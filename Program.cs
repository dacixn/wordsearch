using System;

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

            Console.SetCursorPosition(20, 20);
        }
    }

    public class Display
    {
        public void DisplayLabels(string[,] matrix, int width)
        {
            
        }

        public void DisplayMatrix(string[,] matrix)
        {

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

        public string FindLetter(int x, int y)
        {
            return "foo";
        }

        public void InsertWord(string word)
        {

        }
    }
}