using System;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");
//lines = @"0,9 -> 5,9
//8,0 -> 0,8
//9,4 -> 3,4
//2,2 -> 2,1
//7,0 -> 7,4
//6,4 -> 2,0
//0,9 -> 2,9
//3,4 -> 1,4
//0,0 -> 8,8
//5,5 -> 8,2".Split(Environment.NewLine);
var lines = input.Select(line => new Line(line)).ToArray();

// part1
var board = new Board();
foreach (var line in lines)
{
    if (!line.IsDiagonal)
        board.AddLine(line);
}
Console.WriteLine(board.CountGreaterThan1());

// part2
board = new Board();
foreach (var line in lines)
{
    board.AddLine(line);
}
Console.WriteLine(board.CountGreaterThan1());

class Point
{
    public int X;
    public int Y;
}

class Line
{
    public Point Start;
    public Point End;

    public bool IsDiagonal => Start.X != End.X && Start.Y != End.Y;

    public Line(string line)
    {
        var parts = line.Split(" -> ");
        var start = parts.First().Split(",").Select(int.Parse).ToArray();
        Start = new Point { X = start[0], Y = start[1] };
        var end = parts.Last().Split(",").Select(int.Parse).ToArray();
        End = new Point { X = end[0], Y = end[1] };
    }
}

class Board
{
    public int[,] Fields = new int[1000, 1000];

    public void AddLine(Line line)
    {
        var currentX = line.Start.X;
        var currentY = line.Start.Y;
        var distX = line.End.X - currentX;
        var distY = line.End.Y - currentY;

        Fields[line.Start.X, line.Start.Y]++;

        while (distX != 0 || distY != 0)
        {
            if(Math.Abs(distX) == Math.Abs(distY))
            {
                currentX += distX < 0 ? -1 : 1;
                currentY += distY < 0 ? -1 : 1;
            }
            else if (Math.Abs(distX) > Math.Abs(distY))
            {
                currentX += distX < 0 ? -1 : 1;
            }
            else
            {
                currentY += distY < 0 ? -1 : 1;
            }
            Fields[currentX, currentY]++;
            distX = line.End.X - currentX;
            distY = line.End.Y - currentY;
        }
    }

    public int CountGreaterThan1()
    {
        int sum = 0;
        for (int y = 0; y < 1000; y++)
        {
            for (int x = 0; x < 1000; x++)
            {
                sum += Fields[x, y] > 1 ? 1 : 0;
            }
        }
        return sum;
    }
}