using UnityEngine;

public class GridManager :MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2 gridWorldSize = new Vector2(30, 20);
    public float nodeRadius = 0.5f;

    private Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;

        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = true;
                grid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = Color.white;

                Gizmos.DrawWireCube(
                    node.worldPosition,
                    Vector3.one * (nodeDiameter - 0.1f)
                );
            }
        }
    }
}
