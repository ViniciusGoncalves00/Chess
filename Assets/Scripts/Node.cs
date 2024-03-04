using System;
using Pieces;
using UnityEngine;
using UnityEngine.Serialization;

public class Node : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    private Piece _piece;
    private bool _hasPiece;

    private Color _originalColor;

    public void SetBaseColor(Color color)
    {
        _meshRenderer.material.color = color;
        _originalColor = color;
    }
    
    public void ChangeColor(Color color)
    {
        _meshRenderer.material.color = color;
    }
    
    public void ResetColor()
    {
        _meshRenderer.material.color = _originalColor;
    }

    public void StorePiece(Piece piece)
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