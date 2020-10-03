using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    DiceRoller diceRoller;
    public Tile startingTile;
    Tile currentTile;
    // Start is called before the first frame update
    void Start()
    {
        diceRoller = GameObject.FindObjectOfType<DiceRoller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        //TODO : Resolve UI and GO click conflict.
        Debug.Log("click");

        if (diceRoller.isDoneRolling == false)
        {
            //Can't Move the Piece
            return;
        }

        int spacesToMove = diceRoller.diceTotal;

        Tile finalTile = currentTile; // goal tile . the tile that we are supposed to go 
                                      // regarding the dice number(spaceToMove)
                                      //If we roll zero => finalTile = currentTile

        for (int i = 0; i < spacesToMove; i++) // loops the amount of dice number
        {
            if (finalTile == null)//if we are not on the board
            {
                finalTile = startingTile;
            }
            else
            {
                finalTile = finalTile.nextTiles[0];
            }
        }

        if (finalTile == null)
        {
            return;
        }

        // Teleport the tile to final tile 
        this.transform.position = finalTile.transform.position;
        Debug.Log("tile : " + finalTile.transform.position);
        Debug.Log("piece : " + this.transform.position);
        currentTile = finalTile;
    }
}
