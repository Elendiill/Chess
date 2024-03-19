using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IObjectTweener))]
//[RequireComponent(typeof(MaterialSetter))]
public abstract class Piece : MonoBehaviour
{
    //private MaterialSetter materialSetter;
    [SerializeField] protected Sprite[] sprites;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    public Board board {protected get; set; }

    public Vector2Int occupiedSquare {get; set; }
    
    public TeamColor team { get; set; }

    public bool hasMoved { get; private set; }
    public List<Vector2Int> avaliableMoves;

    private IObjectTweener tweener;

    public abstract List<Vector2Int> SelectAvaliableSquares();

    private void Awake()
    {
        avaliableMoves = new List<Vector2Int>();
        tweener = GetComponent<IObjectTweener>();
        hasMoved = false;
    }

    public bool IsFromSameTeam(Piece piece)
    {
        return team == piece.team;
    }

    public bool CanMoveTo(Vector2Int coords)
    {
        return avaliableMoves.Contains(coords);
    }

    public virtual void MovePiece(Vector2Int coords)
    {
        Vector3 targetPosition = board.CalculatePositionFromCoords(coords);
        occupiedSquare = coords;
        hasMoved = true;
        tweener.MoveTo(transform, targetPosition);
    }

    protected void TryAddMove(Vector2Int coords)
    {
        avaliableMoves.Add(coords);
    }

    public void SetData(Vector2Int coords, TeamColor team, Board board)
    {
        this.team = team;
        occupiedSquare = coords;
        this.board = board;
        transform.position = board.CalculatePositionFromCoords(coords);

        if (team == TeamColor.Black)
            spriteRenderer.sprite = sprites[1]; //black

        else
            spriteRenderer.sprite = sprites[0]; //white
    }

    public bool IsAttackingPieceOfType<T>() where T : Piece
    {
        foreach (var square in avaliableMoves)
        {
            if (board.GetPieceOnSquare(square) is T)
                return true;
        }
        return false;
    }
}
