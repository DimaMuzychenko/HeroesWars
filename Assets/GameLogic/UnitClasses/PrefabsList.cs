using UnityEngine;

namespace Assets.GameLogic.UnitClasses
{
    [CreateAssetMenu]
    class PrefabsList : ScriptableObject
    {
        [SerializeField] private Unit[] leftUnits;
        [SerializeField] private Unit[] rightUnits;

        public Unit GetLeftUnit(Unit.UnitType type)
        {
            return leftUnits[(int)type];
        }

        public Unit GetRightUnit(Unit.UnitType type)
        {
            return rightUnits[(int)type];
        }

        public Unit[] GetAllLeftUnits()
        {
            return leftUnits;
        }

        public Unit[] GetAllRightUnits()
        {
            return rightUnits;
        }
    }
}
