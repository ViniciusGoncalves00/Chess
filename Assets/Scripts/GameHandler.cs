using System;
using System.Collections.Generic;
using Pieces;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Color _brightColor = Color.yellow;
    
    private Node _previousSelectedNode;
    private Node _currentSelectedNode;
    private Piece _selectedPiece;
    private Vector2Int _selectedPiecePosition;
    private bool _hasSelectedPiece;
    
    private PieceColor _colorTurn;
    
    private List<Vector2Int> _possibleMoves = new List<Vector2Int>();
    private List<Piece> _whitePiecesRemoved = new List<Piece>();
    private List<Piece> _blackPiecesRemoved = new List<Piece>();

    private void Start()
    {
        _colorTurn = PieceColor.White;
    }

    private void Update()
    {
        if (InputManager.IsNodeSelected)
        {
            Handle();
        }
    }
    
    private void Handle()
    {
        var nodePos = InputManager.NodeSelected;

        _currentSelectedNode = Board.GetNode(nodePos);

        if (_currentSelectedNode == _previousSelectedNode)
        {
            return;
        }

        if (_hasSelectedPiece)
        {
            if (_currentSelectedNode.HasPiece() && _currentSelectedNode.GetPiece().GetPieceColor() == _colorTurn)
            {
                ResetStatus();
                GetPiece(nodePos);
            }
            
            else if (_possibleMoves.Count > 0)
            {
                TryMovePiece(nodePos, out var doMove);
                ResetStatus();
                
                if (doMove)
                {
                    SwitchTurn();
                }
            }
        }

        else if (_currentSelectedNode.HasPiece() && _currentSelectedNode.GetPiece().GetPieceColor() == _colorTurn)
        {
            GetPiece(nodePos);
        }

        _previousSelectedNode = _currentSelectedNode;
    }

    private void TryMovePiece(Vector2Int desiredMove, out bool doMove)
    {
        doMove = false;

        foreach (var possibleMove in _possibleMoves)
        {
            if (desiredMove != possibleMove)
            {
                continue;
            }

            switch (_selectedPiece)
            {
                case Pawn:
                    _selectedPiece.GetComponent<Pawn>().FirstMovement();
                    break;
                case King:
                    //TODO finish game
                    break;
            }
                
            Board.GetNode(_selectedPiecePosition).RemovePiece();
            var node = Board.GetNode(desiredMove);

            if (node.HasPiece())
            {
                var piece = node.GetPiece();
                    
                if (piece.GetPieceColor() == PieceColor.White)
                {
                    _whitePiecesRemoved.Add(piece);
                }
                else if (piece.GetPieceColor() == PieceColor.Black)
                {
                    _blackPiecesRemoved.Add(piece);
                }

                piece.transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, 10);
                        
                node.RemovePiece();
            }
                
            node.StorePiece(_selectedPiece);
            _selectedPiece.transform.position = new Vector3(desiredMove.x, desiredMove.y, 0);
            doMove = true;
            return;
        }
    }
    
    private void GetPiece(Vector2Int selectedNode)
    {
        _selectedPiece = Board.GetNode(selectedNode).GetPiece();
        _selectedPiecePosition = new Vector2Int(selectedNode.x, selectedNode.y);
        _hasSelectedPiece = true;
        
        GetAndShowMovementPossibilities(selectedNode);
    }
    
    private void GetAndShowMovementPossibilities(Vector2Int selectedNodePosition)
    {
        _possibleMoves = _selectedPiece.PossibleMovements(selectedNodePosition);
        
        foreach (var possibleMove in _possibleMoves)
        {
            Board.GetNode(possibleMove).ChangeColor(_brightColor);
        }
    }

    private void ResetStatus()
    {
        foreach (var possibleMove in _possibleMoves)
        {
            Board.GetNode(possibleMove).ResetColor();
        }
        
        _possibleMoves.Clear();
        _hasSelectedPiece = false;
    }

    private void SwitchTurn()
    {
        _colorTurn = _colorTurn switch
        {
            PieceColor.White => PieceColor.Black,
            PieceColor.Black => PieceColor.White,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
