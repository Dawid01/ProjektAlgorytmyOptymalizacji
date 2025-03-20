using System.Data;

namespace TSP.Algorithm;

public class Chromosome
{
    public int[] Genome { get; set; }
    public double Fitness { get; private set; }
    public int Lenght => Genome.Length;

    public Chromosome(int lenght)
    {
        Genome = new int[lenght];
    }

    public Chromosome Randomize()
    {
        Random rnd = new Random();
        Genome = Enumerable.Range(0, Genome.Length).OrderBy(x => rnd.Next()).ToArray();
        Evaluate();
        return this;
    }

    public void Evaluate()
    {
        Fitness = Genome.Sum();
    }
}