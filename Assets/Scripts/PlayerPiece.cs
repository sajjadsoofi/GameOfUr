using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    
    Tile currentTile;
    StateManager stateManager;
    
    public Tile startingTile;
    public GameObject home;
    public PieceStorage myPieceStorage;


    bool scoreMe = false;
    bool isAnimating = false;

    Tile[] moveQueue;
    int moveQueueIndex;

    Vector2 targetPosition; // Keep track of target position for smooth transition between tiles

    public int playerID;

    PlayerPiece pieceToKick;

    // Start is called before the first frame update
    void Start()
    {
        stateManager = GameObject.FindObjectOfType<StateManager>();

        targetPosition = this.transform.position;
    }


    Vector2 velocity;
    float smoothTime = 0.2f;

    // Update is called once per frame
    void Update()
    {
        if (this.isAnimating == false)
        {
            return;
        }

        if (Vector2.Distance( (Vector2)this.transform.position , targetPosition) < 0.01f )// TODO : Remove Hared coded 0.01f
        {
            if (moveQueue != null && moveQueueIndex < moveQueue.Length)
            {
                Tile nextTile = moveQueue[moveQueueIndex];
                if (nextTile == null)
                {
                    //The piece reached Home
                    SetNewTargetPosition((Vector2)home.transform.position);
                    stateManager.isDoneAnimating = true;
                }
                else
                {
                    SetNewTargetPosition(nextTile.transform.position);
                    moveQueueIndex++;
                }
                
            }
            else
            {
                if (pieceToKick != null)
                {
                    pieceToKick.ReturnTOStorage();
                    pieceToKick = null;
                }
                this.isAnimating = false;
                stateManager.isDoneAnimating = true;

                // Are we on a safe tile?
                if (currentTile != null && currentTile.isSafe)
                {
                    stateManager.rollAgain();
                }
            }
        }
        // TODO: potential performance issue (smoothDamp)
        this.transform.position = Vector2.SmoothDamp(this.transform.position, targetPosition, ref velocity, smoothTime);
    }

    void SetNewTargetPosition(Vector2 position) // Helper function for target properties when reseting new target
    {
        targetPosition = position;
        velocity = Vector2.zero;
        isAnimating = true;
    }

    private void OnMouseUp()
    {
        //TODO : Resolve UI and G.O click conflict.
        // Is this the correct player?
        if (stateManager.currentPlayerID != playerID)
        {
            return;
        }
        if (stateManager.isDoneRolling == false)
        {
            //Can't Move the Piece
            return;
        }
        if (stateManager.isDoneClicking == true)
        {
            return;
        }
        int spacesToMove = stateManager.diceTotal;

        if (spacesToMove == 0)
        {
            return;
        }
        

        // Where should we end up ?
        moveQueue = GetTilesAhead(spacesToMove);
        Tile finalTile = moveQueue[moveQueue.Length - 1]; // We are getting the final item from the tiles' list above

        // TODO : Check to see if the move is legal.
        if (finalTile == null)
        {
            scoreMe = true; // the piece reached home
        }
        else
        {
            if (CanLegallyMoveTo(finalTile)==false)
            {
                //not allowed
                finalTile = currentTile;
                moveQueue = null;
                return;
            }
            // if there is an enemy in our legal tile , we kick it out.
            if (finalTile.playerPiece != null)
            {
                //finalTile.playerPiece.ReturnTOStorage();

                // We store the piece To Kick out and we will do the kick out when we reached our destination
                pieceToKick = finalTile.playerPiece; 
                pieceToKick.currentTile.playerPiece = null;
                pieceToKick.currentTile = null;
            }
        }

        this.transform.SetParent(null);

        // Remove ourselves from our old tile.
        if (currentTile != null)
        {
            currentTile.playerPiece = null;
        }

        // put ourselves in our new tile.
        finalTile.playerPiece = this;

        moveQueueIndex = 0;
        currentTile = finalTile;
        stateManager.isDoneClicking = true;
        this.isAnimating = true;
    }
    private Tile[] GetTilesAhead(int spacesToMove) // Return the list of tiles moves ahead of us
    {
        if (spacesToMove == 0)
        {
            return null;
        }

        Tile[] listOfTiles = new Tile[spacesToMove]; //moveQueue will be set as zero to four

        Tile finalTile = currentTile;       // goal tile . the tile that we are supposed to go 
                                            // regarding the dice number(spaceToMove)
                                            //If we roll zero => finalTile = currentTile

        for (int i = 0; i < spacesToMove; i++) // loops the amount of dice number
        {
            if (finalTile == null) //if we are not on the board
            {
                finalTile = startingTile;
            }
            else
            {
                if (finalTile.nextTiles == null || finalTile.nextTiles.Length == 0) // Checking if we've
                {                                                                   //reached the end
                    // We are over shooting victory , so just return null 
                    // Just break and we'll return the aray which is gonna have null at the end
                    break;
                }
                else if (finalTile.nextTiles.Length > 1) // Checking the last middle tile 
                {                                        // for splitting p1 and p2 paths
                    //Branch based on player Id 
                    finalTile = finalTile.nextTiles[playerID];
                }
                else
                {
                    finalTile = finalTile.nextTiles[0];
                }
            }

            listOfTiles[i] = finalTile; // Generating the move queue
        }
        return listOfTiles;
    }

    // Returns the final tile we will land on if we moved N spaces
    private Tile GetTileAhead(int spacesToMove)
    {
        Tile[] tiles = GetTilesAhead(spacesToMove);
        if (tiles == null)
        {
            // We are not moving at all , so return current tile
            return currentTile;
        }
        return tiles[tiles.Length - 1 ];
    }

    public bool CanlegallyMoveAhead(int spacesToMove)
    {
        Tile theTile = GetTileAhead(spacesToMove);
        return CanLegallyMoveTo(theTile);
    }

    

    bool CanLegallyMoveTo (Tile destinationTile)
    {
        if (destinationTile == null)
        {
            // A null tile means we are over shooting the victory roll
            return false;
            //Debug.Log("[PlayerPiece.cs] We're tring to move off the board and score.");
            //return true;
        }

        // Is the tile empty?
        if (destinationTile.playerPiece == null)
        {
            return true;
        }
        // Is it one of our own piece
        if (destinationTile.playerPiece.playerID == this.playerID)
        {
            // We can't land on our own piece;
            return false;
        }
        // if it's an enemy piece , Is it in safe tile?
        // Safe Tiles
        if (destinationTile.isSafe == true)
        {
            // Can not kick enemy off
            return false;
        }

        // if We've gotten here , it means we can legally land on the enemy and kick it off the board
        return true;
    }

    public void ReturnTOStorage()
    {
        //currentTile.playerPiece = null;
        //currentTile = null;

        // Save our current position
        Vector2 savePosition = this.transform.position;

        myPieceStorage.AddPieceToStorage(this.gameObject);

        // Set our new position to the animation target
        SetNewTargetPosition(this.transform.position);

        // Restore our saved position
        this.transform.position = savePosition;

    }

}
