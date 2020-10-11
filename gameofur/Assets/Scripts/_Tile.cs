using UnityEngine;

public class _Tile : MonoBehaviour
{

    public bool isSafe;
    Piece currentPiece;

    public void TakeThisTile(Piece _piece)
    {   // _piece takes this tile
        currentPiece = _piece;
    }

    public void FreeThisTile()
    {   // no piece on this tile
        currentPiece = null;
    }

    public bool BlockCheck(int _playerIndex)
    {   // if Tile is Empty: not blocked
        if (currentPiece == null) return false;
        // (taken by me) or (safe tile) : blocked
        return (currentPiece.playerIndex == _playerIndex || isSafe);
    }

    public Piece WillKillHappen()
    {
        // if a piece can move to this tile then it's an enemy
        // null: No kill  else: Kill this piece
        return currentPiece;
    }

}
