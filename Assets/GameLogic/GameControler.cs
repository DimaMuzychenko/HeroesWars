using Assets.GameLogic.CellClasses;
using System;
using UnityEngine;

namespace Assets.GameLogic
{
    class GameControler : MonoBehaviour
    {
        [SerializeField] SpawnMenu spawnMenu;
        [SerializeField] ActionDefiner actionDefiner;
        [SerializeField] CellManager cellManager;
        UnitSelection unitSelection;
        CellSelection cellSelection;
        UnitsList unitsList;

        private void Awake()
        {
            cellSelection = CellSelection.GetInstance();
            unitsList = UnitsList.GetInstance();
            unitSelection = UnitSelection.GetInstance();
        }
        private void Start()
        {
            GameEvents.GetInstance().OnCellClicked += DoAction;            
            GameEvents.GetInstance().OnPlayerChanged += RefreshField;
            GameEvents.GetInstance().OnCaptureButtonPressed += CapturePortal;
        }

        private void DoAction(Vector3 targetPosition)
        {
            if(cellManager.GetCell(targetPosition) != null)
            {
                Debug.Log(actionDefiner.DefineAction(targetPosition));
                switch (actionDefiner.DefineAction(targetPosition))
                {
                    case ActionDefiner.Action.SelectCell:
                        cellSelection.SelectCell(cellManager.GetCell(targetPosition));
                        unitSelection.DeselectUnit();
                        break;

                    case ActionDefiner.Action.SelectFriend:
                        cellSelection.SelectCell(cellManager.GetCell(targetPosition));
                        unitSelection.SelectUnit(unitsList.GetUnit(targetPosition));
                        unitSelection.GetSelectedUnit().ShowActions();
                        break;

                    case ActionDefiner.Action.SelectEnemy:
                        cellSelection.SelectCell(cellManager.GetCell(targetPosition));
                        unitSelection.SelectUnit(unitsList.GetUnit(targetPosition));
                        break;

                    case ActionDefiner.Action.Attack:
                        unitSelection.GetSelectedUnit().HideActions();
                        unitSelection.GetSelectedUnit().Attack(targetPosition);
                        break;

                    case ActionDefiner.Action.ShowActions:
                        cellSelection.SelectCell(cellManager.GetCell(targetPosition));
                        unitSelection.GetSelectedUnit().ShowActions();
                        break;

                    case ActionDefiner.Action.Spawn:
                        cellSelection.SelectCell(cellManager.GetCell(targetPosition));
                        spawnMenu.Open();
                        break;

                    case ActionDefiner.Action.Cancel:
                        unitSelection.GetSelectedUnit().HideActions();
                        break;

                    case ActionDefiner.Action.Move:
                        cellSelection.SelectCell(cellManager.GetCell(targetPosition));
                        unitSelection.GetSelectedUnit().MoveTo(targetPosition);
                        break;

                    case ActionDefiner.Action.Ignore:
                        break;
                }
            }
        }

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

        private void RefreshField()
        {
            cellSelection.HideSelection();
            unitSelection.DeselectUnit();
            unitsList.MakeActiveUnits();
            unitsList.HideDamage();
        }

        private void CapturePortal()
        {
            cellManager.CapturePortal(unitSelection.GetSelectedUnit().transform.position);
            cellSelection.SelectCell(cellManager.GetCell(unitSelection.GetSelectedUnit().transform.position));
            unitSelection.GetSelectedUnit().Disactivate();
            unitSelection.GetSelectedUnit().HideActions();
            if(WinCheck())
            {
                GameEvents.GetInstance().ShowWinScreen();
            }
        }

        private bool WinCheck()
        {
            if(cellManager.LightPortalCount() == 0 || cellManager.DarkPortalCount() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
                
    }
}
