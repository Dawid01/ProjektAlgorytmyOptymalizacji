namespace TSP;

public interface IGeneticProblem<T>
{
    List<T> GenerateInitialPopulation(int size);
    double EvaluateFitness(T individual);
    T Crossover(T parent1, T parent2);
    T Mutate(T individual);
    bool ShouldStop(List<T> population, int generation);
}