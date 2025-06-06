using System;
using System.ComponentModel;
using System.Security;

namespace WordSearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Matrix matrix = new Matrix(16);
            Display display = new Display();

            Console.SetWindowSize(60, 30);

            matrix.InsertWord("hello", 2, 2, 0, 1);
            display.DisplayLabels(matrix.Width);
            display.DisplayMatrix(matrix.Grid, matrix.Width);
            Console.ReadKey();
            display.DisplayLogo();
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

        public void DisplayLogo()
        {
            Console.Clear();
            double height = Console.WindowHeight;
            double width = Console.WindowWidth;

            string[] logo = [
                "                       _                           _     ",
                "                      | |                         | |    ",
                "__      _____  _ __ __| |  ___  ___  __ _ _ __ ___| |__  ",
                "\\ \\ /\\ / / _ \\| '__/ _` | / __|/ _ \\/ _` | '__/ __| '_ \\ ",
                " \\ V  V / (_) | | | (_| | \\__ \\  __/ (_| | | | (__| | | |",
                "  \\_/\\_/ \\___/|_|  \\__,_| |___/\\___|\\__,_|_|  \\___|_| |_|",
                "",
                "",
                "                 Press Any Key to Play"];

            double y = Math.Floor(height / 2 - 9 / 2);
            double x = Math.Floor(width / 2 - 57 / 2);

            foreach (string line in logo)
            {
                Console.SetCursorPosition((int)x, (int)y);
                Console.WriteLine(line);
                y++;
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
        public void MaxStart()
        {
            //def calculate_word_max_start(word):
            //max_x = 24 + 1 - len(word)
            //max_y = 24 + 1 - len(word)
            //max_coords = [max_x, max_y]
            //return max_coords
        }
        public void InsertWord(string word, int x, int y, int dx, int dy)
        {
            string upperWord = word.ToUpper();
            char[] array = upperWord.ToCharArray();

            foreach (char c in array)
            {
                Grid[x, y] = c.ToString();
                x = x + dx;
                y = y + dy;
            }
        }
    }

    public class Word
    {
        public Word()
        {

        }
    }
}