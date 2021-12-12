using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

var lines = File.ReadAllLines("input.txt");

var nodes = new List<Node>();
foreach (var line in lines)
{
    var parts = line.Split("-");

    var name1 = parts.First();
    var node1 = nodes.FirstOrDefault(node => node.Name == name1);
    if (node1 is null)
    {
        node1 = new Node(name1);
        nodes.Add(node1);
    }

    var name2 = parts.Last();
    var node2 = nodes.FirstOrDefault(node => node.Name == name2);
    if (node2 is null)
    {
        node2 = new Node(name2);
        nodes.Add(node2);
    }

    node1.Connections.Add(node2);
    node2.Connections.Add(node1);
}

var start = nodes.First(node => node.Name == "start");
var end = nodes.First(node => node.Name == "end");

var queue = new Queue<(Node, List<Node>)>();
queue.Enqueue((start, new List<Node>()));

var paths = new List<Path>();

while (queue.Any())
{
    var (node, path) = queue.Dequeue();

    if (node.IsSmall && path.Contains(node))
        continue;

    path.Add(node);

    if (node == end)
    {
        paths.Add(new Path(path));
        continue;
    }

    foreach (var connection in node.Connections)
    {
        queue.Enqueue((connection, new List<Node>(path)));
    }
}

Console.WriteLine(paths.Count);

// part2
queue.Enqueue((start, new List<Node>()));
paths.Clear();

Func<List<Node>, Node> small2 = (List<Node> nodes) =>
{
    foreach (var node in nodes)
        if (node.IsSmall && nodes.Count(n => node == n) == 2)
            return node;
    return null;
};

while (queue.Any())
{
    var (node, path) = queue.Dequeue();

    var s2 = small2(path);
    if ((node.IsSpecial && path.Contains(node)) || s2 == node || s2 is not null && node.IsSmall && path.Contains(node))
        continue;

    path.Add(node);

    if (node == end)
    {
        paths.Add(new Path(path));
        continue;
    }

    foreach (var connection in node.Connections)
    {
        queue.Enqueue((connection, new List<Node>(path)));
    }
}
Console.WriteLine(paths.Count);

class Node
{
    public string Name { get; init; }
    public bool IsBig => Name.All(c => char.IsUpper(c));
    public bool IsSmall => !IsBig;
    public bool IsSpecial => Name == "start" || Name == "end";
    public List<Node> Connections { get; } = new List<Node>();

    public Node(string name)
    {
        Name = name;
    }

    public override string ToString() => Name;
}

class Path
{
    public ImmutableArray<Node> Nodes { get; init; }
    public Node Start => Nodes.First();
    public Node End => Nodes.Last();
    public Path(IEnumerable<Node> nodes)
    {
        Nodes = nodes.ToImmutableArray();
    }
}
