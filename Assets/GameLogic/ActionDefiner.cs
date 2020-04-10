using Assets.GameLogic.CellClasses;
using UnityEngine;
using System;

namespace Assets.GameLogic
{
    class ActionDefiner : MonoBehaviour
    {
        [SerializeField] private CellSelection cellSelection;
        [SerializeField] private UnitSelection unitSelection;
        [SerializeField] private UnitsList unitsList;
        [SerializeField] private CellManager cellManager;
        [SerializeField] private GameUI gameUI;
        private bool IsActionShown;
        public enum Action
        {
            ShowActions, Attack, SelectCell, SelectFriend, SelectEnemy, Move, Spawn, Capture, Cancel, Ignore
        }
        public enum Target
        {
            FriendUnit, EnemyUnit, Cell, FriendPortal, EnemyPortal
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
            var currentPlayer = TurnCounter.GetInstance().GetCurrentPlayer();

            if (unitsList.ContainsInLeft(targetPosition))
            {
                if (currentPlayer == TurnCounter.Player.Left) return Target.FriendUnit;

                if (currentPlayer == TurnCounter.Player.Right) return Target.EnemyUnit;
            }
            else if (unitsList.ContainsInRight(targetPosition))
            {
                if (currentPlayer == TurnCounter.Player.Left) return Target.EnemyUnit;

                if (currentPlayer == TurnCounter.Player.Right) return Target.FriendUnit;
            }
            else if (cellManager.GetCell(targetPosition).type == Cell.CellType.LightPortal)
            {
                if (currentPlayer == TurnCounter.Player.Left) return Target.FriendPortal;

                if (currentPlayer == TurnCounter.Player.Right) return Target.EnemyPortal;
            }
            else if (cellManager.GetCell(targetPosition).type == Cell.CellType.DarkPortal)
            {
                if (currentPlayer == TurnCounter.Player.Left) return Target.EnemyPortal;

                if (currentPlayer == TurnCounter.Player.Right) return Target.FriendPortal;
            }
            return Target.Cell;            
        }

        private Action CellAction(Vector3 targetPosition)
        {
            if (IsActionShown)
            {
                if(!unitSelection.GetSelectedUnit().WasMoved())
                {
                    if (CanRich(targetPosition))
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
                    return Action.Ignore;
                }
                
            }
            else
                return Action.SelectCell;
        }

        private Action FriendUnitAction(Vector3 targetPosition)
        {
            if (IsActionShown)
            {
                if(CanRich(targetPosition) && unitSelection.GetSelectedUnit().transform.position != targetPosition)
                {
                    return Action.Ignore;
                }
                else
                {
                    return Action.Cancel;
                }
            }
            else
            {
                if(unitSelection.GetSelectedUnit() == null)
                {
                    return Action.SelectFriend;
                }
                else if (unitSelection.GetSelectedUnit().transform.position == targetPosition && unitSelection.GetSelectedUnit().IsActive())
                {
                    return Action.ShowActions;
                }
                else
                {
                    return Action.SelectFriend;
                }
            }
        }

        private Action EnemyUnitAction(Vector3 targetPosition)
        {
            if (IsActionShown)
            {
                if (CanAttack(targetPosition))
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

        private Action FriendPortalAction(Vector3 targetPosition)
        {
            if (IsActionShown)
            {
                if (CanRich(targetPosition) && !unitSelection.GetSelectedUnit().WasMoved())
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

        private Action EnemyPortalAction(Vector3 targetPosition)
        {
            if (IsActionShown)
            {
                if(CanRich(targetPosition))
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

        private bool CanAttack(Vector3 targetPosition)
        {
            if (Math.Sqrt(Math.Pow(targetPosition.x - cellSelection.GetSelectedCell().transform.position.x, 2) +
                Math.Pow(targetPosition.y - cellSelection.GetSelectedCell().transform.position.y, 2)) <=
                unitsList.GetUnit(cellSelection.GetSelectedCell()).GetRange())
                return true;
            else
                return false;
        }
        public void HideActions()
        {
            IsActionShown = false;
            bool CurrentPlayerLeft = TurnCounter.GetInstance().GetCurrentPlayer() == TurnCounter.Player.Left ? true : false;
            foreach (Cell cell in cellManager.GetAllCells())
            {
                if (cell != null)
                {
                    cell.GetComponent<Renderer>().material.color = Color.white;
                }                
            }
            if(unitsList.GetAllEnemies().Length > 0)
            {
                foreach (Unit unit in unitsList.GetAllEnemies())
                {
                    unit.outline.RemoveOutline();
                    unit.HideDamage();
                }
            }
            if(gameUI.IsButtonSwiched())
            {
                gameUI.SwitchMainButton();
            }
        }

        public void ShowActions()
        {
            if (IsActionShown)
            {
                HideActions();
            }
            IsActionShown = true;
            foreach (Cell cell in cellManager.GetAllCells())
            {
                if (cell != null)
                {
                    if (!CanRich(cell) && !unitSelection.GetSelectedUnit().WasMoved())
                    {
                        cell.GetComponent<Renderer>().material.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
                    }                    
                }
            }
            if(unitsList.GetAllEnemies().Length > 0)
            {
                foreach (Unit unit in unitsList.GetAllEnemies())
                {
                    if (CanAttack(unit.transform.position))
                    {
                        unit.outline.OutlineAsEnemy();
                        unit.ShowDamage(unitSelection.GetSelectedUnit().GetAttackPower());
                        cellManager.GetCell(unit.transform.position).GetComponent<Renderer>().material.color = Color.white;
                    }
                }
            }
            if(TurnCounter.GetInstance().FirstPlayerTurn() && cellManager.GetCell(unitSelection.GetSelectedUnit().transform.position).type == Cell.CellType.DarkPortal)
            {
                if(!gameUI.IsButtonSwiched())
                {
                    gameUI.SwitchMainButton();
                }
            }
            if (!TurnCounter.GetInstance().FirstPlayerTurn() && cellManager.GetCell(unitSelection.GetSelectedUnit().transform.position).type == Cell.CellType.LightPortal)
            {
                if (!gameUI.IsButtonSwiched())
                {
                    gameUI.SwitchMainButton();
                }
            }
        }
        private bool CanRich(Cell cell)
        {
            if (Math.Sqrt(Math.Pow(cell.transform.position.x - cellSelection.GetSelectedCell().transform.position.x, 2) +
                Math.Pow(cell.transform.position.y - cellSelection.GetSelectedCell().transform.position.y, 2)) <=
                unitsList.GetUnit(cellSelection.GetSelectedCell()).GetSpeed())
                return true;
            else
                return false;
        }
        private bool CanRich(Vector3 destination)
        {
            if (Math.Sqrt(Math.Pow(destination.x - cellSelection.GetSelectedCell().transform.position.x, 2) +
                Math.Pow(destination.y - cellSelection.GetSelectedCell().transform.position.y, 2)) <=
                unitsList.GetUnit(cellSelection.GetSelectedCell()).GetSpeed())
                return true;
            else
                return false;
        }
    }
}
