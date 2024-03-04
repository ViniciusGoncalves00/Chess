using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Pawn : Piece, IPawn
    {
        public int Direction;
        private bool _firstMovement = true;
        
        private Vector2Int _position;
        private readonly Vector2Int _oneForward = new Vector2Int(0, 1);
        private readonly Vector2Int _twoForward = new Vector2Int(0, 2);
        private readonly Vector2Int _leftForward = new Vector2Int(-1, 1);
        private readonly Vector2Int _rightForward = new Vector2Int(1, 1);

        private Vector2Int _oneForwardPosition;
        private Vector2Int _twoForwardPosition;
        private Vector2Int _leftForwardPosition;
        private Vector2Int _rightForwardPosition;
        
        public override Vector2Int[] MovementPossibilities(Vector2Int position)
        {
            var possibleMovements = new List<Vector2Int>();
            
            _position = position;
            _oneForwardPosition = _position + new Vector2Int(_oneForward.x, _oneForward.y * Direction);
            _twoForwardPosition = _position + new Vector2Int(_twoForward.x, _twoForward.y * Direction);
            _leftForwardPosition = _position + new Vector2Int(_leftForward.x, _leftForward.y * Direction); 
            _rightForwardPosition = _position + new Vector2Int(_rightForward.x, _rightForward.y * Direction);

            var oneForwardNode = Board.GetNode(_oneForwardPosition);
            
            if (oneForwardNode.HasPiece() == false)
            {
                possibleMovements.Add(_oneForwardPosition);
            }
            
            if (_firstMovement)
            {
                var twoForwardNode = Board.GetNode(_twoForwardPosition);
                
                if (oneForwardNode.HasPiece() == false && twoForwardNode.HasPiece() == false)
                {
                    possibleMovements.Add(_twoForwardPosition);
                }
            }
            
            if (ValidMovement(_leftForwardPosition))
            {
                var leftForwardNode = Board.GetNode(_leftForwardPosition);
                
                if (leftForwardNode.HasPiece() && leftForwardNode.GetPiece().GetPieceColor() != PieceColor)
                {
                    possibleMovements.Add(_leftForwardPosition);
                }
            }
            
            if (ValidMovement(_rightForwardPosition))
            {
                var rightForwardNode = Board.GetNode(_rightForwardPosition);
                
                if (rightForwardNode.HasPiece() && rightForwardNode.GetPiece().GetPieceColor() != PieceColor)
                {
                    possibleMovements.Add(_rightForwardPosition);
                }
            }

            return possibleMovements.ToArray();
        }

        // public override void Move(Vector3Int movement)
        // {
        //     gameObject.transform.position = movement;
        // }
        
        public override void Move()
        {
            throw new System.NotImplementedException();
        }
        
        public override bool ValidMovement(Vector2Int possibleMovement)
        {
            return possibleMovement.x is >= 0 and <= 7 &&
                   possibleMovement.y is >= 0 and <= 7;
        }
        public void FirstMovement()
        {
            _firstMovement = false;
        }
    }
}
