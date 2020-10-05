using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    DiceRoller diceRoller;
    public Tile startingTile;
    public GameObject home;
    Tile currentTile;

    bool scoreMe = false;

    Tile[] moveQueue;
    int moveQueueIndex;

    Vector2 targetPosition; // Keep track of target position for smooth transition between tiles


    // Start is called before the first frame update
    void Start()
    {
        diceRoller = GameObject.FindObjectOfType<DiceRoller>();

        targetPosition = this.transform.position;
    }


    Vector2 velocity;
    float smoothTime = 0.2f;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance( (Vector2)this.transform.position , targetPosition) < 0.01f )// Remove Hared coded 0.01f
        {
            if (moveQueue != null && moveQueueIndex < moveQueue.Length)
            {
                Tile nextTile = moveQueue[moveQueueIndex];
                if (nextTile == null)
                {
                    //The piece reached Home
                    SetNewTargetPosition((Vector2)home.transform.position);
                }
                else
                {
                    SetNewTargetPosition(nextTile.transform.position);
                    moveQueueIndex++;
                }
                
            }
        }
        // TODO potential performance issue (smoothDamp)
        this.transform.position = Vector2.SmoothDamp(this.transform.position, targetPosition, ref velocity, smoothTime);
    }

    void SetNewTargetPosition(Vector2 position) // Helper function for target properties when reseting new target
    {
        targetPosition = position;
        velocity = Vector2.zero;
    }

    private void OnMouseUp()
    {
        //TODO : Resolve UI and G.O click conflict.
        //Debug.Log("click");

        if (diceRoller.isDoneRolling == false)
        {
            //Can't Move the Piece
            return;
        }

        int spacesToMove = diceRoller.diceTotal;

        if (spacesToMove == 0)
        {
            return;
        }

        moveQueue = new Tile[spacesToMove]; //moveQueue will be set as zero to four

        Tile finalTile = currentTile;       // goal tile . the tile that we are supposed to go 
                                            // regarding the dice number(spaceToMove)
                                            //If we roll zero => finalTile = currentTile
        
        

        for (int i = 0; i < spacesToMove; i++) // loops the amount of dice number
        {
            if (finalTile == null && scoreMe == false) //if we are not on the board
            {
                finalTile = startingTile;
            }
            else
            {
                if (finalTile.nextTiles == null || finalTile.nextTiles.Length == 0) // Checking if we've
                {                                                                   //reached the end
                    //Debug.Log("Score");
                    //Destroy(gameObject); // We are doing this temporarly
                    //return;              // TODO : Resolve Reaching the end.
                    scoreMe = true;
                    finalTile = null;
                }
                else if (finalTile.nextTiles.Length > 1)
                {
                    //TODO : Branch based on player Id
                    finalTile = finalTile.nextTiles[0];
                }
                else
                {
                    finalTile = finalTile.nextTiles[0];
                }
            }

            moveQueue[i] = finalTile; // Generating the move queue
        }

        // Teleport the tile to final tile 
        //this.transform.position = finalTile.transform.position;


        //SetNewTargetPosition(finalTile.transform.position);
        moveQueueIndex = 0;
        currentTile = finalTile;
    }
}
