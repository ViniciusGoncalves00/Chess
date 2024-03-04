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
    
    private readonly List<Vector2Int> _possibleMoves = new List<Vector2Int>();

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
                MovePiece(nodePos);
                ResetStatus();
                SwitchTurn();
            }
        }

        else if (_currentSelectedNode.HasPiece() && _currentSelectedNode.GetPiece().GetPieceColor() == _colorTurn)
        {
            GetPiece(nodePos);
        }

        _previousSelectedNode = _currentSelectedNode;
    }

    private void MovePiece(Vector2Int desiredMove)
    {
        if (_selectedPiece is IPawn)
        {
            _selectedPiece.GetComponent<IPawn>().FirstMovement();
        }

        foreach (var possibleMove in _possibleMoves)
        {
            if (desiredMove == possibleMove)
            {
                Board.GetNode(_selectedPiecePosition).RemovePiece();
                Board.GetNode(desiredMove).StorePiece(_selectedPiece);
                _selectedPiece.transform.position = new Vector3(desiredMove.x, desiredMove.y, 0);
                break;
            }
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
        foreach (var possibleMove in _selectedPiece.MovementPossibilities(selectedNodePosition))
        {
            _possibleMoves.Add(possibleMove);
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
        };
    }
}
