using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class King : Piece
    {
        private protected override int PieceValue { get; set; } = 0;
        
        private readonly List<Vector2Int> _possibleMovements = new List<Vector2Int>();

        private bool _forward;
        private bool _backward;
        private bool _right;
        private bool _left;

        private bool _leftForward;
        private bool _rightForward;
        private bool _leftBackward;
        private bool _rightBackward;

        public override List<Vector2Int> PossibleMovements(Vector2Int position)
        {
            _possibleMovements.Clear();

            _forward = true;
            _backward = true;
            _left = true;
            _right = true;

            _leftForward = true;
            _rightForward = true;
            _leftBackward = true;
            _rightBackward = true;

            var i = 1;
            var x = 1;
            var y = 1;
            
            OrthogonalMovement(position, i);
            DiagonalMovement(position, x, y);

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

        private void DiagonalMovement(Vector2Int position, int x, int y)
        {
            var directionLeftForward = new Vector2Int(position.x - x, position.y + y);
            var directionRightForward = new Vector2Int(position.x + x, position.y + y);
            var directionLeftBackward = new Vector2Int(position.x - x, position.y - y);
            var directionRightBackward = new Vector2Int(position.x + x, position.y - y);

            if (_leftForward)
            {
                _leftForward = PossibleMovement(directionLeftForward);
            }

            if (_rightForward)
            {
                _rightForward = PossibleMovement(directionRightForward);
            }

            if (_leftBackward)
            {
                _leftBackward = PossibleMovement(directionLeftBackward);
            }

            if (_rightBackward)
            {
                _rightBackward = PossibleMovement(directionRightBackward);
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