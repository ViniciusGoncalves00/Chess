using UnityEngine;

namespace Pieces
{
    public abstract class Piece :  MonoBehaviour
    {
        public abstract Vector2Int[] MovementPossibilities(Vector2Int position);
        public abstract void Move();
        public abstract bool ValidMovement(Vector2Int possibleMovement);
    }
}