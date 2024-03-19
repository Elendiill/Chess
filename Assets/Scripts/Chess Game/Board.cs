using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SquareSelectorCreator))]
public class Board : MonoBehaviour
{
    [SerializeField] private Transform bottomLeftSquareTransfrom;
    [SerializeField] private float squareSize;
    [SerializeField] private float scale;

    private Piece[,] grid;
    private Piece selectedPiece;
    public const int BOARD_SIZE = 8;
    private ChessGameController chessGameController;
    private SquareSelectorCreator squareSelector;
    private void Awake()
    {
        CreateGrid();
        squareSelector = GetComponent<SquareSelectorCreator>();
    }

    public void SetDependencies(ChessGameController chessGameController)
    {
        this.chessGameController = chessGameController;
    }

    private void CreateGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }

    internal Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransfrom.position + new Vector3(coords.x * squareSize * scale, coords.y * squareSize * scale, 0f);
    }

    internal bool HasPiece(Piece piece)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (grid[i, j] == piece)
                {
                    return true;
                }
            }
        }
        return false;
    }

    internal void onSquareSelected(Vector3 inputPosition)
    {
        if (!chessGameController.IsGameInProgress())
            return;
        
        Vector2Int coords = CalculatCoordsFromPosition(inputPosition);
        Piece piece = GetPieceOnSquare(coords);
        
        if (selectedPiece)
        {
            
            if (piece != null && selectedPiece == piece)
                DeselectPiece();
            
            else if (piece != null && selectedPiece != piece && chessGameController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
            
            else if (selectedPiece.CanMoveTo(coords))
                OnSelectedPieceMoved(coords, selectedPiece);
        }
        else
        {
            if (piece != null && chessGameController.IsTeamTurnActive(piece.team))
                SelectPiece(piece);
            
        }
    }

    private void OnSelectedPieceMoved(Vector2Int coords, Piece piece)
    {
        TryToTakeOppositePiece(coords);
        UpdateBoardOnPieceMove(coords, piece.occupiedSquare, piece, null);
        selectedPiece.MovePiece(coords);
        DeselectPiece();
        EndTurn();
    }

    private void TryToTakeOppositePiece(Vector2Int coords)
    {
        Piece piece = GetPieceOnSquare(coords);
        if (piece != null && !selectedPiece.IsFromSameTeam(piece))
            TakePiece(piece);
    }

    private void TakePiece(Piece piece)
    {
        if (piece)
        {
            grid[piece.occupiedSquare.x, piece.occupiedSquare.y] = null;
            chessGameController.OnPieceRemoved(piece);
        }
    }

    private void EndTurn()
    {
        chessGameController.EndTurn();
    }

    public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }

    private void SelectPiece(Piece piece)
    {
        chessGameController.RemoveMovesEnablingAttackOnPiecesOfType<King>(piece);
        selectedPiece = piece;
        List<Vector2Int> selection = selectedPiece.avaliableMoves;
        ShowSelectionSquares(selection);
    }

    private void ShowSelectionSquares(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 position = CalculatePositionFromCoords(selection[i]);
            bool isSquareFree = GetPieceOnSquare(selection[i]) == null;
            squaresData.Add(position, isSquareFree);
        }
        squareSelector.ShowSelection(squaresData);
    }

    private void DeselectPiece()
    {
        selectedPiece = null;
        squareSelector.ClearSelection();
    }

    public Piece GetPieceOnSquare(Vector2Int coords)
    {
        if (CheckIfCoordinatesOnBoard(coords))
        {
            return grid[coords.x, coords.y];
        }
        return null;
    }

    public bool CheckIfCoordinatesOnBoard(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
            return false;
        return true;
    }

    private Vector2Int CalculatCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / squareSize) + BOARD_SIZE / 2 - 1;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).y / squareSize) + BOARD_SIZE / 2 + 4;
        return new Vector2Int(x, y);
    }

    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if (CheckIfCoordinatesOnBoard(coords))
            grid[coords.x, coords.y] = piece;
    }

    
}
