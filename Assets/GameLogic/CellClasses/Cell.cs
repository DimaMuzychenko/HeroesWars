using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.GameLogic.CellClasses
{
    public class Cell : MonoBehaviour
    {        
        public CellType type;
        public enum CellType
        {
            Portal, LightPortal, DarkPortal, Grass, Send, Rock
        }
    }
}
