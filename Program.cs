using System;
using System.Security;

namespace WordSearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Matrix matrix = new Matrix(16);
            Display display = new Display();

            display.DisplayLabels(matrix.Width);
            display.DisplayMatrix(matrix.Grid, matrix.Width);

            Console.ReadKey();

        }
    }

    public class Display
    {
        public void DisplayLabels(int width)
        {
            Console.Write("    ");

            // -- print x letters
            for (int i = 0; i < width; i++)
            {
               
                char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                Console.Write(" " + alpha[i]);
            }

            // -- print big line
            Console.WriteLine("");
            Console.Write("   ");
            Console.Write("┌");

            for (int i = 1; i <= width; i++)
            {
                Console.Write("─" + "─");
            }

            Console.WriteLine("");

            for (int i = 1; i <= width; i++)
            {
                if (i < 10)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(i + " │");
            }
            
        }

        public void DisplayMatrix(string[,] matrix, int width)
        {
            Console.SetCursorPosition(5, 2);
            (int left, int top) = Console.GetCursorPosition();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Console.Write(matrix[x, y] + " ");
                }
                top++;
                Console.SetCursorPosition(5, top);
            }
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

        public void InsertWord(string word, int dx, int dy)
        {

        }
    }
}