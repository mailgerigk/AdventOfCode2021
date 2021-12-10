using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

var lines = File.ReadAllLines("input.txt");
var errorScore = lines.Select(findErrorScore).Sum();
Console.WriteLine(errorScore);

var autoCompleteScores = lines.Select(findAutoCompleteScore).Where(score => score >0).ToList();
autoCompleteScores.Sort();
Console.WriteLine(autoCompleteScores[autoCompleteScores.Count / 2]);

int findErrorScore(string line)
{
    var stack = new Stack<int>();
    foreach (var c in line)
    {
        if (isOpen(c))
            stack.Push(c);
        else if (isClose(c))
        {
            if (stack.Peek() == getOpen(c))
                stack.Pop();
            else
                return getScore(c);
        }
    }
    return 0;

    static bool isOpen(int c) => c switch
    {
        '(' or '{' or '[' or '<' => true,
        _ => false
    };

    static bool isClose(int c) => c switch
    {
        ')' or '}' or ']' or '>' => true,
        _ => false
    };

    static int getOpen(int c) => c switch
    {
        ')' => '(',
        '}' => '{',
        ']' => '[',
        '>' => '<',
        _ => throw new ArgumentException()
    };

    static int getScore(int c) => c switch
    {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => throw new ArgumentException(),
    };
}

ulong findAutoCompleteScore(string line)
{
    var stack = new Stack<int>();
    foreach (var c in line)
    {
        if (isOpen(c))
            stack.Push(c);
        else if (isClose(c))
        {
            if (stack.Peek() == getOpen(c))
                stack.Pop();
            else
                return 0;
        }
    }
    var score = 0ul;
    while(stack.Any())
    {
        score *= 5;
        var c = stack.Pop();
        score += getScore(c);
    }
    return score;

    static bool isOpen(int c) => c switch
    {
        '(' or '{' or '[' or '<' => true,
        _ => false
    };

    static bool isClose(int c) => c switch
    {
        ')' or '}' or ']' or '>' => true,
        _ => false
    };

    static int getOpen(int c) => c switch
    {
        ')' => '(',
        '}' => '{',
        ']' => '[',
        '>' => '<',
        _ => throw new ArgumentException()
    };

    static ulong getScore(int c) => c switch
    {
        '(' => 1,
        '[' => 2,
        '{' => 3,
        '<' => 4,
        _ => throw new ArgumentException(),
    };
}