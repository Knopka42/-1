using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static char[,] board;
    static char[,] hiddenBoard;
    static int size;
    static int pairsFound;
    static DateTime startTime;

    static void Main(string[] args)
    {
        Console.WriteLine("Введите размер поля (2n, где n - целое число): ");
        size = int.Parse(Console.ReadLine());
        InitializeGame();
        PlayGame();
    }

    static void InitializeGame()
    {
        board = new char[size, size];
        hiddenBoard = new char[size, size];
        List<char> symbols = GenerateSymbols(size * size / 2);
        ShuffleSymbols(symbols);

        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                board[i, j] = '#';

        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                hiddenBoard[i, j] = symbols[i * size + j];
    }

    static List<char> GenerateSymbols(int count)
    {
        List<char> symbols = new List<char>();
        for (char c = 'A'; c < 'A' + count; c++)
        {
            symbols.Add(c);
            symbols.Add(c); // добавляем пару
        }
        return symbols;
    }

    static void ShuffleSymbols(List<char> symbols)
    {
        Random rand = new Random();
        symbols = symbols.OrderBy(x => rand.Next()).ToList();
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                board[i, j] = symbols[i * size + j];
    }

    static void PlayGame()
    {
        startTime = DateTime.Now;
        while (pairsFound < (size * size) / 2)
        {
            DisplayBoard();
            Console.WriteLine("Выберите карточку (формат: x y): ");
            var input1 = Console.ReadLine().Split(' ');
            var firstCard = (int.Parse(input1[0]), int.Parse(input1[1]));

            DisplayCard(firstCard);
            DisplayBoard();

            Console.WriteLine("Выберите вторую карточку (формат: x y): ");
            var input2 = Console.ReadLine().Split(' ');
            var secondCard = (int.Parse(input2[0]), int.Parse(input2[1]));

            DisplayCard(secondCard);
            DisplayBoard();

            if (hiddenBoard[firstCard.Item1, firstCard.Item2] == hiddenBoard[secondCard.Item1, secondCard.Item2])
            {
                Console.WriteLine("Совпадение найдено!");
                pairsFound++;
            }
            else
            {
                Console.WriteLine("Нет совпадения. Попробуйте снова.");
                Thread.Sleep(2000); // Задержка для просмотра
                board[firstCard.Item1, firstCard.Item2] = '#';
                board[secondCard.Item1, secondCard.Item2] = '#';
            }
        }

        TimeSpan timeTaken = DateTime.Now - startTime;
        Console.WriteLine($"Поздравляем! Вы нашли все пары за {timeTaken.TotalSeconds} секунд.");
    }

    static void DisplayBoard()
    {
        Console.Clear();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static void DisplayCard((int, int) card)
    {
        board[card.Item1, card.Item2] = hiddenBoard[card.Item1, card.Item2];
    }
}


