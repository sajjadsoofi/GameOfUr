using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    int currentPlayer; //   0: player_1     1: player_2
    int diceValue; // 0, 1, 2, 3, 4
    public float diceDelayTime; // diceval == 0
    public Player[] myPlayers;
    public Camera mainCamera;

    ////////// UI //////////
    public Button dice;
    public Text diceValueText;
    public Text playerTurnText;

    void Start()
    {
        currentPlayer = 0;
        for (int i = 0; i < myPlayers.Length; i++)
        {
            myPlayers[i].SetPlayer(i);
        }
    }

    public void OnDiceRolled()
    {
        dice.interactable = false;
        diceValue = Random.Range(0, 5);
        diceValueText.text = diceValue.ToString();
        if (diceValue == 0) StartCoroutine(ActivateDiceDelayed());
        else myPlayers[currentPlayer].CheckForOptions(diceValue);
    }

    void OnSwitchTurn()
    {
        currentPlayer = 1 - currentPlayer;
        playerTurnText.text = (currentPlayer + 1).ToString();
    }

    void OnSameTurn()
    {
        Debug.Log("Roll Again!");
    }

    void ActivateDice()
    {
        dice.interactable = true;
        diceValueText.text = "-";
    }

    IEnumerator ActivateDiceDelayed()
    {
        yield return new WaitForSeconds(diceDelayTime);
        OnSwitchTurn();
        ActivateDice();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Piece piece = hit.transform.GetComponent<Piece>();
                if (piece != null)
                {
                    MoveThisPiece(currentPlayer, piece.GetPieceIndex(), diceValue);
                }
            }
        }
    }

    void MoveThisPiece(int _playerIndex, int _pieceIndex, int _diceval)
    {
        if (!myPlayers[_playerIndex].IsThisPieceActive(_pieceIndex)) return;

        // Fill end action to perform when reached
        PieceReachedHandler endAction = () => ActivateDice();
        if (!myPlayers[_playerIndex].IsThisTileSafe(_pieceIndex, _diceval)) endAction += () => { OnSwitchTurn(); };
        else endAction += () => { OnSameTurn(); };

        myPlayers[_playerIndex].NavigateThisPiece(_pieceIndex, _diceval, endAction);
    }

}
