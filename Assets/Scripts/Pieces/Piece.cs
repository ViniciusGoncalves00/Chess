using UnityEngine;

namespace Pieces
{
    public abstract class Piece :  MonoBehaviour
    {
        public abstract Vector2Int[] MovementPossibilities(int x, int y);
        public abstract void Move();
        public abstract bool ValidIndex(int x, int y, int xValue, int yValue);
    }
}