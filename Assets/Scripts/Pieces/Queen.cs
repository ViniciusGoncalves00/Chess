using UnityEngine;

namespace Pieces
{
    public class Queen : Piece
    {
        public override Vector2Int[] MovementPossibilities(int x, int y)
        {
            throw new System.NotImplementedException();
        }
        public override void Move()
        {
        }
        public override bool ValidIndex(int x, int y, int xValue, int yValue)
        {
            throw new System.NotImplementedException();
        }
    }
}