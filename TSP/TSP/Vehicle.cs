namespace TSP;

public struct Vehicle
{
    public int Id { get; set; }
    public int Capacity { get; set; }
    public List<int> Route { get; set; } = new List<int> { 0 };

    public Vehicle(int id, int capacity)
    {
        Id = id;
        Capacity = capacity;
    }

    public Vehicle Clone()
    {
        return new Vehicle(Id, Capacity)
        {
            Route = new List<int>(Route)
        };
    }
}