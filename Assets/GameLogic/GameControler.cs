using Assets.GameLogic.CellClasses;
using System;
using UnityEngine;

namespace Assets.GameLogic
{
    class GameControler : MonoBehaviour
    {
        [SerializeField] SpawnMenu spawnMenu;
        [SerializeField] UnitControler unitControler;
        [SerializeField] ActionDefiner actionDefiner;
        [SerializeField] CellSelection cellSelection;
        [SerializeField] UnitSelection unitSelection;
        [SerializeField] UnitsList unitsList;
        [SerializeField] CellManager cellManager;

        private void Start()
        {
            GameEvents.GetInstance().OnCellClicked += DoAction;            
            GameEvents.GetInstance().OnPlayerChanged += RefreshField;
            GameEvents.GetInstance().OnCaptureButtonPressed += CapturePortal;
        }

        private void DoAction(Vector3 targetPosition)
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
                    if(unitSelection.GetSelectedUnit().IsActive())
                    {
                        actionDefiner.ShowActions();
                    }
                    break;

                case ActionDefiner.Action.SelectEnemy:
                    cellSelection.SelectCell(cellManager.GetCell(targetPosition));
                    unitSelection.SelectUnit(unitsList.GetUnit(targetPosition));
                    break;

                case ActionDefiner.Action.Attack:
                    actionDefiner.HideActions();
                    unitControler.Attack(targetPosition);
                    break;

                case ActionDefiner.Action.ShowActions:
                    actionDefiner.ShowActions();
                    break;

                case ActionDefiner.Action.Spawn:
                    cellSelection.SelectCell(cellManager.GetCell(targetPosition));
                    spawnMenu.Open();
                    break;

                case ActionDefiner.Action.Cancel:
                    actionDefiner.HideActions();
                    break;

                case ActionDefiner.Action.Move:
                    unitControler.Move(cellSelection.GetSelectedCell(), cellManager.GetCell(targetPosition));
                    cellSelection.SelectCell(cellManager.GetCell(targetPosition));
                    actionDefiner.ShowActions();
                    break;

                case ActionDefiner.Action.Ignore:
                    break;
            }
        }

        private void RefreshField()
        {
            actionDefiner.HideActions();
            cellSelection.HideSelection();
            unitSelection.DeselectUnit();
            unitsList.MakeActiveUnits();
        }

        private void CapturePortal()
        {
            cellManager.CapturePortal(unitSelection.GetSelectedUnit().transform.position);
            cellSelection.SelectCell(cellManager.GetCell(unitSelection.GetSelectedUnit().transform.position));
            unitSelection.GetSelectedUnit().Disactivate();
            actionDefiner.HideActions();
            if(WinCheck())
            {
                GameEvents.GetInstance().ShowWinScreen();
            }
        }

        private bool WinCheck()
        {
            if(cellManager.LeftPortalCount() == 0 || cellManager.RightPortalCount() == 0)
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
