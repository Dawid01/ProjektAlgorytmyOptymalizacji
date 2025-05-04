using TSP;

String basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "TSP_DATA\\data\\";
//Console.WriteLine(graph.ToString());





string[] graphs = Directory.GetFiles(basePath, "*", SearchOption.TopDirectoryOnly);


for (int i = 0; i < 5; i++)
{
    Graph graph = GraphReader.ReadGraph(graphs[i]);
    var tspProblem = new TSPProblem(graph);
    var ga = new GeneticAlgorithm<List<int>>(tspProblem, 100, 1000, 20);
    List<int> bestSolution = ga.Run();
    Console.WriteLine($"{graph.Name}, Dystans: {1/(tspProblem.EvaluateFitness(bestSolution))}");
}



// for (int popSize = 100; popSize <= 1000; popSize += 100)
// {
//     var ga = new GeneticAlgorithm<List<int>>(tspProblem, popSize, 1000, 0.05);
//     List<int> bestSolution = ga.Run();
//     Console.WriteLine($"Populacja: {popSize}, Trasa: {string.Join(" -> ", bestSolution)}, Dystans: {(1 / tspProblem.EvaluateFitness(bestSolution))}");
// }
