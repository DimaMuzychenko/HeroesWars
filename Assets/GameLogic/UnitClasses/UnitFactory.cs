using UnityEngine;
using Assets.GameLogic.UnitClasses;

namespace Assets.GameLogic
{
    [CreateAssetMenu]
    public class UnitFactory : ScriptableObject
    {
        [SerializeField] private PrefabsList prefabsList;
        public Unit SpawnUnit(string name, Vector3 position)
        {
            Unit instance;
            if (PlayerControler.GetInstance().FirstPlayerTurn())
            {
                instance = Instantiate(prefabsList.GetLeftUnit(name));
            }
            else
            {
                instance = Instantiate(prefabsList.GetRightUnit(name));
            }
            instance.transform.position = position;
            return instance;
        }
    }
}
