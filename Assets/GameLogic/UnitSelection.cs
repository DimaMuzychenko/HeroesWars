using UnityEngine;

namespace Assets.GameLogic
{
    class UnitSelection
    {
        private Unit unit;
        private bool isUnitSelected;
        private static UnitSelection instance;

        public UnitSelection()
        {
            isUnitSelected = false;
        }

        public static UnitSelection GetInstance()
        {
            if(instance == null)
            {
                instance = new UnitSelection();
            }
            return instance;
        }

        public void SelectUnit(Unit unit)
        {
            this.unit = unit;
            isUnitSelected = true;
        }

        public void DeselectUnit()
        {
            if(unit != null)
                unit.HideActions();
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
