using UnityEngine;

namespace Assets.GameLogic
{
    class UnitSelection : MonoBehaviour
    {
        Unit unit;
        bool isUnitSelected;

        private void Awake()
        {
            isUnitSelected = false;
        }

        public void SelectUnit(Unit unit)
        {
            this.unit = unit;
            isUnitSelected = true;
        }

        public void DeselectUnit()
        {
            this.unit = null;
            isUnitSelected = false;
        }
        public Unit GetSelectedUnit()
        {
            return unit;
        }

        public bool IsUnitSelected()
        {
            return isUnitSelected;
        }
    }
}
