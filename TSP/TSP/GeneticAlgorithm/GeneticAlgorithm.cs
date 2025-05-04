namespace TSP;


public enum CrossoverType
{
    OX,
    PMX, 
    CX,
    UX
}
public class GeneticAlgorithm<T>
{
    private readonly IGeneticProblem<T> _problem;
    private readonly int _populationSize;
    private readonly int _generations;
    private readonly double _mutationRate;
    private readonly double _crossoverRate;
    private readonly int _eliteCount;
    private readonly CrossoverType _crossoverType;

    public GeneticAlgorithm(IGeneticProblem<T> problem, int populationSize, int generations, double mutationRate, double crossoverRate, int eliteCount, CrossoverType crossoverType)
    {
        _problem = problem;
        _populationSize = populationSize;
        _generations = generations;
        _mutationRate = mutationRate;
        _crossoverRate = crossoverRate;
        _eliteCount = eliteCount;
        _crossoverType = crossoverType;

    }

    public T Run()
    {
        List<T> population = _problem.GenerateInitialPopulation(_populationSize);

        for (int generation = 0; generation < _generations; generation++)
        {
            population = population.OrderByDescending(ind => _problem.EvaluateFitness(ind)).ToList();
            if (_problem.ShouldStop(population, generation))
                return population.First();

            List<T> newPopulation = population.Take(_eliteCount).ToList();
            
            while (newPopulation.Count < _populationSize)
            {
                T parent1 = population[new Random().Next(population.Count / 2)];
                T parent2 = population[new Random().Next(population.Count / 2)];
                
                T child;

                if (new Random().NextDouble() < _crossoverRate)
                {
                    child = _problem.Crossover(parent1, parent2, _crossoverType);
                }
                else
                {
                    child = (new Random().Next(2) == 0) ? parent1 : parent2;
                }

                if (new Random().NextDouble() < _mutationRate)
                    child = _problem.Mutate(child);

                newPopulation.Add(child);
            }

            population = newPopulation;
        }

        return population.First();
    }

}