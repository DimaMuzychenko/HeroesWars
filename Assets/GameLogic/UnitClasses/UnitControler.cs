using Assets.GameLogic.CellClasses;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameLogic
{
    public class UnitControler : MonoBehaviour
    {
        [SerializeField] UnitsList units;
        [SerializeField] UnitFactory unitFactory;
        [SerializeField] UnitSelection unitSelection;
        [SerializeField] CellSelection cellSelction;


        public void SpawnUnit(Unit.UnitType type)
        {
            Vector3 position = cellSelction.GetSelectedCell().transform.position;
            Unit newUnit = unitFactory.SpawnUnit(type, position);
            if (cellSelction.GetSelectedCell().type == Cell.CellType.LightPortal)
            {
                units.AddToLeft(newUnit);
            }
            else if (cellSelction.GetSelectedCell().type == Cell.CellType.DarkPortal)
            {
                units.AddToRight(newUnit);
            }
            unitSelection.SelectUnit(newUnit);
        }
                
        public void Move(Cell from, Cell to)
        {
            bool enemyWithinRange = false;
            foreach(Unit enemy in units.GetAllEnemies())
            {
                if (unitSelection.GetSelectedUnit().CanAttack(enemy.transform.position))
                {
                    enemyWithinRange = true;
                }
            }
            if(!enemyWithinRange)
            {
                unitSelection.GetSelectedUnit().Disactivate();
            }
            units.GetUnit(from).MoveTo(to.transform.position);
        }

        public void Attack(Vector3 targetPosition)
        {
            Debug.Log(unitSelection.GetSelectedUnit().GetAttackPower() + " points of damage were applied");
            units.GetUnit(targetPosition).ApplyDamage(unitSelection.GetSelectedUnit().GetAttackPower());
            unitSelection.GetSelectedUnit().Disactivate();
            unitSelection.GetSelectedUnit().outline.RemoveOutline();
        }

    }
}
