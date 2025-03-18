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
}