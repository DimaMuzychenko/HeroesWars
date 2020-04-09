using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.GameLogic.CellClasses
{
    public class Cell : MonoBehaviour
    {        
        public CellType type;
        public enum CellType
        {
            Portal, LPortal, RPortal, Grass, Send, Rock
        }

        private void OnMouseUp()
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                GameEvents.GetInstance().CellClicked(transform.position);
            }
        }
    }
}
