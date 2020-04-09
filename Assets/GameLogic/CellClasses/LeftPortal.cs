using UnityEngine;

namespace Assets.GameLogic.CellClasses
{
    class LeftPortal : Cell
    {
        private void Awake()
        {
            type = CellType.LPortal;
        }
    }
}
