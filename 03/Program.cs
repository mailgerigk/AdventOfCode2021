using System;
using System.IO;
using System.Linq;

static relation get_most_common(string[] lines, int index)
{
    var sum = lines.Sum(line => line[index] == '1' ? 1 : 0);
    if (sum == lines.Length / 2 && (lines.Length % 2) == 0)
        return relation.equal;
    if (sum < (lines.Length + 1) / 2)
        return relation.zero;
    return relation.one;
}

static string[] remove(string[] lines, int index, int value) => lines.Where(line => line[index] == value).ToArray();

static int bin_to_int(string s)
{
    int v = 0;
    foreach (var c in s)
    {
        v <<= 1;
        v |= c == '1' ? 1 : 0;
    }
    return v;
}

var lines = File.ReadAllLines("input.txt");
var line_length = lines.First().Length;

var oxy = lines.ToArray();
var co = lines.ToArray();

for (int i = 0; i < line_length; i++)
{
    if (oxy.Length > 1)
    {
        var orel = get_most_common(oxy, i);
        switch (orel)
        {
            case relation.one:
                oxy = remove(oxy, i, '1');
                break;
            case relation.zero:
                oxy = remove(oxy, i, '0');
                break;
            case relation.equal:
                oxy = remove(oxy, i, '1');
                break;
            default:
                break;
        }
    }

    if (co.Length > 1)
    {
        var crel = get_most_common(co, i);
        switch (crel)
        {
            case relation.one:
                co = remove(co, i, '0');
                break;
            case relation.zero:
                co = remove(co, i, '1');
                break;
            case relation.equal:
                co = remove(co, i, '0');
                break;
            default:
                break;
        }
    }
}

var o = bin_to_int(oxy.First());
var c = bin_to_int(co.First());
Console.WriteLine(o * c);

enum relation
{
    one,
    zero,
    equal,
}