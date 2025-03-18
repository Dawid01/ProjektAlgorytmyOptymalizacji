using System.Text.RegularExpressions;

namespace TSP;

public class GraphReader
{
    public static Graph ReadGraph(String path)
    {
        String[] lines = File.ReadAllLines(path);
        String name = lines[0].Split(": ")[1];
        String type = lines[1].Split(": ")[1];
        int dimension = Int32.Parse(lines[3].Split(": ")[1]);
        Node[] nodes = new Node[dimension];
        for(int i = 0; i < dimension; i++)
        {
            
            if (string.IsNullOrEmpty(lines[6 + i]))
            {
                continue;
            }
            string line = lines[6 + i].Trim();
            string[] parts = Regex.Split(line, @"\s+");
            int index = Int32.Parse(parts[0]);
            int x = Int32.Parse(parts[1]);
            int y = Int32.Parse(parts[2]);
            nodes[i] = new Node(index, x, y);
        }
        return new Graph(name, type, dimension, nodes);
    }
}