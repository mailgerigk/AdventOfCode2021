using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var lines = File.ReadAllLines("input.txt");
var numbers = lines.First().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
var boards = new List<Board>();

for (int i = 2; i < lines.Length; i += 6)
{
    var board = new Board();
    for (int j = 0; j < 5; j++)
    {
        var row = lines[i + j].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        for (int k = 0; k < row.Length; k++)
        {
            board.Fields[j, k].Number = row[k];
        }
    }
    boards.Add(board);
}

int firstScore = 0;
int lastScore = 0;
foreach (var number in numbers)
{
    foreach (var board in boards)
    {
        if (!board.Wins && board.Mark(number) && board.Wins)
        {
            firstScore = firstScore == 0 ? board.Unmarked * number : firstScore;
            lastScore = board.Unmarked * number;
        }
    }
}

Console.WriteLine(firstScore);
Console.WriteLine(lastScore);


class Field
{
    public int Number;
    public bool IsMarked;
}

class Board
{
    public Field[,] Fields = new Field[5, 5];

    public bool Wins
    {
        get
        {
            // left to right
            for (int row = 0; row < 5; row++)
            {
                bool all = true;
                for (int column = 0; column < 5; column++)
                {
                    if (!Fields[column, row].IsMarked)
                    {
                        all = false;
                        break;
                    }
                }
                if (all)
                {
                    return true;
                }
            }

            // top to bottom
            for (int column = 0; column < 5; column++)
            {
                bool all = true;
                for (int row = 0; row < 5; row++)
                {
                    if (!Fields[column, row].IsMarked)
                    {
                        all = false;
                        break;
                    }
                }
                if (all)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public int Unmarked
    {
        get
        {
            int sum = 0;
            for (int column = 0; column < 5; column++)
            {
                for (int row = 0; row < 5; row++)
                {
                    sum += Fields[column, row].IsMarked ? 0 : Fields[column, row].Number;
                }
            }
            return sum;
        }
    }

    public Board()
    {
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                Fields[column, row] = new Field();
            }
        }
    }

    public bool Mark(int number)
    {
        for (int row = 0; row < 5; row++)
        {
            for (int column = 0; column < 5; column++)
            {
                if (Fields[column, row].Number == number)
                {
                    Fields[column, row].IsMarked = true;
                    return true;
                }
            }
        }
        return false;
    }
}