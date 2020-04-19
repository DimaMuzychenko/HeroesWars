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
        CellSelection cellSelction;

        private void Awake()
        {
            cellSelction = CellSelection.GetInstance();
        }

        public void SpawnUnit(string name)
        {
            Vector3 position = cellSelction.GetSelectedCell().transform.position;
            Unit newUnit = unitFactory.SpawnUnit(name, position);
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
            units.GetUnit(from).MoveTo(to.transform.position);
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
