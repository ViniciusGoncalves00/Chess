using UnityEngine;
using UnityEngine.Serialization;

namespace Pieces
{
    public abstract class Piece :  MonoBehaviour
    {
        private protected PieceColor PieceColor;
        public abstract Vector2Int[] MovementPossibilities(Vector2Int position);
        public void SetPieceColor(PieceColor pieceColor)
        {
            PieceColor = pieceColor;
        }
        public PieceColor GetPieceColor()
        {
            return PieceColor;
        }
        public abstract void Move();
        public abstract bool ValidMovement(Vector2Int possibleMovement);
    }
}