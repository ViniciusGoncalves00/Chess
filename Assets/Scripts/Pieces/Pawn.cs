using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Pieces
{
    public class Pawn : Piece
    {
        public override Vector2Int[] MovementPossibilities(int x, int y)
        {
            var array = new List<Vector2Int>();

            if (ValidIndex(x, y, 0, 1) && Board.Nodes[x, y + 1].IsEmpty)
            {
                var item = new Vector2Int(x, y + 1);
                array.Add(item);
            }
            if (ValidIndex(x, y, 0, 2) && Board.Nodes[x, y + 2].IsEmpty)
            {
                var item = new Vector2Int(x, y + 2);
                array.Add(item);
            }
            if (ValidIndex(x, y, -1, 1) && Board.Nodes[x - 1, y + 1].IsEmpty == false)
            {
                var item = new Vector2Int(x - 1, y + 1);
                array.Add(item);
            }
            if (ValidIndex(x, y, 1, 1) && Board.Nodes[x + 1, y + 1]!.IsEmpty == false)
            {
                var item = new Vector2Int(x + 1, y + 1);
                array.Add(item);
            }

            return array.ToArray();
        }
        
        public override void Move()
        {
            throw new System.NotImplementedException();
        }
        public override bool ValidIndex(int x, int y, int xValue, int yValue)
        {
            var xIndex = x + xValue;
            var yIndex = y + yValue;

            return xIndex is >= 0 and <= 7 &&
                   yIndex is >= 0 and <= 7;
        }
    }
}
