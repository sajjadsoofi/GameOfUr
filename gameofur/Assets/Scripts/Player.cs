using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    int playerIndex;
    Piece[] myPieces;
    Vector3[] myPlaceholders;
    public _Tile[] myTiles;

    public void SetPlayer(int _index)
    {
        playerIndex = _index;
        myPieces = GetComponentsInChildren<Piece>();

        CalculatePlaceholders();

        for (int i = 0; i < myPieces.Length; i++)
        {
            myPieces[i].SetPiece(i, _index);
            myPieces[i].transform.localPosition = myPlaceholders[i];
        }
    }

    void CalculatePlaceholders()
    {
        myPlaceholders = new Vector3[myPieces.Length];
        myPlaceholders[0] = myPieces[0].transform.localPosition;
        float offset = (myPieces[myPieces.Length - 1].transform.localPosition.y - myPieces[0].transform.localPosition.y) / (myPieces.Length - 1);

        for (int i = 0; i < myPlaceholders.Length; i++)
        {
            myPlaceholders[i] = myPlaceholders[0] + new Vector3(0, i * offset, 0);
        }
    }

    void DeactivateAllMyPieces()
    {
        for (int i = 0; i < myPieces.Length; i++)
        {
            myPieces[i].Deactivate();
        }
    }

    public bool IsThisPieceActive(int _index)
    {
        return myPieces[_index].isActive;
    }

    public bool IsThisTileSafe(int _index, int _diceval)
    {
        return myTiles[myPieces[_index].currentTile + _diceval].isSafe;
    }

    public void CheckForOptions(int _diceval)
    {
        for (int i = 0; i < myPieces.Length; i++)
        {
            int targetTileIndex = myPieces[i].currentTile + _diceval;
            // Activation conditions
            if (targetTileIndex < myTiles.Length) // condition #1: Tiles length
            {
                if (!myTiles[targetTileIndex].BlockCheck(playerIndex)) // condition #2: Tile not blocked
                {
                    // Only activate these pieces
                    myPieces[i].Activate();
                }
            }
        }
    }

    public void NavigateThisPiece(int _index, int _diceval, PieceReachedHandler _action)
    {
        int targetTile = myPieces[_index].currentTile + _diceval;
        // Find path
        Queue<Vector2> q = new Queue<Vector2>();
        for (int i = myPieces[_index].currentTile + 1; i <= targetTile; i++)
        {
            q.Enqueue((Vector2)myTiles[i].transform.position);
        }

        // Free your tile
        if (myPieces[_index].currentTile != -1)
            myTiles[myPieces[_index].currentTile].FreeThisTile();
        // Take target tile
        myTiles[targetTile].TakeThisTile(myPieces[_index]);

        // Set end action
        myPieces[_index].OnPathEnded += _action;

        // Go to tile
        myPieces[_index].Action(q, targetTile);
        DeactivateAllMyPieces();
    }

}
