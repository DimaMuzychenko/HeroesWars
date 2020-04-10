using UnityEngine;

namespace Assets.GameLogic.UnitClasses
{
    [CreateAssetMenu]
    class PrefabsList : ScriptableObject
    {
        [SerializeField] private Unit[] lightUnits;
        [SerializeField] private Unit[] darkUnits;

        public Unit GetLeftUnit(Unit.UnitType type)
        {
            return lightUnits[(int)type];
        }

        public Unit GetRightUnit(Unit.UnitType type)
        {
            return darkUnits[(int)type];
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
