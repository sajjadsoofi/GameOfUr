﻿using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    int currentPlayer; //   0: player_1     1: player_2
    int diceValue; // 0, 1, 2, 3, 4
    public float diceDelayTime; // diceval == 0
    public Player[] myPlayers;
    public int[] scores;
    public Camera mainCamera;
    public static SceneManager instance;

    ////////// UI //////////
    public Button dice;
    public Text diceValueText;
    public Text playerTurnText;
    public Text[] scoreText;

    void Start()
    {
        currentPlayer = 0;
        scores = new int[myPlayers.Length];

        for (int i = 0; i < myPlayers.Length; i++)
        {
            myPlayers[i].SetPlayer(i);
            scores[i] = 0;
        }

        instance = this;
    }

    public void OnDiceRolled()
    {
        dice.interactable = false;
        diceValue = Random.Range(0, 5);
        diceValueText.text = diceValue.ToString();
        if (diceValue == 0) StartCoroutine(ActivateDiceDelayed());
        else myPlayers[currentPlayer].CheckForOptions(diceValue);
    }

    public void OnSwitchTurnHandler()
    {
        currentPlayer = 1 - currentPlayer;
        playerTurnText.text = (currentPlayer + 1).ToString();
        ActivateDice();
    }

    public void OnSameTurnHandler()
    {
        Debug.Log("Roll Again!");
        ActivateDice();
    }

    void ActivateDice()
    {
        dice.interactable = true;
        diceValueText.text = "-";
    }

    IEnumerator ActivateDiceDelayed()
    {
        yield return new WaitForSeconds(diceDelayTime);
        OnSwitchTurnHandler();
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
        myPlayers[_playerIndex].NavigateThisPiece(_pieceIndex, _diceval);
    }

    public void GrantOneScorePoint(int _playerIndex)
    {
        scores[_playerIndex] += 1;
        scoreText[_playerIndex].text = scores[_playerIndex].ToString();
        if (scores[_playerIndex] > 6)
        {   // On game end
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

}
