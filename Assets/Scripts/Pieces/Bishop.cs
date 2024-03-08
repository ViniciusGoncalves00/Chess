using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Bishop : Piece
    {
        private protected override int PieceValue { get; set; } = 5;
        
        private readonly List<Vector2Int> _possibleMovements = new List<Vector2Int>();

        private bool _leftForward;
        private bool _rightForward;
        private bool _leftBackward;
        private bool _rightBackward;

        public override List<Vector2Int> PossibleMovements(Vector2Int position)
        {
            _possibleMovements.Clear();

            _leftForward = true;
            _rightForward = true;
            _leftBackward = true;
            _rightBackward = true;

            var x = 1;
            var y = 1;

            for (int i = 1; i < 8; i++)
            {
                DiagonalMovement(position, x, y);
                x++;
                y++;
            }

            return _possibleMovements;
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