using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameLogic
{
    public class Player
    {
        public PlayerType type;
        public int Energy;
        public List<Unit> units;
        public enum PlayerType
        {
            Ligth, Dark
        }
    }
}
