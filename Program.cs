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

            /* for (int x = 0; x < matrix.Width; x++)
            {
                for (int y = 0; y < matrix.Width; y++)
                {
                    Console.Write(matrix.Grid[x, y] + " ");
                }
                Console.WriteLine();
            } */

            display.DisplayLabels(matrix.Width);
            Console.SetCursorPosition(0, 0);
            Console.ReadKey();

        }
    }

    public class Display
    {
        public void DisplayLabels(int width) // ━━┏┃
        {
            Console.Write("    ");

            // -- print x letters
            for (int i = 0; i < width; i++)
            {
                // add a leading space for numbers under 10
                //if (i < 10)
                //{
                //    Console.Write(" ");
                //}
                //Console.Write(i + " ");

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

            // -- print y numbers
            for (int i = 1; i <= width; i++)
            {
                if (i < 10)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(i + " │");
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