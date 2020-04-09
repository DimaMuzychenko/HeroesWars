using UnityEngine;

namespace Assets.GameLogic.CellClasses
{
    class RightPortal : Cell
    {
        private void Awake()
        {
            type = CellType.RPortal;
        }
    }
}
