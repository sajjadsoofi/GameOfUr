using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class TurnData
    {
        [JsonProperty("playerIndex")] 
        public int playerIndex { get; set; }
        
        [JsonProperty("pieceIndex")] 
        public int pieceIndex { get; set; }
        
        [JsonProperty("diceValue")] 
        public int diceValue { get; set; }
         
        [JsonProperty("isPiece")]
        public bool isPiece { get; set; }
    }

    public class OnSwitchTurn
    {
        [JsonProperty("Turn")]
        public bool delay { get; set; }
       
    }
}