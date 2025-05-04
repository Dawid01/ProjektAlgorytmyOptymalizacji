namespace TSP;

public class GeneticAlgorithm<T>
{
    private readonly IGeneticProblem<T> _problem;
    private readonly int _populationSize;
    private readonly int _generations;
    private readonly double _mutationRate;

    public GeneticAlgorithm(IGeneticProblem<T> problem, int populationSize, int generations, double mutationRate)
    {
        _problem = problem;
        _populationSize = populationSize;
        _generations = generations;
        _mutationRate = mutationRate;
    }

    public T Run()
    {
        List<T> population = _problem.GenerateInitialPopulation(_populationSize);

        for (int generation = 0; generation < _generations; generation++)
        {
            population = population.OrderByDescending(ind => _problem.EvaluateFitness(ind)).ToList();
            if (_problem.ShouldStop(population, generation))
                return population.First();

            List<T> newPopulation = new List<T> { population.First() };

            while (newPopulation.Count < _populationSize)
            {
                T parent1 = population[new Random().Next(population.Count / 2)];
                T parent2 = population[new Random().Next(population.Count / 2)];
                T child = _problem.Crossover(parent1, parent2);

                if (new Random().NextDouble() < _mutationRate)
                    child = _problem.Mutate(child);

                newPopulation.Add(child);
            }

            population = newPopulation;
        }

        return population.First();
    }

}