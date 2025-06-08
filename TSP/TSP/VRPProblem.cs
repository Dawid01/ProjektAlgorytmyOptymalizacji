namespace TSP;

public class VRPProblem : IGeneticProblem<List<Vehicle>>
{
    private readonly Graph _graph;
    private readonly int _vehicleCount;
    private readonly int _vehicleCapacity;
    private readonly Dictionary<int, int> _demands;
    private readonly Random _random;

    public VRPProblem(Graph graph, int vehicleCount, int vehicleCapacity, Dictionary<int, int> demands, int seed = -1)
    {
        _graph = graph;
        _vehicleCount = vehicleCount;
        _vehicleCapacity = vehicleCapacity; 
        _demands = demands;
        _random = seed != -1 ? new Random(seed) : new Random();
    }

    public List<List<Vehicle>> GenerateInitialPopulation(int size)
    {
        var population = new List<List<Vehicle>>();

        for (int i = 0; i < size; i++)
        {
            var customers = Enumerable.Range(1, _graph.Dimension - 1)
                                      .OrderBy(_ => _random.Next())
                                      .ToList();

            var vehicles = Enumerable.Range(0, _vehicleCount)
                                     .Select(id => new Vehicle(id, _vehicleCapacity))
                                     .ToList();

            int v = 0;
            for (int j = 0; j < customers.Count; j++)
            {
                int customer = customers[j];
                vehicles[v].Route.Add(customer);
                v = (v + 1) % _vehicleCount;
            }
            
            for (int j = 0; j < vehicles.Count; j++)
            {
                vehicles[j].Route.Add(0);
            }
            
            population.Add(vehicles);
        }

        return population;
    }

    public double EvaluateFitness(List<Vehicle> individual)
    {
        double totalDistance = 0;
        double penalty = 0;

        foreach (var vehicle in individual)
        {
            int load = 0;

            for (int i = 0; i < vehicle.Route.Count - 1; i++)
            {
                int from = vehicle.Route[i];
                int to = vehicle.Route[i + 1];
                totalDistance += GetDistance(_graph.Nodes[from], _graph.Nodes[to]);

                if (_demands.ContainsKey(to))
                    load += _demands[to];
            }

            if (load > vehicle.Capacity)
                penalty += (load - vehicle.Capacity) * 1000.0;
        }

        return 1.0 / (totalDistance + penalty + 1);
    }

    //PMX
    public List<Vehicle> Crossover(List<Vehicle> parent1, List<Vehicle> parent2, CrossoverType type)
    {
        var child = new List<Vehicle>();
        var used = new HashSet<int> { 0 };

        int half = _vehicleCount / 2;
        for (int i = 0; i < half; i++)
        {
            var clone = parent1[i].Clone();
            child.Add(clone);
            foreach (var node in clone.Route)
                used.Add(node);
        }

        foreach (var vehicle in parent2)
        {
            var route = new List<int> { 0 };
            foreach (var customer in vehicle.Route)
            {
                if (!used.Contains(customer))
                {
                    route.Add(customer);
                    used.Add(customer);
                }
            }

            if (route.Count > 1)
                route.Add(0);

            if (route.Count > 2)
                child.Add(new Vehicle(vehicle.Id, vehicle.Capacity) { Route = route });
        }

        while (child.Count < _vehicleCount)
            child.Add(new Vehicle(child.Count, _vehicleCapacity) { Route = new List<int> { 0, 0 } });

        return child;
    }

    public List<Vehicle> Mutate(List<Vehicle> individual)
    {
        int v1 = _random.Next(_vehicleCount);
        int v2 = _random.Next(_vehicleCount);

        if (individual[v1].Route.Count <= 2 || individual[v2].Route.Count <= 2)
            return individual;

        int i1 = _random.Next(1, individual[v1].Route.Count - 1);
        int i2 = _random.Next(1, individual[v2].Route.Count - 1);

        (individual[v1].Route[i1], individual[v2].Route[i2]) = 
            (individual[v2].Route[i2], individual[v1].Route[i1]);

        return individual;
    }

    public bool ShouldStop(List<List<Vehicle>> population, int generation)
    {
        return generation >= 1000;
    }

    private double GetDistance(Node a, Node b)
    {
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }
}
