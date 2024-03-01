using System.Collections.Generic;
using Pieces;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Node _prefab;

    [SerializeField] private Pawn _pawnWhite;
    [SerializeField] private Pawn _pawnBlack;
    [SerializeField] private Rook _rook;
    [SerializeField] private Knight _knight;
    [SerializeField] private Bishop _bishop;
    [SerializeField] private Queen _queen;
    [SerializeField] private King _king;

    [SerializeField] private Color _brightColor = Color.yellow;

    private Node _previousSelectedNode;
    private Node _currentSelectedNode;
    private Piece _selectedPiece;
    private Vector2Int _selectedPiecePosition;
    private bool _hasSelectedPiece;

    private readonly List<Vector2Int> _possibleMoves = new List<Vector2Int>();

    private static readonly Node[,] Nodes = new Node[8, 8];

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
                    case 1:
                        var piece = SpawnPiece(_pawnWhite, Color.white, x, y);
                        node.PutPiece(piece);
                        break;
                    case 6:
                        piece = SpawnPiece(_pawnBlack, Color.black, x, y);
                        node.PutPiece(piece);
                        break;

                    case 0:
                        switch (x)
                        {
                            case 0 or 7:
                                piece = SpawnPiece(_rook, Color.white, x, y);
                                node.PutPiece(piece);
                                break;

                            case 1 or 6:
                                piece = SpawnPiece(_knight, Color.white, x, y);
                                node.PutPiece(piece);
                                break;

                            case 2 or 5:
                                piece = SpawnPiece(_bishop, Color.white, x, y);
                                node.PutPiece(piece);
                                break;

                            case 3:
                                piece = SpawnPiece(_queen, Color.white, x, y);
                                node.PutPiece(piece);
                                break;

                            case 4:
                                piece = SpawnPiece(_king, Color.white, x, y);
                                node.PutPiece(piece);
                                break;
                        }
                        break;

                    case 7:
                        switch (x)
                        {
                            case 0 or 7:
                                piece = SpawnPiece(_rook, Color.black, x, y);
                                node.PutPiece(piece);
                                break;

                            case 1 or 6:
                                piece = SpawnPiece(_knight, Color.black, x, y);
                                node.PutPiece(piece);
                                break;

                            case 2 or 5:
                                piece = SpawnPiece(_bishop, Color.black, x, y);
                                node.PutPiece(piece);
                                break;

                            case 3:
                                piece = SpawnPiece(_queen, Color.black, x, y);
                                node.PutPiece(piece);
                                break;

                            case 4:
                                piece = SpawnPiece(_king, Color.black, x, y);
                                node.PutPiece(piece);
                                break;
                        }
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
            HandlePiece();
        }
    }

    public void HandlePiece()
    {
        var nodePos = InputManager.NodeSelected;

        _currentSelectedNode = Nodes[nodePos.x, nodePos.y];
        
        if (_currentSelectedNode == _previousSelectedNode)
        {
            return;
        }
        
        if (_hasSelectedPiece)
        {
            //TODO implement if is my piece
            MovePiece(nodePos);
            return;
        }

        if (_currentSelectedNode.HasPiece())
        {
            GetPiece(nodePos);
        }
    }

    private void MovePiece(Vector2Int desiredMove)
    {
        foreach (var possibleMove in _possibleMoves)
        {
            if (desiredMove == possibleMove)
            {
                Nodes[_selectedPiecePosition.x, _selectedPiecePosition.y].RemovePiece();
                Nodes[desiredMove.x, desiredMove.y].PutPiece(_selectedPiece);
                _selectedPiece.transform.position = new Vector3(desiredMove.x, desiredMove.y, 0);
                
                ResetStatus();
                break;
            }
        }
    }
    
    private void GetPiece(Vector2Int selectedNode)
    {
        _selectedPiece = Nodes[selectedNode.x, selectedNode.y].GetPiece();
        _selectedPiecePosition = new Vector2Int(selectedNode.x, selectedNode.y);
        _hasSelectedPiece = true;
        
        GetAndShowMovementPossibilities(selectedNode);
    }
    
    private void GetAndShowMovementPossibilities(Vector2Int selectedNodePosition)
    {
        foreach (var possibleMove in _selectedPiece.MovementPossibilities(selectedNodePosition))
        {
            _possibleMoves.Add(possibleMove);
            Nodes[possibleMove.x, possibleMove.y].ChangeColor(_brightColor);
        }
    }

    private void ResetStatus()
    {
        foreach (var possibleMove in _possibleMoves)
        {
            Nodes[possibleMove.x, possibleMove.y].ResetColor();
        }
        
        _possibleMoves.Clear();
        _previousSelectedNode = _currentSelectedNode;
        _hasSelectedPiece = false;
    }

    private Piece SpawnPiece(Piece prefab, Color color, int x, int y)
    {
        var piece = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.Euler(-90f, 0, 0), _parent);
        piece.GetComponent<MeshRenderer>().material.color = color;
        return piece;
    }

    public static Node GetNode(int x, int y)
    {
        return Nodes[x, y];
    }

    public static Node GetNode(Vector2Int index)
    {
        return Nodes[index.x, index.y];
    }
}