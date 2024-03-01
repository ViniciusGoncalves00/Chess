using System.Collections.Generic;
using UnityEngine;

namespace Pieces
{
    public class Pawn : Piece
    {
        public int Direction;
        
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
            var array = new List<Vector2Int>();
            
            _position = position;
            _oneForwardPosition = _position + new Vector2Int(_oneForward.x, _oneForward.y * Direction);
            _twoForwardPosition = _position + new Vector2Int(_twoForward.x, _twoForward.y * Direction);
            _leftForwardPosition = _position + new Vector2Int(_leftForward.x, _leftForward.y * Direction); 
            _rightForwardPosition = _position + new Vector2Int(_rightForward.x, _rightForward.y * Direction);

            if (ValidMovement(_oneForwardPosition) && Board.GetNode(_oneForwardPosition).HasPiece() == false)
            {
                array.Add(_oneForwardPosition);
            }
            if (ValidMovement(_twoForwardPosition) && Board.GetNode(_oneForwardPosition).HasPiece() == false && Board.GetNode(_twoForwardPosition).HasPiece() == false)
            {
                array.Add(_twoForwardPosition);
            }
            if (ValidMovement(_leftForwardPosition) && Board.GetNode(_leftForwardPosition).HasPiece())
            {
                array.Add(_leftForwardPosition);
            }
            if (ValidMovement(_rightForwardPosition) && Board.GetNode(_rightForwardPosition).HasPiece())
            {
                array.Add(_rightForwardPosition);
            }

            return array.ToArray();
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
    }
}
