using System;
using System.Threading.Tasks;
using FiroozehGameService.Core;
using FiroozehGameService.Models;
using Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Handlers
{
    public static class GsLiveHandler
    {
        public static async Task TakeTurn(int playerIndex, int pieceIndex , int diceValue, bool isPiece)
        {
            var turnData = new TurnData
            {
                playerIndex = playerIndex,
                pieceIndex = pieceIndex,
                diceValue = diceValue,
                isPiece = isPiece
            };

            var dataToSend = JsonConvert.SerializeObject(turnData);
            Debug.Log(dataToSend);
            try
            {
                await GameService.GSLive.TurnBased.TakeTurn(dataToSend);
                await GameService.GSLive.TurnBased.ChooseNext();
            }
            catch (GameServiceException e)
            {
                Debug.LogError(e.Message);
            }
        }

        public static async Task OnSwitch(bool test)
        {
            var OnSwitchTurnData = new OnSwitchTurn
            {
                delay = test,
            };

            var dataToSend = JsonConvert.SerializeObject(OnSwitchTurnData);
            Debug.Log("OnSwitchCalled" + dataToSend);
            try
            {
                await GameService.GSLive.TurnBased.TakeTurn(dataToSend);
                await GameService.GSLive.TurnBased.ChooseNext();
            }
            catch (GameServiceException e)
            {
                Debug.LogError(e.Message);
            }

        }
    }
}