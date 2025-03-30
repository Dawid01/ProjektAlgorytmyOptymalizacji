using TSP;

String basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "Graphs\\";
Graph graph = GraphReader.ReadGraph(basePath + "a280.tsp");
//Console.WriteLine(graph.ToString());


Graph graphTest = new Graph(
    "TSP Example", "Euclidean", 4, 
    new Node[]
    {
        new Node(0, 0, 0),
        new Node(1, 2, 3),
        new Node(2, 5, 4),
        new Node(3, 6, 1)
    });

var tspProblem = new TSPProblem(graph);
var ga = new GeneticAlgorithm<List<int>>(tspProblem, 100, 1000, 0.1);

List<int> bestSolution = ga.Run();
        
Console.WriteLine($"Trasa: {string.Join(" -> ", bestSolution)}, Dystans: {(1 / tspProblem.EvaluateFitness(bestSolution))}");


// for (int popSize = 100; popSize <= 1000; popSize += 100)
// {
//     var ga = new GeneticAlgorithm<List<int>>(tspProblem, popSize, 1000, 0.05);
//     List<int> bestSolution = ga.Run();
//     Console.WriteLine($"Populacja: {popSize}, Trasa: {string.Join(" -> ", bestSolution)}, Dystans: {(1 / tspProblem.EvaluateFitness(bestSolution))}");
// }
