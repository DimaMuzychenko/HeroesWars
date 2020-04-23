using Assets.GameLogic.CellClasses;
using UnityEngine;
using System;

namespace Assets.GameLogic
{
    class ActionDefiner : MonoBehaviour
    {
        private UnitSelection unitSelection;        
        [SerializeField] private CellManager cellManager;
        private UnitsList unitsList;
        public enum Action
        {
            ShowActions, Attack, SelectCell, SelectFriend, SelectEnemy, Move, Spawn, Capture, Cancel, Ignore
        }
        public enum Target
        {
            FriendUnit, EnemyUnit, Cell, FriendPortal, EnemyPortal
        }

        private void Awake()
        {
            unitsList = UnitsList.GetInstance();
            unitSelection = UnitSelection.GetInstance();
        }

        public Action DefineAction(Vector3 targetPosition)
        {
            switch(DefineTarget(targetPosition))
            {
                case Target.Cell: 
                    return CellAction(targetPosition);

                case Target.EnemyPortal: 
                    return EnemyPortalAction(targetPosition);

                case Target.EnemyUnit:
                    return EnemyUnitAction(targetPosition);

                case Target.FriendPortal:
                    return FriendPortalAction(targetPosition);

                case Target.FriendUnit:
                    return FriendUnitAction(targetPosition);

                default:
                    Debug.LogError("DefineAction error");
                    return Action.Cancel;
            }
        }

        private Target DefineTarget(Vector3 targetPosition)
        {
            var currentPlayer = PlayerControler.GetInstance().GetCurrentPlayer();

            if (unitsList.ContainsInLeft(targetPosition))
            {
                if (currentPlayer == PlayerControler.Player.Left) return Target.FriendUnit;

                if (currentPlayer == PlayerControler.Player.Right) return Target.EnemyUnit;
            }
            else if (unitsList.ContainsInRight(targetPosition))
            {
                if (currentPlayer == PlayerControler.Player.Left) return Target.EnemyUnit;

                if (currentPlayer == PlayerControler.Player.Right) return Target.FriendUnit;
            }
            else if (cellManager.GetCell(targetPosition).type == Cell.CellType.LightPortal)
            {
                if (currentPlayer == PlayerControler.Player.Left) return Target.FriendPortal;

                if (currentPlayer == PlayerControler.Player.Right) return Target.EnemyPortal;
            }
            else if (cellManager.GetCell(targetPosition).type == Cell.CellType.DarkPortal)
            {
                if (currentPlayer == PlayerControler.Player.Left) return Target.EnemyPortal;

                if (currentPlayer == PlayerControler.Player.Right) return Target.FriendPortal;
            }
            return Target.Cell;            
        }

        private Action CellAction(Vector3 targetPosition)
        {
            if(unitSelection.GetSelectedUnit() == null)
            {
                return Action.SelectCell;
            }
            else
            {
                if (unitSelection.GetSelectedUnit().state == Unit.UnitState.Waiting)
                {
                    if(unitSelection.GetSelectedUnit().CanRich(targetPosition))
                    {
                        return Action.Move;
                    }
                    else
                    {
                        return Action.Cancel;
                    }
                }
                else
                {
                    return Action.SelectCell;
                }
            }
        }

        private Action FriendUnitAction(Vector3 targetPosition)
        {
            if (unitSelection.GetSelectedUnit() == null)
            {
                return Action.SelectFriend;
            }
            else
            {
                if (unitSelection.GetSelectedUnit().state == Unit.UnitState.Waiting)
                {
                    if(unitSelection.GetSelectedUnit().transform.position == targetPosition)
                    {
                        return Action.Cancel;
                    }
                    else
                    {
                        return Action.SelectFriend;
                    }
                }
                else
                {
                    if(unitSelection.GetSelectedUnit().transform.position == targetPosition)
                    {
                        return Action.ShowActions;
                    }
                    else
                    {
                        return Action.SelectFriend;
                    }
                }
            }
        }

        private Action EnemyUnitAction(Vector3 targetPosition)
        {
            if(unitSelection.GetSelectedUnit() == null)
            {
                return Action.SelectEnemy;
            }
            else
            {
                if (unitSelection.GetSelectedUnit().state == Unit.UnitState.Waiting)
                {
                    if (unitSelection.GetSelectedUnit().CanAttack(targetPosition))
                    {
                        return Action.Attack;
                    }
                    else
                    {
                        return Action.Cancel;
                    }
                }
                else
                {
                    return Action.SelectEnemy;
                }
            }
        }

        private Action FriendPortalAction(Vector3 targetPosition)
        {
            if (unitSelection.GetSelectedUnit() == null)
            {
                return Action.Spawn;
            }
            else
            {
                if (unitSelection.GetSelectedUnit().state == Unit.UnitState.Waiting)
                {
                    if (unitSelection.GetSelectedUnit().CanRich(targetPosition) && !unitSelection.GetSelectedUnit().WasMoved())
                    {
                        return Action.Move;
                    }
                    else
                    {
                        return Action.Cancel;
                    }
                }
                else
                {
                    return Action.Spawn;
                }
            }
            
        }

        private Action EnemyPortalAction(Vector3 targetPosition)
        {
            if (unitSelection.GetSelectedUnit() == null)
            {
                return Action.SelectCell;
            }
            else
            {
                if (unitSelection.GetSelectedUnit().state == Unit.UnitState.Waiting)
                {
                    if (unitSelection.GetSelectedUnit().CanRich(targetPosition))
                    {
                        return Action.Move;
                    }
                    else
                    {
                        return Action.Cancel;
                    }
                }
                else
                {
                    return Action.SelectCell;
                }
            }   
        }

        //private bool CanAttack(Vector3 targetPosition)
        //{
        //    if (Math.Sqrt(Math.Pow(targetPosition.x - cellSelection.GetSelectedCell().transform.position.x, 2) +
        //        Math.Pow(targetPosition.y - cellSelection.GetSelectedCell().transform.position.y, 2)) <=
        //        unitsList.GetUnit(cellSelection.GetSelectedCell()).GetRange())
        //        return true;
        //    else
        //        return false;
        //}
        public void HideActions()
        {
            foreach (Cell cell in cellManager.GetAllCells())
            {
                if (cell != null)
                {
                    cell.GetComponent<Renderer>().material.color = Color.white;
                }
            }
            if (unitsList.GetAllEnemies().Length > 0)
            {
                foreach (Unit unit in unitsList.GetAllEnemies())
                {
                    unit.outline.RemoveOutline();
                    unit.HideDamage();
                }
            }
        }

        //public void ShowActions()
        //{
        //    if (IsActionShown)
        //    {
        //        HideActions();
        //    }
        //    if(unitSelection.GetSelectedUnit().IsActive())
        //        IsActionShown = true;
        //    foreach (Cell cell in cellManager.GetAllCells())
        //    {
        //        if (cell != null)
        //        {
        //            if (!CanRich(cell) && !unitSelection.GetSelectedUnit().WasMoved())
        //            {
        //                cell.GetComponent<Renderer>().material.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
        //            }
        //        }
        //    }
        //    if (unitsList.GetAllEnemies().Length > 0)
        //    {
        //        foreach (Unit unit in unitsList.GetAllEnemies())
        //        {
        //            if (CanAttack(unit.transform.position))
        //            {
        //                unit.outline.OutlineAsEnemy();
        //                unit.ShowDamage(unitSelection.GetSelectedUnit().GetAttackPower());
        //                cellManager.GetCell(unit.transform.position).GetComponent<Renderer>().material.color = Color.white;
        //            }
        //        }
        //    }
        //    if (TurnCounter.GetInstance().FirstPlayerTurn() && cellManager.GetCell(unitSelection.GetSelectedUnit().transform.position).type == Cell.CellType.DarkPortal || cellManager.GetCell(unitSelection.GetSelectedUnit().transform.position).type == Cell.CellType.Portal)
        //    {
        //        if (!gameUI.IsButtonSwiched())
        //        {
        //            gameUI.SwitchMainButton();
        //        }
        //    }
        //    if (!TurnCounter.GetInstance().FirstPlayerTurn() && cellManager.GetCell(unitSelection.GetSelectedUnit().transform.position).type == Cell.CellType.LightPortal || cellManager.GetCell(unitSelection.GetSelectedUnit().transform.position).type == Cell.CellType.Portal)
        //    {
        //        if (!gameUI.IsButtonSwiched())
        //        {
        //            gameUI.SwitchMainButton();
        //        }
        //    }
        //}
        //private bool CanRich(Cell cell)
        //{
        //    if (Math.Sqrt(Math.Pow(cell.transform.position.x - cellSelection.GetSelectedCell().transform.position.x, 2) +
        //        Math.Pow(cell.transform.position.y - cellSelection.GetSelectedCell().transform.position.y, 2)) <=
        //        unitsList.GetUnit(cellSelection.GetSelectedCell()).GetSpeed())
        //        return true;
        //    else
        //        return false;
        //}
        //private bool CanRich(Vector3 destination)
        //{
        //    if (Math.Sqrt(Math.Pow(destination.x - cellSelection.GetSelectedCell().transform.position.x, 2) +
        //        Math.Pow(destination.y - cellSelection.GetSelectedCell().transform.position.y, 2)) <=
        //        unitsList.GetUnit(cellSelection.GetSelectedCell()).GetSpeed())
        //        return true;
        //    else
        //        return false;
        //}
    }
}
