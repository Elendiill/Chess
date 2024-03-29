using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
    };
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        avaliableMoves.Clear();
        float range = Board.BOARD_SIZE;
        foreach (var direction in directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = occupiedSquare + direction * i;
                Piece piece = board.GetPieceOnSquare(nextCoords);
                if (!board.CheckIfCoordinatesOnBoard(nextCoords))
                    break;
                if (piece == null)
                    TryAddMove(nextCoords);
                else if (!piece.IsFromSameTeam(this))
                {
                    TryAddMove(nextCoords);
                    break;
                }
                else if (piece.IsFromSameTeam(this))
                    break;

            }
        }
        return avaliableMoves;
    }
}
