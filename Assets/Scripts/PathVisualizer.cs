using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private GameObject pathDotPrefab;

    private List<GameObject> spawnedDots = new List<GameObject>();

    public void ShowPath(Vector2 startPos, Vector2 targetPos)
    {
        Debug.Log("ShowPath called");

        ClearPath();

        List<Node> path =
            pathfinding.FindPath(startPos, targetPos);
        Debug.Log("Start: " + startPos);
        Debug.Log("Target: " + targetPos);

        if (path == null)
        {
            Debug.Log("Path is null");
            return;
        }

        Debug.Log("Path Size: " + path.Count);

        foreach (Node node in path)
        {
            GameObject dot =
                Instantiate(
                    pathDotPrefab,
                    node.worldPosition,
                    Quaternion.identity
                );

            spawnedDots.Add(dot);
        }
    }

    public void ClearPath()
    {
        foreach (GameObject dot in spawnedDots)
        {
            Destroy(dot);
        }

        spawnedDots.Clear();
    }
}
