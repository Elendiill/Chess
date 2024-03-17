using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform bottomLeftSquareTransfrom;
    [SerializeField] private float squareSize;

    internal Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransfrom.position + new Vector3(coords.x * squareSize * 1.4f, coords.y * squareSize * 1.4f, 0f);
    }

    internal bool HasPiece(Piece piece)
    {
        throw new NotImplementedException();
    }
}
