using Pieces;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Node _prefab;

    private static readonly Node[,] Nodes = new Node[8, 8];

    private void Awake()
    {
        var nodeColor = false;

        for (int y = 0; y < 8; y++)
        {
            nodeColor = !nodeColor;
            for (int x = 0; x < 8; x++)
            {
                var node = Instantiate(_prefab, new Vector3(x, y, 0), Quaternion.identity, _parent);

                node.name = $"X = {x}, Y = {y}";

                node.SetBaseColor(nodeColor ? Color.white : Color.black);
                nodeColor = !nodeColor;
                
                Nodes[x, y] = node;
            }
        }
    }

    public static Node GetNode(Vector2Int boardPosition)
    {
        return Nodes[boardPosition.x, boardPosition.y];
    }
    
    public static Node GetNode(int x, int y)
    {
        return Nodes[x, y];
    }
}