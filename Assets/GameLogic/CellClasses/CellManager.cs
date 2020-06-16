using System.Collections.Generic;
using UnityEngine;


namespace Assets.GameLogic.CellClasses
{
    public class CellManager : MonoBehaviour
    {
        private List<Cell> cells;
        private List<Vector3> capturings;
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
            capturings = new List<Vector3>();
            instance = this;
            
        }

        private void Start()
        {
            GameEvents.GetInstance().OnPlayerChanged += DoCapturings;
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

        public int EnemyPortalCount()
        {
            if(PlayerControler.GetInstance().FirstPlayerTurn())
            {
                return DarkPortalCount();
            }
            else
            {
                return LightPortalCount();
            }
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
            Debug.Log("Dark portals count: " + cells.FindAll(cell => cell.type == Cell.CellType.DarkPortal).Count.ToString());
            return cells.FindAll(cell => cell.type == Cell.CellType.DarkPortal).Count;
        }

        public void CapturePortal(Vector3 portalPosition)
        {
            capturings.Add(portalPosition);
            
        }

        private void DoCapturings()
        {
            Debug.Log("Capturings: " + capturings.Count.ToString());
            Debug.Log("Friendly units: " + UnitsList.GetInstance().GetAllFriends().Length);
            if(capturings.Count == 0)
            {
                return;
            }
            if(UnitsList.GetInstance().GetAllFriends().Length == 0)
            {
                return;
            }
            foreach(Vector3 portalPos in capturings)
            {
                foreach(Unit unit in UnitsList.GetInstance().GetAllFriends())
                {
                    if(unit.transform.position == portalPos)
                    {
                        Cell newPortal;
                        Cell oldPortal = GetCell(portalPos);

                        if (PlayerControler.GetInstance().FirstPlayerTurn())
                        {
                            Debug.Log("!!!");
                            //newPortal = Instantiate(leftPortalPrefab, gameObject.transform);
                            newPortal = Instantiate(leftPortalPrefab, oldPortal.transform.position, GetCell(portalPos).transform.rotation, gameObject.transform);
                        }
                        else
                        {
                            //newPortal = Instantiate(rightPortalPrefab, gameObject.transform);
                            newPortal = Instantiate(rightPortalPrefab, oldPortal.transform.position, GetCell(portalPos).transform.rotation, gameObject.transform);
                        }                        
                        cells[cells.IndexOf(oldPortal)] = newPortal;
                        Destroy(oldPortal.gameObject);
                    }
                }
            }
            capturings.RemoveAll(pos => (int)PlayerControler.GetInstance().GetCurrentPlayer() == (int)GetCell(pos).type - 1);

            if (GameControler.WinCheck())
            {
                Debug.Log("Win!!!");
                GameEvents.GetInstance().ShowWinScreen();
            }
        }
    }
}
