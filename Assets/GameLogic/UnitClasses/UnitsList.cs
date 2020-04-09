using Assets.GameLogic.CellClasses;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.GameLogic
{
    public class UnitsList : MonoBehaviour
    {
        List<Unit> leftUnits;
        List<Unit> rightUnits;

        private void Awake()
        {
            leftUnits = new List<Unit>();
            rightUnits = new List<Unit>();
        }

        public void AddToRight(Unit unit)
        {
            unit.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            rightUnits.Add(unit);
        }

        public void AddToLeft(Unit unit)
        {
            leftUnits.Add(unit);
        }


        public bool ContainsInLeft(Cell cell)
        {
            for (int i = 0; i < leftUnits.Count; i++)
            {
                if (leftUnits[i] != null)
                {
                    if (leftUnits[i].transform.position == cell.transform.position)
                        return true;
                }
            }
            return false;
        }

        public bool ContainsInLeft(Vector3 position)
        {
            for (int i = 0; i < leftUnits.Count; i++)
            {
                if (leftUnits[i] != null)
                {
                    if (leftUnits[i].transform.position == position)
                        return true;
                }
            }
            return false;
        }

        public bool ContainsInRight(Cell cell)
        {
            for (int i = 0; i < rightUnits.Count; i++)
            {
                if (rightUnits[i] != null)
                {
                    if (rightUnits[i].transform.position == cell.transform.position)
                        return true;
                }
            }
            return false;
        }

        public bool ContainsInRight(Vector3 position)
        {
            for (int i = 0; i < rightUnits.Count; i++)
            {
                if (rightUnits[i] != null)
                {
                    if (rightUnits[i].transform.position == position)
                        return true;
                }
            }
            return false;
        }

        public int PlayerOf(Vector3 positionOfUnit)
        {
            if (ContainsInLeft(positionOfUnit))
                return 1;
            else if (ContainsInRight(positionOfUnit))
                return 2;
            else
                return 0;
        }


        public Unit GetUnit(Cell cell)
        {
            for (int i = 0; i < leftUnits.Count; i++)
            {
                if (leftUnits[i] != null)
                {
                    if (leftUnits[i].transform.position == cell.transform.position)
                    {
                        return leftUnits[i];
                    }
                }
            }
            for (int i = 0; i < rightUnits.Count; i++)
            {
                if (rightUnits[i] != null)
                {
                    if (rightUnits[i].transform.position == cell.transform.position)
                    {
                        return rightUnits[i];
                    }
                }
            }
            return null;
        }
        public Unit GetUnit(Vector3 position)
        {
            for (int i = 0; i < leftUnits.Count; i++)
            {
                if (leftUnits[i] != null)
                {
                    if (leftUnits[i].transform.position == position)
                    {
                        return leftUnits[i];
                    }
                }
            }
            for (int i = 0; i < rightUnits.Count; i++)
            {
                if (rightUnits[i] != null)
                {
                    if (rightUnits[i].transform.position == position)
                    {
                        return rightUnits[i];
                    }
                }
            }
            return null;
        }
        
        public void MakeActiveUnits()
        {
            if(TurnCounter.GetInstance().GetCurrentPlayer() == TurnCounter.Player.Left)
            {
                foreach (Unit unit in leftUnits)
                {
                    if (unit != null)
                    {
                        unit.Activate();
                        unit.outline.OutlineAsFriend();
                        unit.MakeFriendlyStatHUD();
                    }
                }
                foreach(Unit unit in rightUnits)
                {
                    if (unit != null)
                    {
                        unit.MakeEnemyStatHUD();
                    }
                }
            }

            if (TurnCounter.GetInstance().GetCurrentPlayer() == TurnCounter.Player.Right)
            {
                foreach (Unit unit in rightUnits)
                {
                    if (unit != null)
                    {
                        unit.Activate();
                        unit.outline.OutlineAsFriend();
                        unit.MakeFriendlyStatHUD();
                    }
                }
                foreach (Unit unit in leftUnits)
                {
                    if (unit != null)
                    {
                        unit.MakeEnemyStatHUD();
                    }
                }
            }
        }

        public Unit[] GetAllEnemies()
        {
            if(TurnCounter.GetInstance().GetCurrentPlayer() == TurnCounter.Player.Left)
            {
                rightUnits.RemoveAll(unit => unit == null);
                return rightUnits.ToArray();
            }
            else
            {
                leftUnits.RemoveAll(unit => unit == null);
                return leftUnits.ToArray();
            }
        }

        public Unit[] GetAllFriends()
        {
            if (TurnCounter.GetInstance().GetCurrentPlayer() == TurnCounter.Player.Left)
            {
                leftUnits.RemoveAll(unit => unit == null);
                return leftUnits.ToArray();
            }
            else
            {                
                rightUnits.RemoveAll(unit => unit == null);
                return rightUnits.ToArray();
            }
        }
    }
}
