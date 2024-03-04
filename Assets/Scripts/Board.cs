using System;
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
    
    private Node _currentSelectedNode;
    private Piece _selectedPiece;
    private Vector2Int _selectedPiecePosition;
    private bool _hasSelectedPiece;

    private PieceColor _colorTurn;

    private readonly List<Vector2Int> _possibleMoves = new List<Vector2Int>();

    private static readonly Node[,] Nodes = new Node[8, 8];

    private void Start()
    {
        var nodeColor = false;

        _colorTurn = PieceColor.White;

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
                        var piece = SpawnPiece(_pawnWhite, PieceColor.White, x, y);
                        node.PutPiece(piece);
                        break;
                    case 6:
                        piece = SpawnPiece(_pawnBlack,PieceColor.Black, x, y);
                        node.PutPiece(piece);
                        break;

                    case 0:
                        switch (x)
                        {
                            case 0 or 7:
                                piece = SpawnPiece(_rook, PieceColor.White, x, y);
                                node.PutPiece(piece);
                                break;

                            case 1 or 6:
                                piece = SpawnPiece(_knight, PieceColor.White, x, y);
                                node.PutPiece(piece);
                                break;

                            case 2 or 5:
                                piece = SpawnPiece(_bishop, PieceColor.White, x, y);
                                node.PutPiece(piece);
                                break;

                            case 3:
                                piece = SpawnPiece(_queen, PieceColor.White, x, y);
                                node.PutPiece(piece);
                                break;

                            case 4:
                                piece = SpawnPiece(_king, PieceColor.White, x, y);
                                node.PutPiece(piece);
                                break;
                        }
                        break;

                    case 7:
                        switch (x)
                        {
                            case 0 or 7:
                                piece = SpawnPiece(_rook, PieceColor.Black, x, y);
                                node.PutPiece(piece);
                                break;

                            case 1 or 6:
                                piece = SpawnPiece(_knight, PieceColor.Black, x, y);
                                node.PutPiece(piece);
                                break;

                            case 2 or 5:
                                piece = SpawnPiece(_bishop, PieceColor.Black, x, y);
                                node.PutPiece(piece);
                                break;

                            case 3:
                                piece = SpawnPiece(_queen, PieceColor.Black, x, y);
                                node.PutPiece(piece);
                                break;

                            case 4:
                                piece = SpawnPiece(_king, PieceColor.Black, x, y);
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

    private void HandlePiece()
    {
        var nodePos = InputManager.NodeSelected;

        _currentSelectedNode = Nodes[nodePos.x, nodePos.y];

        if (_hasSelectedPiece)
        {
            if (_currentSelectedNode.HasPiece() && _currentSelectedNode.GetPiece().GetPieceColor() == _colorTurn)
            {
                GetPiece(nodePos);
            }
            
            else if (_possibleMoves.Count > 0)
            {
                MovePiece(nodePos);
            }
        }
        
        else if (_currentSelectedNode.HasPiece() && _currentSelectedNode.GetPiece().GetPieceColor() == _colorTurn)
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

        _colorTurn = _colorTurn switch
        {
            PieceColor.White => PieceColor.Black,
            PieceColor.Black => PieceColor.White,
        };

        _possibleMoves.Clear();
        _hasSelectedPiece = false;
    }

    private Piece SpawnPiece(Piece prefab, PieceColor color, int x, int y)
    {
        var piece = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.Euler(-90f, 0, 0), _parent);
        piece.SetPieceColor(color);

        piece.GetComponent<MeshRenderer>().material.color = color switch
        {
            PieceColor.White => Color.white,
            PieceColor.Black => Color.black,
        };

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