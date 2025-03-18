using TSP;

String basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\") + "Graphs\\";
Graph graph = GraphReader.ReadGraph(basePath + "a280.tsp");
Console.WriteLine(graph.ToString());