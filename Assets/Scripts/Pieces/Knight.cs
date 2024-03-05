using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Knight : Piece
        {
        private readonly List<Vector2Int> _possibleMovements = new List<Vector2Int>();
        
        public override List<Vector2Int> PossibleMovements(Vector2Int position)
        {
            _possibleMovements.Clear();

            var x = 1;
            var y = 2;
            
            for (int i = 1; i <= 2; i++)
            {
                LMovement(position, x, y);
                x++;
                y--;
            }

            return _possibleMovements;
        }

        private void LMovement(Vector2Int position, int x, int y)
        {
            var directionLeftForward = new Vector2Int(position.x - x, position.y + y);
            var directionRightForward = new Vector2Int(position.x + x, position.y + y);
            var directionLeftBackward = new Vector2Int(position.x - x, position.y - y);
            var directionRightBackward = new Vector2Int(position.x + x, position.y - y);
            
            PossibleMovement(directionLeftForward); 
            PossibleMovement(directionRightForward); 
            PossibleMovement(directionLeftBackward); 
            PossibleMovement(directionRightBackward);
        }

        private void PossibleMovement(Vector2Int direction)
        {
            if (InsideOfBounds(direction))
            {
                var node = Board.GetNode(direction);
                
                if (node.HasPiece() && node.GetPiece().GetPieceColor() == PieceColor)
                {
                    return;
                }
                
                _possibleMovements.Add(direction);
            }
        }
    }
}