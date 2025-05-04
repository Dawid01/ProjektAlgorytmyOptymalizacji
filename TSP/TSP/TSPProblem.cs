namespace TSP;

public class TSPProblem : IGeneticProblem<List<int>>
{
    private readonly Graph _graph;
    private readonly Random _random = new Random();

    public TSPProblem(Graph graph, int seed = -1)
    {
        _graph = graph;
        if (seed != -1)
            _random = new Random(seed);
    }

    public List<List<int>> GenerateInitialPopulation(int size)
    {
        List<List<int>> population = new List<List<int>>();
        
        for (int i = 0; i < size; i++)
        {
            List<int> route = Enumerable.Range(0, _graph.Dimension).OrderBy(x => _random.Next()).ToList();
            population.Add(route);
        }
        
        return population;
    }

    public double EvaluateFitness(List<int> individual)
    {
        double distance = 0;
        
        for (int i = 0; i < individual.Count - 1; i++)
        {
            distance += GetDistance(_graph.Nodes[individual[i]], _graph.Nodes[individual[i + 1]]);
        }

        distance += GetDistance(_graph.Nodes[individual.Last()], _graph.Nodes[individual.First()]);
        return 1 / distance;
    }
    
    public List<int> Crossover(List<int> parent1, List<int> parent2, CrossoverType crossoverType)
    {
        switch (crossoverType)
        {
            case CrossoverType.OX:
                return CrossoverOX(parent1, parent2);
            case CrossoverType.PMX:
                return CrossoverPMX(parent1, parent2);
            case CrossoverType.CX:
                return CrossoverCX(parent1, parent2);
            case CrossoverType.UX:
                return CrossoverUX(parent1, parent2);
            default:
                return CrossoverOX(parent1, parent2);
        }
    }
    
    // Crossover z OX (Order Crossover)
    public List<int> CrossoverOX(List<int> parent1, List<int> parent2)
    {
        int size = parent1.Count;
        List<int> child = new List<int>(new int[size]);
    
        int start = _random.Next(size / 2);
        int end = _random.Next(start, size);
    
        for (int i = start; i < end; i++)
            child[i] = parent1[i];
    
        int index = 0;
        for (int i = 0; i < size; i++)
        {
            if (!child.Contains(parent2[i]))
            {
                while (child[index] != 0) index++;
                child[index] = parent2[i];
            }
        }
    
        return child;
    }
    
    
    // Crossover z PMX (Partially Matched Crossover)
    public List<int> CrossoverPMX(List<int> parent1, List<int> parent2)
    {
        int size = parent1.Count;
        List<int> child = new List<int>(new int[size]);

        int start = _random.Next(size / 2);
        int end = _random.Next(start, size);

        for (int i = start; i < end; i++)
        {
            child[i] = parent1[i];
        }

        for (int i = 0; i < size; i++)
        {
            if (!child.Contains(parent2[i]))
            {
                int idx = (i < start || i >= end) ? i : -1;

                if (idx == -1)
                {
                    idx = 0;
                    while (child[idx] != 0)
                        idx = (idx + 1) % size; 
                }
        
                child[idx] = parent2[i];
            }
        }
        
        return child;
    }
    
    
    // Crossover z CX (Cycle Crossover)
    public List<int> CrossoverCX(List<int> parent1, List<int> parent2)
    {
        int size = parent1.Count;
        List<int> child = new List<int>(new int[size]);
        bool[] visited = new bool[size];

        int i = 0;
        int childIndex = 0;
    
        while (childIndex < size)
        {
            if (!visited[childIndex])
            {
                child[childIndex] = parent1[childIndex];
                visited[childIndex] = true;

                int nextIndex = parent2.IndexOf(parent1[childIndex]);
                while (nextIndex != childIndex)
                {
                    child[nextIndex] = parent1[nextIndex];
                    visited[nextIndex] = true;
                    nextIndex = parent2.IndexOf(parent1[nextIndex]);
                }
            }
            childIndex++;
        }

        return child;
    }

    
    // Crossover z UX (Uniform Crossover)
    public List<int> CrossoverUX(List<int> parent1, List<int> parent2)
    {
        int size = parent1.Count;
        List<int> child = new List<int>(new int[size]);

        for (int i = 0; i < size; i++)
        {
            child[i] = (_random.Next(2) == 0) ? parent1[i] : parent2[i];
        }

        return child;
    }



    public List<int> Mutate(List<int> individual)
    {
        int a = _random.Next(individual.Count);
        int b = _random.Next(individual.Count);
        
        if (a != b)
        {
            (individual[a], individual[b]) = (individual[b], individual[a]);
        }        
        return individual;
    }

    public bool ShouldStop(List<List<int>> population, int generation)
    {
        return generation > 10000;
    }

    private double GetDistance(Node a, Node b)
    {
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }
}