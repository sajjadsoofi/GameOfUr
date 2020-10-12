using UnityEngine;

public class _TileHome : _Tile
{
    public override void TileEndAction()
    {
        SceneManager sm = SceneManager.instance;
        if (sm != null)
        {
            sm.OnSwitchTurnHandler(true);
            sm.GrantOneScorePoint(currentPiece.playerIndex);
        }
        currentPiece.WinMe();
        FreeThisTile();
    }
}
