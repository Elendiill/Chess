using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Board))]
public class BoardInputHandler : MonoBehaviour, IinputHandler
{
    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }
    public void ProcessInput(Vector3 inputPosition, GameObject selectedObject, Action callback)
    {
        board.onSquareSelected(inputPosition);
    }

}
