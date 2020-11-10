using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TileSafe : _Tile
{

    public override bool BlockCheck(int _playerIndex)
    {   // if Tile is Empty: not blocked
        return (currentPiece != null);
    }

    public override void TileEndAction()
    {
        if (SceneManagerNetWork.instance != null) SceneManagerNetWork.instance.OnSameTurnHandler(false);
    }

}
