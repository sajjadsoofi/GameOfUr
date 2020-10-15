using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    int playerIndex;
    Piece[] myPieces;
    public _Tile[] myTiles;

    public void SetPlayer(int _index)
    {
        playerIndex = _index;
        myPieces = GetComponentsInChildren<Piece>();

        float offset = (myPieces[myPieces.Length - 1].transform.position.y - myPieces[0].transform.position.y) / (myPieces.Length - 1);
        for (int i = 0; i < myPieces.Length; i++)
        {
            myPieces[i].SetPiece(i, _index, myPieces[0].transform.position + new Vector3(0, i * offset, 0));
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

    public _Tile GetTile(int _index, int _diceval)
    {
        return myTiles[myPieces[_index].currentTile + _diceval];
    }

    public void CheckForOptions(int _diceval)
    {
        int activated = 0;
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
                    activated++;
                }
            }
        }
        if (activated < 1) SceneManager.instance.OnSwitchTurnHandler(true);
    }

    public void NavigateThisPiece(int _index, int _diceval)
    {
        int targetTile = myPieces[_index].currentTile + _diceval;
        // Find path
        Queue<Vector2> q = new Queue<Vector2>();
        for (int i = myPieces[_index].currentTile + 1; i <= targetTile; i++)
        {
            q.Enqueue((Vector2)myTiles[i].transform.position);
        }

        DeactivateAllMyPieces();

        // SceneManager turn End actions
        EndActionHandler endAction = () => myTiles[targetTile].TileEndAction();
        // Get current piece's end action to execute after other piece reached it
        Piece enemy = myTiles[targetTile].GetEnemyPiece();
        if (enemy != null) endAction += () => { enemy.KillMe(); };

        // Free your tile
        if (myPieces[_index].currentTile != -1)
            myTiles[myPieces[_index].currentTile].FreeThisTile();
        // Take target tile
        myTiles[targetTile].TakeThisTile(myPieces[_index]);

        // Go to tile
        myPieces[_index].Action(q, targetTile, endAction);
    }

}
