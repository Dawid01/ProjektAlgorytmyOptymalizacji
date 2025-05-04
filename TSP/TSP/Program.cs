using TSP;

String basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "TSP_DATA\\data\\";
string[] graphs = Directory.GetFiles(basePath, "*", SearchOption.TopDirectoryOnly);
List<int> skipedGraphs = new List<int>();

const int populationSize = 300; //50-500
const int maxGenerations = 3000; //500 - 5000
const double mutationRate = 0.02; // 0.01- 0.1
const double crossoverRate = 0.9; // 0.7 - 1
const int eliteCount = 2; // 1 - 5

// for (int i = 0; i < 2; i++)
// {
//     try
//     {
//         Graph graph = GraphReader.ReadGraph(graphs[i]);
//         var tspProblem = new TSPProblem(graph);
//         var ga = new GeneticAlgorithm<List<int>>(tspProblem, populationSize, maxGenerations, mutationRate);
//         List<int> bestSolution = ga.Run();
//         Console.WriteLine($"{graph.Name}, Dystans: {(int)(1 / (tspProblem.EvaluateFitness(bestSolution)))}");
//     }
//     catch (Exception ex)
//     {
//        skipedGraphs.Add(i);
//     }
// }
//
// for(int i = 0; i < skipedGraphs.Count; i++)
// {
//     Console.WriteLine($"{i + 1}. Pominięty plik: {graphs[skipedGraphs[i]]}");
// }


Graph graph = GraphReader.ReadGraph(graphs[0]);
var tspProblem = new TSPProblem(graph);
var ga = new GeneticAlgorithm<List<int>>(tspProblem, populationSize, maxGenerations, mutationRate, crossoverRate, eliteCount);
List<int> bestSolution = ga.Run();
Console.WriteLine($"{graph.Name}, Dystans: {(int)(1 / (tspProblem.EvaluateFitness(bestSolution)))}");

// for (int value = 100; value <= 1000; value += 100)
// {
//     var ga = new GeneticAlgorithm<List<int>>(tspProblem, 80, 1000, 0.002);
//     List<int> bestSolution = ga.Run();
//     Console.WriteLine($"{graph.Name}, Dystans: {(int)(1 / (tspProblem.EvaluateFitness(bestSolution)))}");
// }
