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

        public int LightPortalCount()
        {
            int lPortalsCount = 0;
            foreach(Cell cell in cells)
            {
                if(cell.type == Cell.CellType.LightPortal)
                {
                    lPortalsCount++;
                }
            }
            return lPortalsCount;
        }
        public int DarkPortalCount()
        {
            return cells.FindAll(cell => cell.type == Cell.CellType.DarkPortal).Count;
        }

        public void CapturePortal(Vector3 portalPosition)
        {

            Cell newPortal;
            Cell oldPortal = GetCell(portalPosition);

            if (PlayerControler.GetInstance().FirstPlayerTurn())
            {                
                //newPortal = Instantiate(leftPortalPrefab, gameObject.transform);
                newPortal = Instantiate(leftPortalPrefab, oldPortal.transform.position, GetCell(portalPosition).transform.rotation, gameObject.transform);
            }
            else
            {
                //newPortal = Instantiate(rightPortalPrefab, gameObject.transform);
                newPortal = Instantiate(rightPortalPrefab, oldPortal.transform.position, GetCell(portalPosition).transform.rotation, gameObject.transform);
            }          
            cells[cells.IndexOf(oldPortal)] = newPortal;
            Destroy(oldPortal.gameObject);
        }
    }
}
