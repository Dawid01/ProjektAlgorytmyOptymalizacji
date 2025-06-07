namespace TSP;

public struct Node
{
    public int Index;
    public int X;
    public int Y;

    public Node(int index, int x, int y)
    {
        Index = index;
        X = x;
        Y = y;
    }
    
    public String ToString()
    {
        return $"{Index} [x: {X}, y: {Y}]";
    }
    
    public double DistanceTo(Node other)
    {
        int dx = this.X - other.X;
        int dy = this.Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}