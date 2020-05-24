using Assets.GameLogic.CellClasses;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.GameLogic
{
    public class UnitsList
    {
        private List<Unit> lightUnits;
        private List<Unit> darkUnits;
        private static UnitsList instance;

        public UnitsList()
        {
            lightUnits = new List<Unit>();
            darkUnits = new List<Unit>();
        }


        public static UnitsList GetInstance()
        {
            if (instance == null)
            {
                instance = new UnitsList();
            }
            return instance;
        }

        public void AddToRight(Unit unit)
        {
            darkUnits.Add(unit);
        }

        public void AddToLeft(Unit unit)
        {
            lightUnits.Add(unit);
        }


        public bool ContainsInLeft(Cell cell)
        {
            for (int i = 0; i < lightUnits.Count; i++)
            {
                if (lightUnits[i] != null)
                {
                    if (lightUnits[i].transform.position == cell.transform.position)
                        return true;
                }
            }
            return false;
        }

        public bool ContainsInLeft(Vector3 position)
        {
            for (int i = 0; i < lightUnits.Count; i++)
            {
                if (lightUnits[i] != null)
                {
                    if (lightUnits[i].transform.position == position)
                        return true;
                }
            }
            return false;
        }

        public bool ContainsInRight(Cell cell)
        {
            for (int i = 0; i < darkUnits.Count; i++)
            {
                if (darkUnits[i] != null)
                {
                    if (darkUnits[i].transform.position == cell.transform.position)
                        return true;
                }
            }
            return false;
        }

        public bool ContainsInRight(Vector3 position)
        {
            for (int i = 0; i < darkUnits.Count; i++)
            {
                if (darkUnits[i] != null)
                {
                    if (darkUnits[i].transform.position == position)
                        return true;
                }
            }
            return false;
        }

        public int PlayerOf(Vector3 positionOfUnit)
        {
            if (ContainsInLeft(positionOfUnit))
                return 0;
            else if (ContainsInRight(positionOfUnit))
                return 1;
            else
                return -1;
        }


        public Unit GetUnit(Cell cell)
        {
            for (int i = 0; i < lightUnits.Count; i++)
            {
                if (lightUnits[i] != null)
                {
                    if (lightUnits[i].transform.position == cell.transform.position)
                    {
                        return lightUnits[i];
                    }
                }
            }
            for (int i = 0; i < darkUnits.Count; i++)
            {
                if (darkUnits[i] != null)
                {
                    if (darkUnits[i].transform.position == cell.transform.position)
                    {
                        return darkUnits[i];
                    }
                }
            }
            return null;
        }
        public Unit GetUnit(Vector3 position)
        {
            for (int i = 0; i < lightUnits.Count; i++)
            {
                if (lightUnits[i] != null)
                {
                    if (lightUnits[i].transform.position == position)
                    {
                        return lightUnits[i];
                    }
                }
            }
            for (int i = 0; i < darkUnits.Count; i++)
            {
                if (darkUnits[i] != null)
                {
                    if (darkUnits[i].transform.position == position)
                    {
                        return darkUnits[i];
                    }
                }
            }
            return null;
        }

        public void MakeActiveUnits()
        {
            if (PlayerControler.GetInstance().GetCurrentPlayer() == PlayerControler.Player.Light)
            {
                foreach (Unit unit in lightUnits)
                {
                    if (unit != null)
                    {
                        unit.Activate();
                        unit.outline.OutlineAsFriend();
                        unit.MakeFriendlyStatHUD();
                    }
                }
                foreach (Unit unit in darkUnits)
                {
                    if (unit != null)
                    {
                        unit.MakeEnemyStatHUD();
                    }
                }
            }

            if (PlayerControler.GetInstance().GetCurrentPlayer() == PlayerControler.Player.Dark)
            {
                foreach (Unit unit in darkUnits)
                {
                    if (unit != null)
                    {
                        unit.Activate();
                        unit.outline.OutlineAsFriend();
                        unit.MakeFriendlyStatHUD();
                    }
                }
                foreach (Unit unit in lightUnits)
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
            if (PlayerControler.GetInstance().GetCurrentPlayer() == PlayerControler.Player.Light)
            {
                darkUnits.RemoveAll(unit => unit == null);
                return darkUnits.ToArray();
            }
            else
            {
                lightUnits.RemoveAll(unit => unit == null);
                return lightUnits.ToArray();
            }
        }

        public Unit[] GetAllFriends()
        {
            if (PlayerControler.GetInstance().GetCurrentPlayer() == PlayerControler.Player.Light)
            {
                lightUnits.RemoveAll(unit => unit == null);
                return lightUnits.ToArray();
            }
            else
            {
                darkUnits.RemoveAll(unit => unit == null);
                return darkUnits.ToArray();
            }
        }

        public Unit[] GetAllUnits()
        {
            var allUnits = new List<Unit>();
            allUnits.AddRange(GetAllFriends());
            allUnits.AddRange(GetAllEnemies());
            return allUnits.ToArray();
        }

        public void HideDamage()
        { 
            foreach(Unit unit in GetAllUnits())
            {
                unit.HideDamage();
            }
        }
    }
}
