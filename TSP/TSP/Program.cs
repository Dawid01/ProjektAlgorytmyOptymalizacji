using TSP;

String basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "TSP_DATA\\data\\";
string[] graphs = Directory.GetFiles(basePath, "*", SearchOption.TopDirectoryOnly);
List<int> skipedGraphs = new List<int>();

const int populationSize = 50; //50-500
const int maxGenerations = 1000; //500 - 5000
const double mutationRate = 0.02; // 0.01- 0.1
const double crossoverRate = 0.9; // 0.7 - 1
const int eliteCount = 2; // 1 - 5


//RunFromFile(0, CrossoverType.OX);
//RunFromFiles(CrossoverType.OX);
TestRun(CrossoverType.CX);
TestRun(CrossoverType.OX);
TestRun(CrossoverType.PMX);
TestRun(CrossoverType.UX);


void RunFromFile(int fileIndex, CrossoverType crossoverType)
{
    try
    {
        Graph graph = GraphReader.ReadGraph(graphs[fileIndex]);
        var tspProblem = new TSPProblem(graph);
        var ga = new GeneticAlgorithm<List<int>>(tspProblem, populationSize, maxGenerations, mutationRate,
            crossoverRate, eliteCount, crossoverType);
        List<int> bestSolution = ga.Run();
        Console.WriteLine(
            $"{graph.Name}, Dystans: {(int)(1 / (tspProblem.EvaluateFitness(bestSolution)))}, Crossover Type: {crossoverType.ToString()}");
    }
    catch (Exception ex)
    {
        skipedGraphs.Add(fileIndex);
    }
}


void RunFromFiles(CrossoverType crossoverType)
{
    for (int i = 0; i < graphs.Length; i++)
    {
        try
        {
            Graph graph = GraphReader.ReadGraph(graphs[i]);
            var tspProblem = new TSPProblem(graph);
            var ga = new GeneticAlgorithm<List<int>>(tspProblem, populationSize, maxGenerations, mutationRate,
                crossoverRate, eliteCount, crossoverType);
            List<int> bestSolution = ga.Run();
            Console.WriteLine(
                $"{graph.Name}, Dystans: {(int)(1 / (tspProblem.EvaluateFitness(bestSolution)))}, Crossover Type: {crossoverType.ToString()}");
        }
        catch (Exception ex)
        {
            skipedGraphs.Add(i);
        }
    }

    for (int i = 0; i < skipedGraphs.Count; i++)
    {
        Console.WriteLine($"{i + 1}. Pominięty plik: {graphs[skipedGraphs[i]]}");
    }
}

void TestRun(CrossoverType crossoverType)
{
    Graph testGraph = new Graph(
        "TSP TEST", "TEST", 10,
        new Node[]
        {
            new Node(0, 0, 0),
            new Node(1, 1, 0),
            new Node(2, 10, 10),
            new Node(3, 0, 4),
            new Node(4, 2, 5),
            new Node(5, 3, 9),
            new Node(6, 12, 9),
            new Node(7, 5, 4),
            new Node(8, 3, 2),
            new Node(9, 1, 10)
        });

    var tspProblem = new TSPProblem(testGraph);
    var ga = new GeneticAlgorithm<List<int>>(tspProblem, populationSize, maxGenerations, mutationRate, crossoverRate,
        eliteCount, crossoverType);
    List<int> bestSolution = ga.Run();
    Console.WriteLine(
        $"{testGraph.Name}, Dystans: {(1 / (tspProblem.EvaluateFitness(bestSolution)))}, Crossover Type: {crossoverType.ToString()}");
    Console.WriteLine($"Trasa: {string.Join(" -> ", bestSolution)}");
}