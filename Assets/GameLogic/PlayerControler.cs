using UnityEngine;
using Assets.GameLogic.CellClasses;
namespace Assets.GameLogic
{
    public class PlayerControler
    {
        public const int PORTALPROFIT = 50;

        private int turnsCount;
        private static PlayerControler instance;
        private bool turnOfLight;
        private int lightsEnergy;
        private int darksEnergy;

        public enum Player
        {
            Light, Dark
        }

        public PlayerControler()
        {
            turnOfLight = true;
            lightsEnergy = 100;
            darksEnergy = 100;
            GameEvents.GetInstance().OnPlayerChanged += PassTheMove;
        }

        public static PlayerControler GetInstance()
        {
            if (instance == null)
                instance = new PlayerControler();
            return instance;
        }

        public void PassTheMove()
        {
            turnOfLight = !turnOfLight;
            if(turnOfLight)
            {
                turnsCount++;
            }
            AddEnergy();
        }

        public Player GetCurrentPlayer()
        {
            if (turnOfLight)
                return Player.Light;
            else
                return Player.Dark;
        }
        public bool FirstPlayerTurn()
        {
            return turnOfLight;
        }

        public int GetTurnsCount()
        {
            return turnsCount;
        }

        public void AddEnergy()
        {
            if (turnsCount > 0)
            {
                if (turnOfLight)
                {
                    lightsEnergy += CellManager.GetInstance().LightPortalCount() * PORTALPROFIT;
                }
                else
                {
                    darksEnergy += CellManager.GetInstance().DarkPortalCount() * PORTALPROFIT;
                }
            }            
        }

        public void SpendEnergy(int value)
        {
            if(turnOfLight)
            {
                lightsEnergy -= value;
            }
            else
            {
                darksEnergy -= value;
            }
        }

        public int GetEnergy()
        {
            return turnOfLight ? lightsEnergy : darksEnergy;
        }
    }
}
