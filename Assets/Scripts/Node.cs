using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;

    public int gCost;
    public int hCost;

    public Node parent;

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public Node(bool walkable, Vector2 worldPosition)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
    }
}
