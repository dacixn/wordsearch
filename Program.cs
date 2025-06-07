using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            matrix = new Matrix(20);
            display = new Display();
            words = new List<Word>();
            wordsFound = 0;
        }

        public void Start()
        {
            SetupWords();
            PlaceWords();
            matrix.FillGrid();

            SaveToFile("puzzle.txt");
            Console.Clear();
            Console.WriteLine("Puzzle has been saved to puzzle.txt");
            Console.WriteLine("Press any key to start playing...");
            Console.ReadKey(true);

            Play();
        }

        private void SetupWords()
        {
            Console.Clear();
            Console.WriteLine("Enter words to find, separated by a comma (e.g. hello,world,csharp):");
            string input = Console.ReadLine();
            string[] wordArray = input.Split(',');

            List<string> wordsToSave = new List<string>();
            foreach (string s in wordArray)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    wordsToSave.Add(s.Trim());
                }
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

        public void SaveToFile(string path)
        {
            List<string> lines = new List<string>();

            for (int y = 0; y < matrix.Width; y++)
            {
                string rowString = "";
                for (int x = 0; x < matrix.Width; x++)
                {
                    rowString += matrix.Grid[x, y] + " ";
                }
                lines.Add(rowString.TrimEnd());
            }

            lines.Add("");
            lines.Add("WORDS TO FIND:");
            foreach (Word word in words)
            {
                string startCoord = FormatCoordinate(word.StartX, word.StartY);
                string endCoord = FormatCoordinate(word.EndX, word.EndY);
                lines.Add($"{word.Text.ToUpper()} ({startCoord},{endCoord})");
            }

            File.WriteAllLines(path, lines);
        }

        private string FormatCoordinate(int x, int y)
        {
            char col = (char)('A' + x);
            int row = y + 1;
            return $"{col}{row}";
        }

        private void Play()
        {
            while (wordsFound < words.Count)
            {
                Console.Clear();
                display.DisplayLabels(matrix.Width);
                display.DisplayMatrix(matrix.Grid, matrix.Width, false, null);

                Console.SetCursorPosition(0, matrix.Width + 4);
                Console.WriteLine("WORDS TO FIND: (type 'cheat' to reveal answers)");
                foreach (Word word in words)
                {
                    if (word.Found)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(word.Text.ToUpper());
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(word.Text.ToUpper());
                    }
                }

                Console.WriteLine($"\nFound {wordsFound} of {words.Count}");
                Console.Write("Enter coordinates of a word (e.g., A1,F6): ");
                string guess = Console.ReadLine();

                if (guess.ToLower().Trim() == "cheat")
                {
                    Console.Clear();
                    display.DisplayLabels(matrix.Width);
                    display.DisplayMatrix(matrix.Grid, matrix.Width, true, matrix.IsWordCell);
                    Console.SetCursorPosition(0, matrix.Width + 4);
                    Console.WriteLine("Cheat mode activated. Press any key to continue playing.");
                    Console.ReadKey(true);
                    continue;
                }

                CheckGuess(guess);
            }

            Console.Clear();
            display.DisplayLabels(matrix.Width);
            display.DisplayMatrix(matrix.Grid, matrix.Width, false, null);
            Console.SetCursorPosition(0, matrix.Width + 4);
            Console.WriteLine("You found all the words");
        }

        private (int, int) ParseCoordinate(string coord)
        {
            if (string.IsNullOrEmpty(coord) || coord.Length < 2) return (-1, -1);
            coord = coord.ToUpper();
            char colChar = coord[0];
            if (!int.TryParse(coord.Substring(1), out int rowNum)) return (-1, -1);
            int x = colChar - 'A';
            int y = rowNum - 1;
            if (x < 0 || x >= matrix.Width || y < 0 || y >= matrix.Width) return (-1, -1);
            return (x, y);
        }

        private void CheckGuess(string input)
        {
            input = input.Replace(" ", "");
            string[] parts = input.Split(',');
            if (parts.Length != 2) return;
            var (startX, startY) = ParseCoordinate(parts[0]);
            var (endX, endY) = ParseCoordinate(parts[1]);
            if (startX == -1 || endX == -1) return;

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
                if (i < 10) Console.Write(" ");
                Console.WriteLine(i + " │");
            }
        }

        public void DisplayMatrix(string[,] matrix, int width, bool showCheat, bool[,] isWordCell)
        {
            Console.SetCursorPosition(5, 2);
            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (showCheat && isWordCell[x, y])
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(matrix[x, y] + " ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(matrix[x, y] + " ");
                    }
                }
                top++;
                Console.SetCursorPosition(left, top);
            }
        }

        public void DisplayLogo()
        {
            Console.Clear();
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

            double y = Math.Floor((double)(Console.WindowHeight / 2 - logo.Length / 2));
            double x = Math.Floor((double)(Console.WindowWidth / 2 - logo[0].Length / 2));

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
        public bool[,] IsWordCell { get; }

        public Matrix(int desiredWidth)
        {
            Width = desiredWidth;
            Grid = Generate();
            IsWordCell = new bool[Width, Width];
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

        public void InsertWord(string word, int x, int y, int dx, int dy)
        {
            string upperWord = word.ToUpper();
            char[] array = upperWord.ToCharArray();

            foreach (char c in array)
            {
                Grid[x, y] = c.ToString();
                IsWordCell[x, y] = true;
                x += dx;
                y += dy;
            }
        }

        public bool CheckForOverlaps(string word, int x, int y, int dx, int dy)
        {
            int currentX = x;
            int currentY = y;
            for (int i = 0; i < word.Length; i++)
            {
                if (currentX >= Width || currentY >= Width || currentX < 0 || currentY < 0) return true;
                if (Grid[currentX, currentY] != "." && Grid[currentX, currentY] != word[i].ToString().ToUpper()) return true;
                currentX += dx;
                currentY += dy;
            }
            return false;
        }

        public Word InsertWordRandom(string wordText)
        {
            Random random = new Random();
            int attempts = 0;
            while (attempts < 25)
            {
                int dx = random.Next(-1, 2);
                int dy = random.Next(-1, 2);
                if (dx == 0 && dy == 0)
                {
                    attempts++;
                    continue;
                }

                int x = random.Next(0, Width);
                int y = random.Next(0, Width);

                if (!CheckForOverlaps(wordText, x, y, dx, dy))
                {
                    InsertWord(wordText, x, y, dx, dy);
                    Word word = new Word
                    {
                        Text = wordText,
                        StartX = x,
                        StartY = y,
                        EndX = x + (dx * (wordText.Length - 1)),
                        EndY = y + (dy * (wordText.Length - 1))
                    };
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
