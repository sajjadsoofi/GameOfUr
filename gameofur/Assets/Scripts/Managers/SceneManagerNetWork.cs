using System;
using System.Collections;
using System.Collections.Generic;
using FiroozehGameService.Core;
using FiroozehGameService.Handlers;
using FiroozehGameService.Models;
using FiroozehGameService.Models.GSLive;
using FiroozehGameService.Models.GSLive.TB;
using Handlers;
using Models;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine;
using Random = UnityEngine.Random;

public class SceneManagerNetWork : MonoBehaviour
{
    public Text playerVal, diceVal;
    private Member _me,_opponent,_currentTurnMember,_whoIsX;

    int currentPlayer; //   0: player_1     1: player_2
    int diceValue; // 0, 1, 2, 3, 4
    public float diceDelayTime; // diceval == 0
    public Player[] myPlayers;
    public int[] scores;
    public Camera mainCamera;
    public static SceneManagerNetWork instance;

    ////////// UI //////////
    public Button dice;
    public Text diceValueText;
    public Text playerTurnText;
    public Text[] scoreText;
    void Start()
    {
        scores = new int[myPlayers.Length];

        for (int i = 0; i < myPlayers.Length; i++)
        {
            myPlayers[i].SetPlayer(i);
            scores[i] = 0;
        }
        instance = this;

        SetEventHandlers();
    }

    private void SetEventHandlers()
    {
        TurnBasedEventHandlers.TakeTurn += OnTakeTurn;
        TurnBasedEventHandlers.CurrentTurnMemberReceived += CurrentTurnMemberReceived;
        TurnBasedEventHandlers.RoomMembersDetailReceived += RoomMembersDetailReceived;
    }

   

    private void RoomMembersDetailReceived(object sender, List<Member> members)
    {
        foreach (var member in members)
        {
            if (member.User.IsMe) _me = member;
            else _opponent = member;
        }
    }
    
    private void CurrentTurnMemberReceived(object sender, Member currentMember)
    {
        
        _currentTurnMember = currentMember; 
        
        currentPlayer = 0;

        // Debug.Log(currentMember.User.IsMe);
        //
        if (currentMember.User.IsMe)
        {
            myPlayers[0].IsPlayable = true;                //for activating the dice in each round
            myPlayers[1].IsPlayable = false;
        }
        else
        {
            dice.interactable = false;
            myPlayers[0].IsPlayable = false;               //for activating the dice in each round
            myPlayers[1].IsPlayable = true;
        }
    }

    private void OnTakeTurn(object sender, Turn turn)
    {
        var data = JsonConvert.DeserializeObject<TurnData>(turn.Data);
        if (data.isPiece)
        {
            Debug.Log(data.playerIndex + "  "+ data.pieceIndex+"  " + data.diceValue + data.isPiece);
        
            int pieceIndex = data.pieceIndex;
            int diceVal = data.diceValue;
            int playerIndex = data.playerIndex;
        
            this.diceVal.text = diceVal.ToString();
            playerVal.text = (playerIndex + 1).ToString();
        
            MoveThisPiece(playerIndex, pieceIndex , diceVal);
        }
        else
        {
            var aaaa = JsonConvert.DeserializeObject<OnSwitchTurn>(turn.Data);
            OnSwitchTurnHandler( aaaa.delay);
        
            Debug.LogError("OnSwitchTurn : " + aaaa.delay );   
        }
    }

    public async void OnDiceRolled()
    {
        dice.interactable = false;
        diceValue = 2 - Random.Range(0, 3) + Random.Range(0, 3);
        diceValueText.text = diceValue.ToString();
        
        if (diceValue == 0)
        {
            Debug.LogError("miaim");
            await GsLiveHandler.OnSwitch(true );
        }
        else myPlayers[currentPlayer].CheckForOptions(diceValue);
    }

    public void OnSwitchTurnHandler(bool delayed)
    {
        if (delayed) StartCoroutine(ActivateDiceSwitchDelayed(diceDelayTime));
        else StartCoroutine(ActivateDiceSwitchDelayed(0.1f));
    }

    IEnumerator ActivateDiceSwitchDelayed(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        //currentPlayer = (currentPlayer + 1) % myPlayers.Length; 
        //playerTurnText.text = (currentPlayer + 1).ToString();
        ActivateDice();
    }

    public void OnSameTurnHandler(bool delayed)
    {
        if (delayed) StartCoroutine(ActivateDiceSameDelayed(diceDelayTime));
        else StartCoroutine(ActivateDiceSameDelayed(0.1f));
    }

    IEnumerator ActivateDiceSameDelayed(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        Debug.Log("Roll Again!");
        ActivateDice();
    }

    void ActivateDice()
    {
        dice.interactable = true;
        diceValueText.text = "-";
    }

    async void Update()
    {
        
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Piece piece = hit.transform.GetComponent<Piece>();
                if (piece != null)
                {
                    //MoveThisPiece(currentPlayer, piece.GetPieceIndex(), diceValue);
                    try
                    {
                        await GsLiveHandler.TakeTurn(currentPlayer, piece.GetPieceIndex(), diceValue, true);
                    }
                    catch (GameServiceException e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }
        }
    }

    void MoveThisPiece(int _playerIndex, int _pieceIndex, int _diceval)
    {
        //Debug.Log(!myPlayers[_playerIndex].IsThisPieceActive(_pieceIndex));
        
        //if (!myPlayers[_playerIndex].IsThisPieceActive(_pieceIndex)) return;
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
