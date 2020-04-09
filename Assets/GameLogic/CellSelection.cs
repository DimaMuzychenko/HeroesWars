using UnityEngine;
using Assets.GameLogic.CellClasses;

namespace Assets.GameLogic
{
    public class CellSelection : MonoBehaviour
    {
        Cell cell;

        public void SelectCell(Cell cell)
        {
            this.cell = cell;            
            gameObject.transform.position = cell.transform.position;
            gameObject.SetActive(true);
        }

        public Cell GetSelectedCell()
        {
            return cell;
        }

        public void HideSelection()
        {
            this.cell = null;
            gameObject.SetActive(false);
        }
    }
}
