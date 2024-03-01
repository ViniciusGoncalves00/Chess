using Pieces;
using UnityEngine;
using UnityEngine.Serialization;

public class Node : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    private Piece _piece;
    private bool _hasPiece;

    private Color _originalColor;
    
    public void SetBaseColor(Color color)
    {
        MeshRenderer.material.color = color;
        _originalColor = color;
    }
    
    public void ChangeColor(Color color)
    {
        MeshRenderer.material.color = color;
    }
    
    public void ResetColor()
    {
        MeshRenderer.material.color = _originalColor;
    }

    public void PutPiece(Piece piece)
    {
        _piece = piece;
        _hasPiece = true;
    }
    
    public void RemovePiece()
    {
        _piece = null;
        _hasPiece = false;
    }

    public Piece GetPiece()
    {
        return _piece;
    }

    public bool HasPiece()
    {
        return _hasPiece;
    }
}