using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public abstract class Piece :  MonoBehaviour
    {
        private protected PieceColor PieceColor;
        public abstract List<Vector2Int> PossibleMovements(Vector2Int position);
        public void SetPieceColor(PieceColor pieceColor)
        {
            PieceColor = pieceColor;
        }
        public PieceColor GetPieceColor()
        {
            return PieceColor;
        }
        
        protected virtual bool InsideOfBounds(Vector2Int possibleMovement)
        {
            return possibleMovement.x is >= 0 and <= 7 &&
                   possibleMovement.y is >= 0 and <= 7;
        }
    }
}