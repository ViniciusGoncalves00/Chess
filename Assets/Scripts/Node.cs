using Pieces;
using UnityEngine;

public class Node : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    public Piece Piece { get; private set; }
    public bool IsEmpty = true;

    private Color OriginalColor;
    
    public void SetBaseColor(Color color)
    {
        MeshRenderer.material.color = color;
        OriginalColor = color;
    }
    
    public void ChangeColor(Color color)
    {
        MeshRenderer.material.color = color;
    }
    
    public void ResetColor()
    {
        MeshRenderer.material.color = OriginalColor;
    }

    public void SetPiece(Piece piece)
    {
        Piece = piece;
        IsEmpty = false;
    }
}