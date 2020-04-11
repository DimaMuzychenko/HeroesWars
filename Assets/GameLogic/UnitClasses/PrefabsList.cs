using UnityEngine;

namespace Assets.GameLogic.UnitClasses
{
    [CreateAssetMenu]
    class PrefabsList : ScriptableObject
    {
        [SerializeField] private Unit[] lightUnits;
        [SerializeField] private Unit[] darkUnits;

        public Unit GetLeftUnit(string name)
        {
            foreach(Unit unit in lightUnits)
            {
                if (unit.GetUnitInfo()[1].ToString() == name)
                    return unit;
            }
            return lightUnits[0];
        }

        public Unit GetRightUnit(string name)
        {
            foreach(Unit unit in darkUnits)
            {
                if(unit.GetUnitInfo()[1].ToString() == name)
                {
                    return unit;
                }
            }
            return darkUnits[0];
        }

        public Unit[] GetAllLeftUnits()
        {
            return lightUnits;
        }

        public Unit[] GetAllRightUnits()
        {
            return darkUnits;
        }
    }
}
