using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace TSP.Algorithm;

public class GeneticAlgorithm
{
    private Random random = new Random();
    public int ChromosomeLength { get; set; }
    public int PopulationLength { get; set; }
    public int SelectionPercent { get; set; }
    public int MutationProbability { get; set; }
    public int RegenerationLimit { get; set; }
    public int RegenerationCounter { get; set; }
    public int ConvergenceRate { get; set; }
    public Chromosome[] Population { get; set; }
    
    public GeneticAlgorithm(int len, int? popLength = null,
        int? selectionPercent = null, int? mutationRate = null,
        int? maxRegenerationCount = null, int? convergenceRate = null){
        ChromosomeLength = len;
        PopulationLength = popLength ?? 100;
        SelectionPercent = selectionPercent ?? 50;
        MutationProbability = mutationRate ?? 20;
        RegenerationLimit = maxRegenerationCount ?? int.MaxValue;
        RegenerationCounter = 0;
        ConvergenceRate = convergenceRate ?? 60;

        Population = Enumerable.Range(0, PopulationLength)
            .Select(r => new Chromosome(ChromosomeLength).Randomize())
            .ToArray();
    }

    public Chromosome Start()
    {
        while (Evaluation())
        {
            Selection(SelectionPercent);
        }

        return Population.First();
    }

    public bool Evaluation()
    {
        //sort population by fitness
        Population = Population.OrderByDescending(p => p.Fitness).ToArray();
        var bestChromosome = Population.First();

        if (Math.Abs(bestChromosome.Fitness) < 2)
        {
            Debug.WriteLine("GA ended: Best chromosome found!");
            return false;
        }

        if (RegenerationCounter >= RegenerationLimit)
        {
            Debug.WriteLine("GA ended: Maximum generations reached!");
            return false;
        }

        if (Population.Count(c => Math.Abs(c.Fitness - bestChromosome.Fitness) < 1) >= 
            Math.Min((double)ConvergenceRate / 100, 0.9) * PopulationLength)
        {
            Debug.WriteLine("GA ended: Convergence detected!");
            return false;
        }

        return true;
    }

    public void Selection(int percent)
    {
        int keepCount = percent * PopulationLength / 100;
        Population = Population.Take(keepCount).ToArray(); // Keep best solutions
        Regeneration(); // Create new generation
    }
    
    public void Regeneration()
    {
        RegenerationCounter++;

        if (RegenerationCounter % 100 == 0)
            Debug.WriteLine($"Generation {RegenerationCounter}, Best Fitness: {Population[0].Fitness}");

        var newPopulation = new List<Chromosome>();

        for (var index = Population.Length; index < PopulationLength; index++)
        {
            var parent = GetRandomParent();
            var child = Crossover(parent.mom, parent.dad);
            Mutation(child, MutationProbability);
            child.Evaluate();
            newPopulation.Add(child);
        }

        Population = Population.Concat(newPopulation).ToArray(); // Add new offspring
    }

    public void Mutation(Chromosome chromosome, int rate)
    {
        if (random.Next(0, 100) <= rate)
        {
            int gene1 = random.Next(0, chromosome.Lenght - 1);
            int gene2 = random.Next(0, chromosome.Lenght - 1);

            if (gene1 != gene2)
            {
                var temp = chromosome.Genome[gene1];
                chromosome.Genome[gene1] = chromosome.Genome[gene2];
                chromosome.Genome[gene2] = temp;
            }
        }
    }
    protected T[] PerformPMX<T>(T[] parent1, T[] parent2)
    {
        int cut1 = random.Next(1, parent1.Length / 2);   
        int cut2 = random.Next(cut1 + 1, parent1.Length - 1);  
        var child = new T[parent1.Length];
        var usedGenes = new HashSet<T>();

        // Copy segment from Parent1
        for (int i = cut1; i <= cut2; i++)
        {
            child[i] = parent1[i];
            usedGenes.Add(parent1[i]); 
        }

        // Identify empty spots and fill from Parent2
        var emptyIndices = Enumerable.Range(0, child.Length)
            .Where(i => i < cut1 || i > cut2)
            .ToList();

        foreach (var gene in parent2.Concat(parent1))
        {
            if (emptyIndices.Count == 0) break;
            if (!usedGenes.Contains(gene))
            {
                child[emptyIndices.First()] = gene;
                emptyIndices.RemoveAt(0);
            }
        }

        return child;
    }

    protected (Chromosome mom, Chromosome dad) GetRandomParent()
    {
        int rand1 = random.Next(0, Population.Length);
        int rand2;

        do
        {
            rand2 = random.Next(0, Population.Length);
        } while (rand1 == rand2); // Ensure different parents

        return (Population[rand1], Population[rand2]);
    }


    public Chromosome Crossover(Chromosome mom, Chromosome dad)
    {
        var child = new Chromosome(ChromosomeLength)
        {
            Genome = PerformPMX(mom.Genome, dad.Genome)
        };

        return child;
    }


}

