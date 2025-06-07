namespace TSP.Algorithm;

public class Chromosome
{
    public int[] Genome { get; set; }
    public double Fitness { get; set; }
    public int Lenght => Genome.Length;

    public Chromosome(int length)
    {
        Genome = new int[length];
    }

    public Chromosome Randomize()
    {
        Genome = Enumerable.Range(0, Genome.Length).OrderBy(_ => Guid.NewGuid()).ToArray();
        return this;
    }

    public void Evaluate(Graph graph)
    {
        Fitness = 0;
        for (int i = 0; i < Genome.Length - 1; i++)
        {
            Fitness += graph.Nodes[Genome[i]].DistanceTo(graph.Nodes[Genome[i + 1]]);
        }
        Fitness += graph.Nodes[Genome[^1]].DistanceTo(graph.Nodes[Genome[0]]); // powrót
    }
}