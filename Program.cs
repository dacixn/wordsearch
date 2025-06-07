using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Security;

namespace WordSearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Display display = new Display();
            display.DisplayLogo();
            Console.ReadKey(true);

            GameLoop game = new GameLoop();
            game.Start();

            Console.WriteLine("\nGame Over! Press any key to exit.");
            Console.ReadKey();
        }
    }

    public class GameLoop
    {
        private Matrix matrix;
        private Display display;
        private List<Word> words;
        private int wordsFound;

        public GameLoop()
        {
            matrix = new Matrix(16);
            display = new Display();
            words = new List<Word>();
            wordsFound = 0;
        }

        public void Start()
        {
            SetupWords();
            PlaceWords();
            matrix.FillGrid();
            Play();
        }

        private void SetupWords()
        {
            Console.Clear();
            Console.WriteLine("Enter words to find separated by a comma (for example 'hello,world,csharp'):");
            string input = Console.ReadLine();
            string[] wordArray = input.Split(',');

            List<string> wordsToSave = new List<string>();
            foreach (string s in wordArray)
            {
                wordsToSave.Add(s.Trim());
            }

            File.WriteAllLines("words.txt", wordsToSave);
        }

        private void PlaceWords()
        {
            string[] wordsFromFile = File.ReadAllLines("words.txt");
            foreach (string w in wordsFromFile)
            {
                if (w.Length > 0)
                {
                    Word placedWord = matrix.InsertWordRandom(w);
                    if (placedWord != null)
                    {
                        words.Add(placedWord);
                    }
                }
            }
        }

        private void Play()
        {
            while (wordsFound < words.Count)
            {
                Console.Clear();
                display.DisplayLabels(matrix.Width);
                display.DisplayMatrix(matrix.Grid, matrix.Width);

                Console.SetCursorPosition(0, matrix.Width + 4);
                Console.WriteLine("Words:");
                foreach (Word word in words)
                {
                    if (word.Found)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(word.Text.ToUpper());
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(word.Text.ToUpper());
                    }
                }

                Console.WriteLine($"\nFound {wordsFound} of {words.Count}");
                Console.Write("Enter word coordinates (A1,F6): ");
                string guess = Console.ReadLine();
                CheckGuess(guess);
            }

            Console.Clear();
            display.DisplayLabels(matrix.Width);
            display.DisplayMatrix(matrix.Grid, matrix.Width);
            Console.SetCursorPosition(0, matrix.Width + 4);
            Console.WriteLine("You found all the words!");
        }

        private (int, int) ParseCoordinate(string coord)
        {
            if (string.IsNullOrEmpty(coord) || coord.Length < 2)
            {
                return (-1, -1);
            }

            coord = coord.ToUpper();
            char colChar = coord[0];
            string rowStr = coord.Substring(1);

            if (colChar < 'A' || colChar > 'Z' || !int.TryParse(rowStr, out int rowNum))
            {
                return (-1, -1);
            }

            int x = colChar - 'A';
            int y = rowNum - 1;

            if (x < 0 || x >= matrix.Width || y < 0 || y >= matrix.Width)
            {
                return (-1, -1);
            }

            return (x, y);
        }

        private void CheckGuess(string input)
        {
            input = input.Replace(" ", "");
            string[] parts = input.Split(',');

            if (parts.Length != 2)
            {
                return;
            }

            (int startX, int startY) = ParseCoordinate(parts[0]);
            (int endX, int endY) = ParseCoordinate(parts[1]);

            if (startX == -1 || endX == -1)
            {
                return;
            }

            foreach (Word word in words)
            {
                if (!word.Found)
                {
                    bool forwardMatch = (word.StartX == startX && word.StartY == startY && word.EndX == endX && word.EndY == endY);
                    bool reverseMatch = (word.EndX == startX && word.EndY == startY && word.StartX == endX && word.StartY == endY);

                    if (forwardMatch || reverseMatch)
                    {
                        word.Found = true;
                        wordsFound++;
                        return;
                    }
                }
            }
        }
    }

    public class Display
    {
        public void DisplayLabels(int width)
        {
            Console.Write("    ");

            for (int i = 0; i < width; i++)
            {

                char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                Console.Write(" " + alpha[i]);
            }

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
            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(matrix[x, y] + " ");
                }
                top++;
                Console.SetCursorPosition(left, top);
            }
        }

        public void DisplayLogo()
        {
            Console.Clear();
            double height = Console.WindowHeight;
            double width = Console.WindowWidth;

            string[] logo = new string[] {
                "                       _                           _     ",
                "                      | |                         | |    ",
                "__      _____  _ __ __| |  ___  ___  __ _ _ __ ___| |__  ",
                "\\ \\ /\\ / / _ \\| '__/ _` | / __|/ _ \\/ _` | '__/ __| '_ \\ ",
                " \\ V  V / (_) | | | (_| | \\__ \\  __/ (_| | | | (__| | | |",
                "  \\_/\\_/ \\___/|_|  \\__,_| |___/\\___|\\__,_|_|  \\___|_| |_|",
                "",
                "",
                "                 Press Any Key to Play"};

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

        public int[] WordMaxStart(string word)
        {
            int length = word.Length;
            int width = Width;

            int maximumX = width - length;
            int maximumY = width - length;

            return new int[] { maximumX, maximumY };
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

        public bool CheckForOverlaps(string word, int x, int y, int dx, int dy)
        {
            if (word.Length > Width)
            {
                Console.WriteLine($"Word '{word}' is larger than puzzle width ({word.Length} > {Width})");
                return true;
            }

            int currentX = x;
            int currentY = y;

            for (int i = 0; i < word.Length; i++)
            {
                if (currentX >= Width || currentY >= Width || currentX < 0 || currentY < 0)
                {
                    return true;
                }

                if (Grid[currentX, currentY] != "." && Grid[currentX, currentY] != word[i].ToString().ToUpper())
                {
                    return true;
                }

                currentX += dx;
                currentY += dy;
            }

            return false;
        }

        public Word InsertWordRandom(string wordText)
        {
            Random random = new Random();
            int attempts = 0;

            while (attempts < 500)
            {
                int dx = 0;
                int dy = 0;
                while (dx == 0 && dy == 0)
                {
                    dx = random.Next(0, 2);
                    dy = random.Next(0, 2);
                }

                int x = random.Next(0, Width);
                int y = random.Next(0, Width);

                if (!CheckForOverlaps(wordText, x, y, dx, dy))
                {
                    InsertWord(wordText, x, y, dx, dy);

                    Word word = new Word();
                    word.Text = wordText;
                    word.StartX = x;
                    word.StartY = y;
                    word.EndX = x + (dx * (wordText.Length - 1));
                    word.EndY = y + (dy * (wordText.Length - 1));

                    return word;
                }

                attempts++;
            }

            Console.WriteLine($"Could not place word: {wordText}");
            return null;
        }

        public void FillGrid()
        {
            Random random = new Random();
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            for (int y = 0; y < Width; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Grid[x, y] == ".")
                    {
                        Grid[x, y] = alpha[random.Next(0, alpha.Length)].ToString();
                    }
                }
            }
        }
    }

    public class Word
    {
        public string Text { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public bool Found { get; set; }

        public Word()
        {
            Found = false;
        }
    }
}
