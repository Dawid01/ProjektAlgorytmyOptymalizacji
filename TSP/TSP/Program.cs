using TSP;

String basePathTSP = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "TSP_DATA\\data\\";
string[] graphsTSP = Directory.GetFiles(basePathTSP, "*", SearchOption.TopDirectoryOnly);
List<int> skipedGraphs = new List<int>();

String basePathVRP= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "VRP_DATA\\data\\";
string[] graphsVRP = Directory.GetFiles(basePathVRP, "*", SearchOption.TopDirectoryOnly);

const int populationSize = 50; //50-500
const int maxGenerations = 1000; //500 - 5000
const double mutationRate = 0.02; // 0.01- 0.1
const double crossoverRate = 0.9; // 0.7 - 1
const int eliteCount = 2; // 1 - 5


//RunFromFileTSP(0, CrossoverType.OX);
//RunFromFilesTSP(CrossoverType.OX);
//TestRunTSP(CrossoverType.CX);
//TestRunTSP(CrossoverType.OX);
//TestRunTSP(CrossoverType.PMX);
//TestRunTSP(CrossoverType.UX);
//TestRunVRP();
RunFromFileVRP(0);


void RunFromFileTSP(int fileIndex, CrossoverType crossoverType)
{
    try
    {
        Graph graph = GraphReader.ReadGraphTSP(graphsTSP[fileIndex]);
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


void RunFromFilesTSP(CrossoverType crossoverType)
{
    for (int i = 0; i < graphsTSP.Length; i++)
    {
        try
        {
            Graph graph = GraphReader.ReadGraphTSP(graphsTSP[i]);
            var tspProblem = new TSPProblem(graph);
            var ga = new GeneticAlgorithm<List<int>>(tspProblem, populationSize, maxGenerations, mutationRate,
                crossoverRate, eliteCount, crossoverType);
            List<int> bestSolution = ga.Run();
            Console.WriteLine(
                $"{graph.Name}, Dystans: {(int)(1 / (tspProblem.EvaluateFitness(bestSolution))):F2}, Crossover Type: {crossoverType.ToString()}");
        }
        catch (Exception ex)
        {
            skipedGraphs.Add(i);
        }
    }

    for (int i = 0; i < skipedGraphs.Count; i++)
    {
        Console.WriteLine($"{i + 1}. Pominięty plik: {graphsTSP[skipedGraphs[i]]}");
    }
}

void TestRunTSP(CrossoverType crossoverType)
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
        $"{testGraph.Name}, Dystans: {(1 / (tspProblem.EvaluateFitness(bestSolution))):F2}, Crossover Type: {crossoverType.ToString()}");
    Console.WriteLine($"Trasa: {string.Join(" -> ", bestSolution)}");
}

void TestRunVRP()
{
    Graph testGraph = new Graph(
        "VRP TEST", "TEST", 10,
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

    var demands = new Dictionary<int, int>
    {
        { 1, 10 }, { 2, 15 }, { 3, 5 }, { 4, 20 }, { 5, 8 },
        { 6, 12 }, { 7, 7 }, { 8, 9 }, { 9, 14 }
    };

    int populationSize = 100;
    int maxGenerations = 500;
    double mutationRate = 0.2;
    double crossoverRate = 0.8;
    int eliteCount = 5;
    int vehicleCount = 2;
    int vehicleCapacity = 50;

    var vrpProblem = new VRPProblem(testGraph, vehicleCount, vehicleCapacity, demands);
    var ga = new GeneticAlgorithm<List<Vehicle>>(
        vrpProblem, populationSize, maxGenerations,
        mutationRate, crossoverRate, eliteCount, CrossoverType.CX
    );

    List<Vehicle> bestSolution = ga.Run();

    double bestFitness = vrpProblem.EvaluateFitness(bestSolution);
    double bestDistance = 1.0 / bestFitness - 1; 

    Console.WriteLine($"{testGraph.Name}, Dystans: {bestDistance:F2}");

   for (int i = 0; i < bestSolution.Count; i++)
   {
       Vehicle vehicle = bestSolution[i];
       string route = string.Join(" -> ", vehicle.Route);
       Console.WriteLine($"Pojazd {vehicle.Id + 1}: {route}");
   }
}

void RunFromFileVRP(int fileIndex)
{
    try
    {
        GraphReader.VRPData vrpData = GraphReader.ReadGraphVRP(graphsVRP[fileIndex]);
        var vrpProblem = new VRPProblem(vrpData.Graph, vrpData.VehicleCount, vrpData.VehicleCapacity, vrpData.Demands);
        var ga = new GeneticAlgorithm<List<Vehicle>>(
            vrpProblem, populationSize, maxGenerations,
            mutationRate, crossoverRate, eliteCount, CrossoverType.CX
        );

        List<Vehicle> bestSolution = ga.Run();

        double bestFitness = vrpProblem.EvaluateFitness(bestSolution);
        double bestDistance = 1.0 / bestFitness - 1; 

        Console.WriteLine($"{vrpData.Graph.Name}, Dystans: {bestDistance:F2}");
    }
    catch (Exception ex)
    {
        skipedGraphs.Add(fileIndex);
    }
}