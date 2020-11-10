using UnityEngine;

public class _Tile : MonoBehaviour
{

    protected Piece currentPiece;

    public void TakeThisTile(Piece _piece)
    {   // _piece takes this tile
        currentPiece = _piece;
    }

    public void FreeThisTile()
    {   // no piece on this tile
        currentPiece = null;
    }

    public virtual bool BlockCheck(int _playerIndex)
    {   // if Tile is Empty: not blocked
        if (currentPiece == null) return false;
        // (taken by me) or (safe tile) : blocked
        return (currentPiece.playerIndex == _playerIndex);
    }

    public virtual void TileEndAction()
    {
        if (SceneManagerNetWork.instance != null) SceneManagerNetWork.instance.OnSwitchTurnHandler(false);
    }

    public Piece GetEnemyPiece()
    {
        return currentPiece;
    }

}
