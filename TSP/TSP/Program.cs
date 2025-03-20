using TSP;
using TSP.Algorithm;

String basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "Graphs\\";
Graph graph = GraphReader.ReadGraph(basePath + "a280.tsp");
Console.WriteLine(graph.ToString());

Console.WriteLine("Starting Genetic Algorithm...");

int chromosomeLength = 10;  
int populationSize = 100;   
int selectionPercent = 50;  
int mutationRate = 20;      
int maxGenerations = 1000;  

GeneticAlgorithm ga = new GeneticAlgorithm(
    chromosomeLength, populationSize, selectionPercent, mutationRate, maxGenerations
);

Chromosome bestSolution = ga.Start();

Console.WriteLine("Best Chromosome Found:");
Console.WriteLine($"Genome: {string.Join(", ", bestSolution.Genome)}");
Console.WriteLine($"Fitness: {bestSolution.Fitness}");

Console.WriteLine("Genetic Algorithm Finished.");