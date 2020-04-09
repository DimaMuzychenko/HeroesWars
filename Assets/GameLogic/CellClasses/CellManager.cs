using System.Collections.Generic;
using UnityEngine;


namespace Assets.GameLogic.CellClasses
{
    public class CellManager : MonoBehaviour
    {
        private List<Cell> cells;
        private static CellManager instance;
        [SerializeField] Cell leftPortalPrefab;
        [SerializeField] Cell rightPortalPrefab;

        private void Awake()
        {
            cells = new List<Cell>();
            foreach (Transform cell in gameObject.transform)
            {
                cells.Add(cell.GetComponent<Cell>());
            }
            instance = this;
        }

        public static CellManager GetInstance()
        {
            return instance;
        }

        public Cell GetCell(Vector3 targetPosition)
        {
            return cells.Find(cell => cell.transform.position == targetPosition);
        }

        public bool Exists(Vector3 targetPosition)
        {
            return cells.Exists(cell => cell.transform.position == targetPosition);
        }

        public Cell[] GetAllCells()
        {
            return cells.ToArray();
        }

        public int LeftPortalCount()
        {
            int lPortalsCount = 0;
            foreach(Cell cell in cells)
            {
                if(cell.type == Cell.CellType.LPortal)
                {
                    lPortalsCount++;
                }
            }
            return lPortalsCount;
        }
        public int RightPortalCount()
        {
            return cells.FindAll(cell => cell.type == Cell.CellType.RPortal).Count;
        }

        public void CapturePortal(Vector3 portalPosition)
        {

            Cell newPortal;

            if (TurnCounter.GetInstance().FirstPlayerTurn())
            {                
                newPortal = Instantiate(leftPortalPrefab, gameObject.transform);
            }
            else
            {
                newPortal = Instantiate(rightPortalPrefab, gameObject.transform);
            }

            newPortal.gameObject.transform.position = portalPosition;
            Cell oldPortal = GetCell(portalPosition);
            cells[cells.IndexOf(oldPortal)] = newPortal;
            Destroy(oldPortal.gameObject);
        }
    }
}
