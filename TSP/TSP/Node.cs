namespace TSP;

public struct Node
{
    public int Index { get; }
    public double X { get; }
    public double Y { get; }

    public Node(int index, double x, double y)
    {
        Index = index;
        X = x;
        Y = y;
    }
    
    public String ToString()
    {
        return $"{Index} [x: {X}, y: {Y}]";
    }
}