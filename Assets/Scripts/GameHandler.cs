using System;
using System.Collections;
using System.Collections.Generic;
using Pieces;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Color _brightColor = Color.yellow;
    [SerializeField] private GameObject _victoryText;
    [SerializeField] private TMP_Text _winner;

    private Node _currentNode;
    private Node _previousNode;
    private Piece _currentPiece;
    private Piece _capturedPiece;
    private Vector2Int _currentNodePosition;
    private Vector2Int _currentPiecePosition;
    private bool _hasSelectedPiece;

    private PieceColor _colorTurn;

    private List<Vector2Int> _possibleMoves = new List<Vector2Int>();
    private List<Piece> _whitePiecesAlive = new List<Piece>();
    private List<Piece> _whitePiecesRemoved = new List<Piece>();
    private List<Piece> _blackPiecesAlive = new List<Piece>();
    private List<Piece> _blackPiecesRemoved = new List<Piece>();

    private void Start()
    {
        _colorTurn = PieceColor.White;

        _whitePiecesAlive = SpawnPieces.WhitePieces;
        _blackPiecesAlive = SpawnPieces.BlackPieces;
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
        _currentNodePosition = InputManager.NodeSelected;

        _currentNode = Board.GetNode(_currentNodePosition);

        if (_currentNode == _previousNode)
        {
            return;
        }

        if (_hasSelectedPiece)
        {
            if (_currentNode.HasPiece() && _currentNode.GetPiece().GetPieceColor() == _colorTurn)
            {
                ResetStatus();
                GetPiece();
            }

            else if (_possibleMoves.Count > 0)
            {
                if (TryMovePiece())
                {
                    PlayMade();
                }

                ResetStatus();
            }
        }

        else if (_currentNode.HasPiece() && _currentNode.GetPiece().GetPieceColor() == _colorTurn)
        {
            GetPiece();
        }

        _previousNode = _currentNode;
    }

    private bool TryMovePiece()
    {
        var desiredMove = _currentNodePosition;

        if (_possibleMoves.Contains(desiredMove))
        {
            MovePiece();
            return true;
        }

        return false;
    }

    private void MovePiece()
    {
        SpecialCase();

        _previousNode.RemovePiece();

        if (_currentNode.HasPiece())
        {
            CapturePiece();
        }

        _currentNode.StorePiece(_currentPiece);
        _currentPiece.transform.position = new Vector3(_currentNodePosition.x, _currentNodePosition.y, 0);
    }

    private void CapturePiece()
    {
        _capturedPiece = _currentNode.GetPiece();

        switch (_capturedPiece.GetPieceColor())
        {
            case PieceColor.White:
                _whitePiecesAlive.Remove(_capturedPiece);
                _whitePiecesRemoved.Add(_capturedPiece);
                break;
            case PieceColor.Black:
                _blackPiecesAlive.Remove(_capturedPiece);
                _blackPiecesRemoved.Add(_capturedPiece);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var position = _capturedPiece.transform.position;
        position.z = 10f;
        _capturedPiece.transform.position = position;

        _currentNode.RemovePiece();
    }

    private void GetPiece()
    {
        _currentPiece = Board.GetNode(_currentNodePosition).GetPiece();
        _currentPiecePosition = new Vector2Int(_currentNodePosition.x, _currentNodePosition.y);
        _hasSelectedPiece = true;

        GetPossibleMovements();
        ShowPossibleMovements();
    }

    private void GetPossibleMovements()
    {
        _possibleMoves = _currentPiece.PossibleMovements(_currentPiecePosition);
    }

    private void ShowPossibleMovements()
    {
        _possibleMoves.ForEach(possibleMove => Board.GetNode(possibleMove).ChangeColor(_brightColor));
    }

    private void ResetStatus()
    {
        _possibleMoves.ForEach(possibleMove => Board.GetNode(possibleMove).ResetColor());
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

    private void SpecialCase()
    {
        switch (_currentPiece)
        {
            case Pawn:
                _currentPiece.GetComponent<Pawn>().FirstMovement();
                break;
        }
    }

    private void PlayMade()
    {
        switch (_colorTurn)
        {
            case PieceColor.White:
                TryRevivePiece(ref _whitePiecesAlive, ref _whitePiecesRemoved);
                break;
            case PieceColor.Black:
                TryRevivePiece(ref _blackPiecesAlive, ref _blackPiecesRemoved);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        SwitchTurn();

        if (KingCaptured())
        {
            FinishGame();
        }
    }

    private void TryRevivePiece(ref List<Piece> piecesAlive, ref List<Piece> piecesRemoved)
    {
        switch (_currentPiece)
        {
            case Pawn when _currentPiece.transform.position.y == 7:
            case Pawn when _currentPiece.transform.position.y == 0:
                RevivePiece(ref piecesAlive, ref piecesRemoved);
                break;
        }
    }

    private void RevivePiece(ref List<Piece> piecesAlive, ref List<Piece> piecesRemoved)
    {
        var pieceToReviveValue = 0;
        var pieceToRevive = (Piece) null;

        foreach (var pieceRemoved in piecesRemoved)
        {
            var pieceRemovedValue = pieceRemoved.GetPieceValue();

            if (pieceRemovedValue > pieceToReviveValue)
            {
                pieceToReviveValue = pieceRemovedValue;
                pieceToRevive = pieceRemoved;
            }
        }

        piecesAlive.Remove(_currentPiece);
        piecesRemoved.Add(_currentPiece);

        piecesRemoved.Remove(pieceToRevive);
        piecesAlive.Add(pieceToRevive);

        Board.GetNode(_currentNodePosition).StorePiece(pieceToRevive);

        var position = _currentPiece.transform.position;
        pieceToRevive.transform.position = position;
        position.z = 10;
        _currentPiece.transform.position = position;
    }

    private bool KingCaptured()
    {
        return _capturedPiece is King;
    }

    private void FinishGame()
    {
        _victoryText.SetActive(true);

        _winner.text = _capturedPiece.GetPieceColor() switch
        {
            PieceColor.White => "Black",
            PieceColor.Black => "White",
            _ => throw new ArgumentOutOfRangeException()
        };
        
        Invoke(nameof(LoadScene), 3f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}