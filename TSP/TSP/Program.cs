using TSP;

String basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "Graphs\\";
//Graph graph = GraphReader.ReadGraph(basePath + "a280.tsp");
//Console.WriteLine(graph.ToString());


Graph graph = new Graph(
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
        
Console.WriteLine("Najlepsza trasa: " + string.Join(" -> ", bestSolution));