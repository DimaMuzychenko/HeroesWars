using UnityEngine;
using Assets.GameLogic.UnitClasses;

namespace Assets.GameLogic
{
    [CreateAssetMenu]
    public class UnitFactory : ScriptableObject
    {
        [SerializeField] private PrefabsList prefabsList;
        public Unit SpawnUnit(Unit.UnitType type, Vector3 position)
        {
            Unit instance;
            if (TurnCounter.GetInstance().FirstPlayerTurn())
            {
                instance = Instantiate(prefabsList.GetLeftUnit(type));
            }
            else
            {
                instance = Instantiate(prefabsList.GetRightUnit(type));
            }
            instance.transform.position = position;
            return instance;
        }
    }
}
