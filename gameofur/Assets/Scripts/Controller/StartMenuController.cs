using System;
using System.Collections;
using System.Collections.Generic;
using FiroozehGameService.Core;
using FiroozehGameService.Core.GSLive;
using FiroozehGameService.Handlers;
using FiroozehGameService.Models.GSLive;
using FiroozehGameService.Models.GSLive.Command;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    public static Member _me, _opponent;
    public Member _currentTurnMember,_whoIsX;
    
    public event Action ReadyToPlay;
    
    // Start is called before the first frame update
    void Start()
    {
        CoreEventHandlers.SuccessfullyLogined += OnSuccessfullyLogined;

        TurnBasedEventHandlers.AutoMatchUpdated += AutoMatchUpdated;
        TurnBasedEventHandlers.JoinedRoom += JoinedRoom;
    }
    
    private void OnSuccessfullyLogined(object sender, EventArgs e)
    {
        try
        {
            startButton.onClick.AddListener(async () =>
            {
                await GameService.GSLive.TurnBased.AutoMatch(new GSLiveOption.AutoMatchOption("partner",2,2));
            });
        }
        catch (Exception exception)
        {
            Debug.LogError("OnSuccessfullyLogined Err : " + exception.Message);
        }
    }

    private async void JoinedRoom(object sender, JoinEvent joinEvent)
    {
        try
        {
            await GameService.GSLive.TurnBased.GetCurrentTurnMember();
            
            if (joinEvent.JoinData.JoinedMember.User.IsMe)
                _me = joinEvent.JoinData.JoinedMember;
            else _opponent = joinEvent.JoinData.JoinedMember;
            
            // Get Players Info
            if(_me == null || _opponent == null)
                await GameService.GSLive.TurnBased.GetRoomMembersDetail();

            // Get CurrentTurn Info
            if (_currentTurnMember == null)
                await GameService.GSLive.TurnBased.GetCurrentTurnMember();
            
            //Debug.Log("JoinedRoom : " + joinEvent.JoinData.JoinedMember.User.Name);
            ReadyToPlay?.Invoke();
        }
        catch (Exception exception)
        {
            Debug.LogError("JoinedRoom Err : " + exception.Message);

        }
    }
    
    private void AutoMatchUpdated(object sender, AutoMatchEvent matchEvent)
    {
        try
        {
            //Debug.Log("Status : "+ matchEvent.Status);
        
            foreach (var player in matchEvent.Players)
            {
                Debug.Log("AutoMatchUpdated" + player.User.Name);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("AutoMatchUpdated Err : " + exception.Message);
        }
       
    }

}
