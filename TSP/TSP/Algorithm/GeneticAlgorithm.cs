using System.Diagnostics;

namespace TSP.Algorithm;

public class GeneticAlgorithm
{
    private readonly Graph _graph;
    private readonly Random _random = new();
    public int ChromosomeLength { get; set; }
    public int PopulationLength { get; set; }
    public int SelectionPercent { get; set; }
    public int MutationProbability { get; set; }
    public int RegenerationLimit { get; set; }
    public int RegenerationCounter { get; set; }
    public int ConvergenceRate { get; set; }
    public Chromosome[] Population { get; set; }

    public GeneticAlgorithm(Graph graph, int len, int popLength = 100, int selectionPercent = 50,
        int mutationRate = 20, int maxRegenerationCount = int.MaxValue, int convergenceRate = 60)
    {
        _graph = graph;
        ChromosomeLength = len;
        PopulationLength = popLength;
        SelectionPercent = selectionPercent;
        MutationProbability = mutationRate;
        RegenerationLimit = maxRegenerationCount;
        RegenerationCounter = 0;
        ConvergenceRate = convergenceRate;

        Population = Enumerable.Range(0, PopulationLength)
            .Select(_ =>
            {
                var c = new Chromosome(ChromosomeLength).Randomize();
                c.Evaluate(_graph);
                return c;
            }).ToArray();
    }

    public Chromosome Start()
    {
        while (Evaluation())
        {
            Selection(SelectionPercent);
            Regeneration();
        }

        return Population.First();
    }

    public bool Evaluation()
    {
        foreach (var c in Population)
            c.Evaluate(_graph);

        Population = Population.OrderBy(p => p.Fitness).ToArray();
        var best = Population.First();

        if (RegenerationCounter >= RegenerationLimit)
        {
            Debug.WriteLine("GA ended: Max generations reached!");
            return false;
        }

        if (Population.Count(c => Math.Abs(c.Fitness - best.Fitness) < 1) >=
            Math.Min((double)ConvergenceRate / 100, 0.9) * PopulationLength)
        {
            Debug.WriteLine("GA ended: Convergence detected!");
            return false;
        }

        return true;
    }

    public void Selection(int percent)
    {
        int keep = Math.Max(2, percent * PopulationLength / 100);
        Population = Population.Take(keep).ToArray();
        Regeneration();
    }

    public void Regeneration()
    {
        RegenerationCounter++;
        if (RegenerationCounter % 10 == 0)
            Debug.WriteLine($"Generation {RegenerationCounter}, Best Fitness: {Population[0].Fitness}");

        var newPop = new List<Chromosome>();
        while (Population.Length + newPop.Count < PopulationLength)
        {
            var (mom, dad) = GetRandomParents();
            var child = Crossover(mom, dad);
            Mutation(child);
            child.Evaluate(_graph);
            newPop.Add(child);
        }

        Population = Population.Concat(newPop).ToArray();
    }

    public void Mutation(Chromosome chromosome)
    {
        if (_random.Next(100) <= MutationProbability)
        {
            int a = _random.Next(0, chromosome.Lenght);
            int b = _random.Next(0, chromosome.Lenght);
            (chromosome.Genome[a], chromosome.Genome[b]) = (chromosome.Genome[b], chromosome.Genome[a]);
        }
    }

    public Chromosome Crossover(Chromosome mom, Chromosome dad)
    {
        var child = new Chromosome(ChromosomeLength)
        {
            Genome = PMX(mom.Genome, dad.Genome)
        };
        return child;
    }

    private (Chromosome mom, Chromosome dad) GetRandomParenzts()
    {
        int i = _random.Next(Population.Length);
        int j;
        do j = _random.Next(Population.Length); while (i == j);
        return (Population[i], Population[j]);
    }

    private T[] PMX<T>(T[] p1, T[] p2)
    {
        int size = p1.Length;
        int cut1 = _random.Next(1, size - 2);
        int cut2 = _random.Next(cut1 + 1, size - 1);

        T[] child = new T[size];
        Array.Fill(child, default);

        for (int i = cut1; i < cut2; i++)
            child[i] = p1[i];

        for (int i = 0; i < size; i++)
        {
            if (i >= cut1 && i < cut2) continue;
            T gene = p2[i];
            while (child.Contains(gene))
            {
                int index = Array.IndexOf(p2, gene);
                gene = p1[index];
            }
            child[i] = gene;
        }

        return child;
    }
}
