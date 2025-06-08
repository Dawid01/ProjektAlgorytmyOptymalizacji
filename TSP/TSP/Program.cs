using TSP;

String basePathTSP = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "TSP_DATA\\data\\";
string[] graphsTSP = Directory.GetFiles(basePathTSP, "*", SearchOption.TopDirectoryOnly);
List<int> skipedGraphs = new List<int>();

String basePathVRP= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "VRP_DATA\\data\\";
string[] graphsVRP = Directory.GetFiles(basePathVRP, "*", SearchOption.TopDirectoryOnly);

int populationSize = 400; //50-500
int maxGenerations = 1250; //500 - 5000
double mutationRate = 0.01; // 0.01- 0.1
double crossoverRate = 0.7; // 0.7 - 1
int eliteCount = 1; // 1 - 5
int vehicleCount = 9;
int vehicleCapacity = 200;

for (int i = 0; i < 5; i++)
{
    TestRunVRP(); 
}

//RunFromFilesTSP(CrossoverType.OX);
//TestRunTSP(CrossoverType.CX);     
//TestRunTSP(CrossoverType.OX);
//TestRunTSP(CrossoverType.PMX);
//TestRunTSP(CrossoverType.UX);

//RunFromFileVRP(0);
/*
Console.WriteLine("POPULATION SIZE");
while (populationSize <= 500)
{
    Console.WriteLine(populationSize);
    TestRunVRP();
    populationSize += 25;
}

Console.WriteLine("-----------------------------");
*/
/*
Console.WriteLine("MAX GEN");
while (maxGenerations <= 5000)
{
    Console.WriteLine(maxGenerations);
    TestRunVRP();
    maxGenerations += 250;
}
Console.WriteLine("-----------------------------");
*/
/*
Console.WriteLine("Mutations rate");
while (mutationRate <= 0.1)
{
    Console.WriteLine(mutationRate);
    TestRunVRP();
    mutationRate += 0.01;
}
Console.WriteLine("-----------------------------");
*/
/*
Console.WriteLine("crossover rate");
while (crossoverRate <= 1)
{
    Console.WriteLine(crossoverRate);
    TestRunVRP();
    crossoverRate += 0.1;
}
Console.WriteLine("-----------------------------");
*/
/*
Console.WriteLine("elite");
while (eliteCount <= 5)
{
    Console.WriteLine(eliteCount);
    TestRunVRP();
    eliteCount += 1;
}
Console.WriteLine("-----------------------------");
*/
/*
Console.WriteLine("vehicle count");
while (vehicleCount <= 10)
{
    Console.WriteLine(vehicleCount);
    TestRunVRP();
    vehicleCount += 1;
}
Console.WriteLine("-----------------------------");
*/
/*
Console.WriteLine("vehicle capacity");
while (vehicleCapacity <= 250)
{
    Console.WriteLine(vehicleCapacity);
    TestRunVRP();
    vehicleCapacity += 25;
}

Console.WriteLine("-----------------------------");
*/
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
    "VRP_BIG", "BIG_TEST", 51,
    new Node[]
    {
        new Node(0, 50, 50), // Depot
        new Node(1, 10, 23), new Node(2, 35, 12), new Node(3, 64, 20), new Node(4, 25, 75), new Node(5, 82, 15),
        new Node(6, 11, 86), new Node(7, 72, 41), new Node(8, 61, 66), new Node(9, 15, 10), new Node(10, 37, 84),
        new Node(11, 88, 76), new Node(12, 48, 13), new Node(13, 13, 56), new Node(14, 59, 48), new Node(15, 94, 29),
        new Node(16, 20, 66), new Node(17, 74, 55), new Node(18, 33, 26), new Node(19, 60, 10), new Node(20, 42, 93),
        new Node(21, 21, 37), new Node(22, 26, 90), new Node(23, 87, 45), new Node(24, 18, 28), new Node(25, 99, 11),
        new Node(26, 36, 67), new Node(27, 55, 32), new Node(28, 16, 88), new Node(29, 70, 25), new Node(30, 49, 60),
        new Node(31, 66, 80), new Node(32, 12, 14), new Node(33, 81, 54), new Node(34, 38, 22), new Node(35, 63, 38),
        new Node(36, 27, 63), new Node(37, 30, 45), new Node(38, 44, 11), new Node(39, 58, 94), new Node(40, 47, 47),
        new Node(41, 52, 73), new Node(42, 90, 66), new Node(43, 29, 12), new Node(44, 68, 57), new Node(45, 77, 33),
        new Node(46, 15, 39), new Node(47, 40, 35), new Node(48, 84, 21), new Node(49, 22, 52), new Node(50, 32, 79)
    });

    var demands = new Dictionary<int, int>
    {
        {1, 12}, {2, 7}, {3, 18}, {4, 6}, {5, 21}, {6, 14}, {7, 19}, {8, 8}, {9, 13}, {10, 5},
        {11, 20}, {12, 17}, {13, 11}, {14, 9}, {15, 16}, {16, 10}, {17, 8}, {18, 6}, {19, 13}, {20, 15},
        {21, 12}, {22, 5}, {23, 9}, {24, 7}, {25, 18}, {26, 10}, {27, 14}, {28, 13}, {29, 11}, {30, 8},
        {31, 6}, {32, 10}, {33, 17}, {34, 19}, {35, 15}, {36, 12}, {37, 5}, {38, 7}, {39, 6}, {40, 9},
        {41, 13}, {42, 14}, {43, 11}, {44, 6}, {45, 20}, {46, 7}, {47, 10}, {48, 16}, {49, 18}, {50, 12}
    };

    var vrpProblem = new VRPProblem(testGraph, vehicleCount, vehicleCapacity, demands);
    var ga = new GeneticAlgorithm<List<Vehicle>>(
        vrpProblem, populationSize, maxGenerations,
        mutationRate, crossoverRate, eliteCount, CrossoverType.CX
    );

    List<Vehicle> bestSolution = ga.Run();

    double bestFitness = vrpProblem.EvaluateFitness(bestSolution);
    double bestDistance = 1.0 / bestFitness - 1;
    Console.WriteLine($"Liczba pojazdów: {vehicleCount}");
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