using UnityEngine;

namespace Assets.GameLogic.CellClasses
{
    class Portal : Cell
    {
        private void Awake()
        {
            type = CellType.Portal;
        }
    }
}
