using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public int numberOfPlayers = 2;
    public int currentPlayerID = 0;

    public int diceTotal = 5;

    public bool isDoneRolling = false;
    public bool isDoneClicking = false;
    public bool isDoneAnimating = false;

    public GameObject noLegelMovePopUp;

    public void NewTurn()
    {
        //The Start Of Player's turn

        isDoneRolling = false;
        isDoneClicking = false;
        isDoneAnimating = false;

        currentPlayerID = (currentPlayerID +1 ) % numberOfPlayers;
    }

    public void rollAgain()
    {
        Debug.Log("Roll Again!!");
        isDoneRolling = false;
        isDoneClicking = false;
        isDoneAnimating = false;
    }

    // TODO : Use FSM
    // VVVVVVVVVVVVVV

    //public enum turnPhase { WAITING_FOR_ROLL, WAITING_FOR_CLICK, WAITING_FOR_ANIMATION};
    //public turnPhase currentPhase;

    //public void AdvancePhase() { }
    //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoneAnimating && isDoneClicking && isDoneRolling)// Is the turn Over?
        {
            NewTurn();
        }
    }

    public void CheckLegalMoves()
    {
        // If rolled zero => no legal move
        if (diceTotal == 0)
        {
            StartCoroutine("NolegalMoveCoroutine");
            return;
        }

        // Loop through al of player's pieces
        PlayerPiece[] playerPieces = GameObject.FindObjectsOfType<PlayerPiece>();

        bool hasLegalMove = false;


        foreach (PlayerPiece playerPiece in playerPieces)
        {
            if (playerPiece.playerID == currentPlayerID)
            {
                if (playerPiece.CanlegallyMoveAhead(diceTotal))
                {
                    // TODO : Highlight pieces that can be legally moved
                    hasLegalMove = true;
                }

            }
        }

        // If no legel moves are possible , wait a second then move to newxt player

        if (hasLegalMove == false)
        {
            StartCoroutine(NolegalMoveCoroutine());
            return;
        }
    }

    IEnumerator NolegalMoveCoroutine()
    {
        // Display message
        noLegelMovePopUp.SetActive(true);

        // Wait a second
        yield return new WaitForSeconds(1f);
        noLegelMovePopUp.SetActive(false); ;

        NewTurn();
    }
}
