using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.GameLogic.CellClasses;

namespace Assets.GameLogic
{
    public class Node
    {
        public Cell cell;
        public int g;
        public int h;
        public int f;
        public Node parent;
    }

    public class PathFinder
    {
        private static PathFinder instance;
        private CellManager cells;
        private UnitSelection selectedUnit;
        private UnitsList units;
        private PlayerControler playerControler;

        public PathFinder()
        {
            cells = CellManager.GetInstance();
            selectedUnit = UnitSelection.GetInstance();
            units = UnitsList.GetInstance();
            playerControler = PlayerControler.GetInstance();
        }

        public static PathFinder GetInstance()
        {
            if (instance == null)
                instance = new PathFinder();
            return instance;
        }
        
        private Cell[] GetPossibleCells()
        {
            var pCells = new List<Cell>();
            var unitPosition = selectedUnit.GetSelectedUnit().transform.position;
            var unitSpeed = selectedUnit.GetSelectedUnit().GetSpeed();
            foreach (Cell cell in cells.GetAllCells())
            {
                if(Vector3.Distance(unitPosition, cell.transform.position) <= unitSpeed+0.1f)
                {
                    //if(units.GetUnit(cell) == null)
                    //{
                        pCells.Add(cell);
                    //}
                }
            }
            return pCells.ToArray();
        }

        public int FindDistance(Vector3 a, Vector3 b)
        {
            var start = new Node { cell = cells.GetCell(a) };
            var end = new Node { cell = cells.GetCell(b) };
            var openSet = new List<Node>();
            openSet.Add(start);

            var current = new Node();

            while (openSet.Count != 0)
            {
                var lowestFScore = openSet.Min(node => node.f);
                current = openSet.Find(node => node.f == lowestFScore);

                if (current.cell.transform.position == end.cell.transform.position)
                {
                    break;
                }

                openSet.Remove(current);
                var neighbors = GetAllNeighborsOf(current);
                foreach (var neighbor in neighbors)
                {
                    var tentativeGScore = current.g + neighbor.h;
                    if (tentativeGScore <= neighbor.g)
                    {
                        neighbor.g = tentativeGScore;
                        neighbor.f = neighbor.g + (int)Math.Ceiling(d(neighbor, end));
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
            var path = new List<Cell>();
            while (current != null)
            {
                path.Insert(0, current.cell);
                current = current.parent;
            }
            Debug.Log("Distance: " + (path.Count - 1).ToString());
            return path.Count - 1;
        }

        public Cell[] FindPath(Vector3 targetPosition)
        {
            var start = new Node { cell = cells.GetCell(selectedUnit.GetSelectedUnit().transform.position) };
            var end = new Node { cell = cells.GetCell(targetPosition) };
            var openSet = new List<Node>();
            openSet.Add(start);

            var current = new Node();

            while(openSet.Count != 0)
            {
                var lowestFScore = openSet.Min(node => node.f);
                current = openSet.Find(node => node.f == lowestFScore);

                if (current.cell.transform.position == end.cell.transform.position)
                {
                    break;
                }

                openSet.Remove(current);
                var neighbors = GetNeighborsOf(current);
                foreach(var neighbor in neighbors)
                {
                    var tentativeGScore = current.g + neighbor.h;
                    if(tentativeGScore <= neighbor.g)
                    {
                        neighbor.g = tentativeGScore;
                        neighbor.f = neighbor.g + (int)Math.Ceiling(d(neighbor, end));
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
            var path = new List<Cell>();
            while(current != null)
            {
                path.Insert(0, current.cell);
                current = current.parent;
            }
            return path.ToArray();
        }

        private List<Node> GetNeighborsOf(Node node)
        {
            List<Node> neighbors = new List<Node>();
            foreach(Cell c in GetPossibleCells())
            {
                if (Vector3.Distance(node.cell.transform.position, c.transform.position) <= 1 && units.GetUnit(c.transform.position) == null)
                {
                    var neighbor = new Node { cell = c, parent = node, h = 1 };
                    neighbor.g = node.g + neighbor.h;
                    neighbors.Add(neighbor);
                }
            }
            return neighbors;
        }

        private List<Node> GetAllNeighborsOf(Node node)
        {
            List<Node> neighbors = new List<Node>();
            foreach (Cell c in GetPossibleCells())
            {
                if (Vector3.Distance(node.cell.transform.position, c.transform.position) <= 1)
                {
                    var neighbor = new Node { cell = c, parent = node, h = 1 };
                    neighbor.g = node.g + neighbor.h;
                    neighbors.Add(neighbor);
                }
            }
            return neighbors;
        }

        private float d(Node a, Node b)
        {
            return Vector3.Distance(a.cell.transform.position, b.cell.transform.position);
        }

        public List<Cell> GetAvailableCells()
        {
            var availableCells = new List<Cell>();
            Debug.Log("Possible cells : " + GetPossibleCells().Length);
            foreach(Cell cell in GetPossibleCells())
            {
                if(units.GetUnit(cell) == null)
                {
                    if (FindPath(cell.transform.position).Length <= selectedUnit.GetSelectedUnit().GetSpeed() + 1)
                    {
                        availableCells.Add(cell);
                    }
                }
            }
            return availableCells;
        }

        public bool CanRich(Cell cell)
        {
            if (Vector3.Distance(selectedUnit.GetSelectedUnit().transform.position, cell.transform.position) < selectedUnit.GetSelectedUnit().GetSpeed() + 0.1f)
                return FindPath(cell.transform.position).Length <= selectedUnit.GetSelectedUnit().GetSpeed() + 1;
            else
                return false;
        }

        public bool CanRich(Vector3 position)
        {
            if (Vector3.Distance(selectedUnit.GetSelectedUnit().transform.position, position) < selectedUnit.GetSelectedUnit().GetSpeed() + 0.1f)
                return FindPath(position).Length <= selectedUnit.GetSelectedUnit().GetSpeed() + 1;
            else
                return false;
        }

        public bool CanAttack(Cell cell)
        {
            if (Vector3.Distance(selectedUnit.GetSelectedUnit().transform.position, cell.transform.position) < selectedUnit.GetSelectedUnit().GetSpeed() + 0.1f)
                return FindPath(cell.transform.position).Length <= selectedUnit.GetSelectedUnit().GetRange() + 1;
            else
                return false;
        }

        public bool CanAttack(Vector3 position)
        {
            return FindDistance(selectedUnit.GetSelectedUnit().transform.position, position) <= selectedUnit.GetSelectedUnit().GetRange();
        }

        public bool CanAttack(Unit unit)
        {
            if (Vector3.Distance(selectedUnit.GetSelectedUnit().transform.position, unit.transform.position) < selectedUnit.GetSelectedUnit().GetSpeed() + 0.1f)
                return FindPath(unit.transform.position).Length <= selectedUnit.GetSelectedUnit().GetRange() + 1;
            else
                return false;
        }
    }
}
