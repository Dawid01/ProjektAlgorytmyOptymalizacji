using System.Globalization;
using System.Text.RegularExpressions;

namespace TSP;

public class GraphReader
{
    public static Graph ReadGraphTSP(string path)
    {
        string[] lines = File.ReadAllLines(path);
        string name = "";
        string type = "";
        int dimension = 0;
        int nodeSectionIndex = -1;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (line.StartsWith("NODE_COORD_SECTION"))
            {
                nodeSectionIndex = i + 1;
                break;
            }

            if (line.StartsWith("NAME"))
            {
                name = line.Split(':')[1].Trim();
            }
            else if (line.StartsWith("TYPE"))
            {
                type = line.Split(':')[1].Trim();
            }
            else if (line.StartsWith("DIMENSION"))
            {
                dimension = int.Parse(line.Split(':')[1].Trim());
            }
        }

        if (nodeSectionIndex == -1)
        {
            throw new Exception("Brak sekcji NODE_COORD_SECTION w pliku.");
        }

        Node[] nodes = new Node[dimension];

        for (int i = 0; i < dimension; i++)
        {
            string line = lines[nodeSectionIndex + i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = Regex.Split(line, @"\s+");
            int index = int.Parse(parts[0]);
            double x = double.Parse(parts[1], CultureInfo.InvariantCulture);
            double y = double.Parse(parts[2], CultureInfo.InvariantCulture);

            nodes[i] = new Node(index, x, y);
        }

        return new Graph(name, type, dimension, nodes);
    }

    public struct VRPData
    {
        public Graph Graph { get; set; }
        public int VehicleCount { get; set; }
        public int VehicleCapacity { get; set; }
        public Dictionary<int, int> Demands { get; set; }
    }

    // DO DOKONCZENIA
    public static VRPData ReadGraphVRP(string path)
    {
        return new VRPData();
    }

    
}