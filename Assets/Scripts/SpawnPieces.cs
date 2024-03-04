using System;
using Pieces;
using UnityEngine;

public class SpawnPieces : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    [SerializeField] private Pawn _pawnWhite;
    [SerializeField] private Pawn _pawnBlack;
    [SerializeField] private Rook _rook;
    [SerializeField] private Knight _knight;
    [SerializeField] private Bishop _bishop;
    [SerializeField] private Queen _queen;
    [SerializeField] private King _king;

    private int _x;
    private int _y;

    private void Start()
    {
        for (_x = 0; _x < 8; _x++)
        {
            for (_y = 0; _y < 8; _y++)
            {
                switch (_y)
                {
                    case > 1 and < 6:
                        break;
                    case 1:
                        SpawnPiece(_pawnWhite);
                        break;
                    case 6:
                        SpawnPiece(_pawnBlack);
                        break;

                    case 0 or 7:
                        switch (_x)
                        {
                            case 0 or 7:
                                SpawnPiece(_rook);
                                break;

                            case 1 or 6:
                                SpawnPiece(_knight);
                                break;

                            case 2 or 5:
                                SpawnPiece(_bishop);
                                break;

                            case 3:
                                SpawnPiece(_queen);
                                break;

                            case 4:
                                SpawnPiece(_king);
                                break;
                        }
                        break;
                }
            }
        }
    }

    private void SpawnPiece(Piece prefab)
    {
        var piece = Instantiate(prefab, new Vector3(_x, _y, 0), Quaternion.Euler(-90f, 0, 0), _parent);

        var color = _y switch
        {
            0 or 1 => PieceColor.White,
            6 or 7 => PieceColor.Black,
            _ => throw new ArgumentOutOfRangeException(nameof(_y), _y, null)
        };

        piece.SetPieceColor(color);

        piece.GetComponent<MeshRenderer>().material.color = color switch
        {
            PieceColor.White => Color.white,
            PieceColor.Black => Color.black,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };

        Board.GetNode(_x, _y).StorePiece(piece);
    }
}