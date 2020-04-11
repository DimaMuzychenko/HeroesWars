using UnityEngine;
using Assets.GameLogic.CellClasses;

namespace Assets.GameLogic
{
    public class CellSelection
    {
        public static CellSelection instance;
        Cell cell;

        public static CellSelection GetInstance()
        {
            if(instance == null)
            {
                instance = new CellSelection();
            }
            return instance;
        }

        public void SelectCell(Cell cell)
        {
            if(this.cell != null)
                this.cell.OutlineCell(false);
            this.cell = cell;
            cell.OutlineCell(true);
        }

        public Cell GetSelectedCell()
        {
            return cell;
        }

        public void HideSelection()
        {
            if(this.cell != null)
            {
                cell.OutlineCell(false);
                this.cell = null;
            }
        }

        
    }
}
