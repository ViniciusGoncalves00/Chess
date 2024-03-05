using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Rook : Piece
    {
        private readonly List<Vector2Int> _possibleMovements = new List<Vector2Int>();

        private bool _forward;
        private bool _backward;
        private bool _right;
        private bool _left;
        
        public override List<Vector2Int> PossibleMovements(Vector2Int position)
        {
            _possibleMovements.Clear();

            _forward = true;
            _backward = true;
            _left = true;
            _right = true;

            for (int i = 1; i < 8; i++)
            {
                OrthogonalMovement(position, i);
            }

            return _possibleMovements;
        }

        private void OrthogonalMovement(Vector2Int position, int value)
        {
            var directionForward = new Vector2Int(position.x, position.y + value);
            var directionBackward = new Vector2Int(position.x, position.y - value);
            var directionLeft = new Vector2Int(position.x - value, position.y);
            var directionRight = new Vector2Int(position.x + value, position.y);

            if (_forward)
            {
                _forward = PossibleMovement(directionForward);
            }
            
            if (_backward)
            {
                _backward = PossibleMovement(directionBackward);
            }
            
            if (_left)
            {
                _left = PossibleMovement(directionLeft);
            }
            
            if (_right)
            {
                _right = PossibleMovement(directionRight);
            }
        }

        private bool PossibleMovement(Vector2Int direction)
        {
            if (InsideOfBounds(direction))
            {
                var node = Board.GetNode(direction);
                
                if (node.HasPiece() == false)
                {
                    _possibleMovements.Add(direction);
                    return true;
                }
                
                if (node.GetPiece().GetPieceColor() != PieceColor)
                {
                    _possibleMovements.Add(direction);
                    return false;
                }
                
                return false;
            }
            
            return false;
        }
    }
}