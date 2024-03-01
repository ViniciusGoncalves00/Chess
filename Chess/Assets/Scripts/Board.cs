using System.Collections.Generic;
using Pieces;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Node _prefab;

    [SerializeField] private Pawn _pawn;
    [SerializeField] private Rook _rook;
    [SerializeField] private Knight _knight;
    [SerializeField] private Bishop _bishop;
    [SerializeField] private Queen _queen;
    [SerializeField] private King _king;

    [SerializeField] private Color _brightColor = Color.yellow;

    private Node _previousSelectedNode;
    private Node _currentSelectedNode;

    private readonly List<Vector2Int> _possibleMoves = new List<Vector2Int>();

    public static readonly Node[,] Nodes = new Node[8, 8];

    private void Start()
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

                switch (y)
                {
                    case > 1 and < 6:
                        break;
                    case 1 or 6:
                        SpawnPiece(_pawn, x, y);
                        node.SetPiece(_pawn);
                        break;
                    case 0 or 7 when x is 0 or 7:
                        SpawnPiece(_rook, x, y);
                        node.SetPiece(_rook);
                        break;
                    case 0 or 7 when x is 1 or 6:
                        SpawnPiece(_knight, x, y);
                        node.SetPiece(_knight);
                        break;
                    case 0 or 7 when x is 2 or 5:
                        SpawnPiece(_bishop, x, y);
                        node.SetPiece(_bishop);
                        break;
                    case 0 or 7 when x is 3:
                        SpawnPiece(_queen, x, y);
                        node.SetPiece(_queen);
                        break;
                    case 0 or 7 when x is 4:
                        SpawnPiece(_king, x, y);
                        node.SetPiece(_king);
                        break;
                }

                Nodes[x, y] = node;
            }
        }
    }

    private void Update()
    {
        if (InputManager.IsNodeSelected)
        {
            var selectedNode = InputManager.NodeSelected;
            var x = selectedNode.x;
            var y = selectedNode.y;

            _currentSelectedNode = Nodes[x, y];
            
            if (_currentSelectedNode != _previousSelectedNode)
            {
                foreach (var possibleMove in _possibleMoves)
                {
                    Nodes[possibleMove.x, possibleMove.y].ResetColor();
                }

                _possibleMoves.Clear();
                _previousSelectedNode = _currentSelectedNode;
            }
            
            if (Nodes[x, y].Piece == null)
            {
                return;
            }

            foreach (var possibleMove in Nodes[x, y].Piece.MovementPossibilities(x, y))
            {
                _possibleMoves.Add(possibleMove);
                Nodes[possibleMove.x, possibleMove.y].ChangeColor(_brightColor);
            }
        }
    }

    public void ShowMovementPossibilities(int x, int y)
    {
        Nodes[x, y].SetBaseColor(_brightColor);
    }
    
    private void SpawnPiece(Piece prefab, int x, int y)
    {
        Instantiate(prefab, new Vector3(x, y, 0), Quaternion.Euler(-90f,0,0), _parent);
    }
}