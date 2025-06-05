using System;

namespace WordSearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Matrix matrix = new Matrix(24);
            Display display = new Display();

            Console.WriteLine(matrix.Grid[1, 1]);

            /* for (int x = 0; x < matrix.Width; x++)
            {
                for (int y = 0; y < matrix.Width; y++)
                {
                    Console.Write(matrix.Grid[x, y] + " ");
                }
                Console.WriteLine();
            } */

            display.DisplayLabels(matrix.Width);

        }
    }

    public class Display
    {
        public void DisplayLabels(int width)
        {
            for (int i = 1; i <= width; i++)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
            for (int i = 1; i <= width; i++)
            {
                if (i < 10)
                {
                    Console.Write("━" + "━");
                }
                else
                {   
                    Console.Write("━━" + "━");
                }
            }
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