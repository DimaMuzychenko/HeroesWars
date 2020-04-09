using UnityEngine;
namespace Assets.GameLogic
{
    public class TurnCounter : MonoBehaviour
    {
        private int turnsCount;
        private static TurnCounter instance;
        private bool firstPlayerTurn;

        public enum Player
        {
            Left, Right
        }
        private void Awake()
        {
            instance = this;
            firstPlayerTurn = true;
        }

        private void Start()
        {
            GameEvents.GetInstance().OnPlayerChanged += PassTheMove;
        }

        public static TurnCounter GetInstance()
        {            
            return instance;
        }

        public void PassTheMove()
        {
            firstPlayerTurn = !firstPlayerTurn;
            if(firstPlayerTurn)
            {
                turnsCount++;
            }
        }

        public Player GetCurrentPlayer()
        {
            if (firstPlayerTurn)
                return Player.Left;
            else
                return Player.Right;
        }
        public bool FirstPlayerTurn()
        {
            return firstPlayerTurn;
        }

        public int GetTurnsCount()
        {
            return turnsCount;
        }
    }
}
