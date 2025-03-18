using System.Text;

namespace TSP;

public struct Graph
{
    public String Name;
    public String Type;
    public int Dimension;
    public Node[] Nodes;
    
    public Graph(String name, String type, int dimension, Node[] nodes)
    {
        Name = name;
        Type = type;
        Dimension = dimension;
        Nodes = nodes;
    }
    
    public String ToString()
    {
        StringBuilder result = new StringBuilder();
        result.Append($"Graph name: {Name}  Type: {Type} with {Dimension} nodes:\n");
        for(int i = 0; i < Nodes.Length; i++)
        {
            result.Append($"{Nodes[i].ToString()}\n");
        }
        return result.ToString();
    }
}