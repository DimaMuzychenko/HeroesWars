using System;
using UnityEngine;
using System.Collections;
using TMPro;
using Assets.GameLogic.CellClasses;

namespace Assets.GameLogic
{
    public class Unit : MonoBehaviour
    {
        public UnitClasses.UnitOutline outline;
        public Sprite sprite;
        public UnitState state;
        [SerializeField] private Team team;
        [SerializeField] private UnitType type;
        [SerializeField] private string unitName;
        [SerializeField] private int maxHealth;
        [SerializeField] private int basicAttackPower;        
        [SerializeField] private int speed;
        [SerializeField] private int range;
        [SerializeField] private int price;
        [SerializeField] private string description;
        [SerializeField] private float movingSpeed;
        private int attackPower;
        private int health;
        private bool isActive;
        private bool wasMoved;
        private int healingCD;
        private int abillityCD;
        private TextMeshProUGUI statHUD;
        private CellManager cellManager;
        private UnitsList unitsList;
        [SerializeField] private GameUI ui;
        
        public enum UnitState
        {
            IDLE, Moving, Attacking, Healing, Capturig, Waiting
        }

        public enum Team
        {
            Light, Dark
        }

        public enum UnitType
        {
            Ground, Air
        }

        private void Awake()
        {
            cellManager = CellManager.GetInstance();
            unitsList = UnitsList.GetInstance();
            statHUD = GetComponentInChildren<TextMeshProUGUI>();            
            outline.RemoveOutline();
            attackPower = basicAttackPower;
            health = maxHealth;
            statHUD.text = health.ToString();
            MakeFriendlyStatHUD();
            isActive = false;
            state = UnitState.IDLE;
            healingCD = 0;
            abillityCD = 0;
        }

        public Team GetUnitType()
        {
            return team;
        }

        public void ShowDamage(int value)
        {
            statHUD.text = health.ToString() + "-" + value;
        }

        public void HideDamage()
        {
            statHUD.text = health.ToString();
        }

        public int GetPrice()
        {
            return price;
        }

        public void Heal()
        {
            if (!wasMoved && state == UnitState.IDLE|| state == UnitState.Waiting)
            {
                //StartCoroutine(Healing());
                health += 10;
                statHUD.text = health.ToString();
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, 1f);
            }
        }

        private IEnumerator Healing()
        {
            Debug.Log("Healing was started");
            isActive = false;
            state = UnitState.Healing;
            float healingSpeed = 0.01f;
            float a = 1f;
            SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            while (renderer.color != new Color(1f, 1f, 0f, 1f))
            {
                renderer.color = new Color(1f, 1f, a - healingSpeed, 1f);
                yield return null;
            }
            health += 10;
            state = UnitState.IDLE;
            Debug.Log("Healing ends");
        }
        
        public void MoveTo(Vector3 destination)
        {
            if(!wasMoved && CanRich(destination))
            {
                //move
                StartCoroutine(Moving(destination));
            }
            else
            {
                //cancel
                HideActions();
            }
            
        }

        private IEnumerator Moving(Vector3 destination)
        {
            Debug.Log("Moving was started");
            wasMoved = true;
            state = UnitState.Moving;
            float progress = 0;
            float step = 0.01f;
            while (Vector3.Distance(transform.position, destination) != 0.0f)
            {
                transform.position = Vector3.Lerp(transform.position, destination, progress);
                progress += step;
                yield return null;
            }
            state = UnitState.IDLE;
            Debug.Log("Moving was stopped");
            ShowActions();
        }

        public int GetAttackPower()
        {
            return attackPower;
        }

        public int GetSpeed()
        {
            return speed;
        }

        public void Attack(Vector3 targetPosition)
        {
            Debug.Log(GetAttackPower() + " points of damage were applied");
            unitsList.GetUnit(targetPosition).ApplyDamage(GetAttackPower());
            Disactivate();
            outline.RemoveOutline();
        }

        public void ApplyDamage(int damage)
        {
            health = health - damage;
            if(health < 1)
            {
                ToDie();
            }
            else
            {
                statHUD.text = health.ToString();
            }

        }
        public int GetRange()
        {
            return range;
        }

        public bool CanAttack(Vector3 targetPosition)
        {
            if (Math.Sqrt(Math.Pow(targetPosition.x - transform.position.x, 2) +
                Math.Pow(targetPosition.y - transform.position.y, 2)) <= range)
                return true;
            else
                return false;
        }

        public bool CanRich(Vector3 destination)
        {
            if (Math.Sqrt(Math.Pow(destination.x - transform.position.x, 2) +
                Math.Pow(destination.y - transform.position.y, 2)) <= GetSpeed())
                return true;
            else
                return false;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void Disactivate()
        {
            outline.RemoveOutline(); 
            isActive = false;
            wasMoved = true;
        }

        public bool WasMoved()
        {
            return wasMoved;
        }


        public void Activate()
        {
            isActive = true;
            wasMoved = false;
        }

        public void MakeEnemyStatHUD()
        {
            statHUD.faceColor = new Color(1f, 0f, 0f, 1f);
            statHUD.outlineColor = new Color(165 / 256f, 165 / 256f, 165 / 256f, 1f);
        }

        public void MakeFriendlyStatHUD()
        {
            statHUD.faceColor = new Color(1f, 200 / 256f, 0f, 1f);
            statHUD.outlineColor = new Color(168 / 256f, 150 / 256f, 36 / 256f, 1f);
        }

        public object[] GetUnitInfo()
        {
            object[] info = new object[8];
            info[0] = sprite;
            info[1] = unitName;
            info[2] = maxHealth.ToString();
            info[3] = basicAttackPower.ToString();
            info[4] = range.ToString();
            info[5] = speed.ToString();
            info[6] = price.ToString();
            info[7] = description;
            return info;
        }

        public void HideActions()
        {
            state = UnitState.IDLE;
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

        public void ShowActions()
        {
            Debug.Log("ShowActions");
            HideActions();

            if (isActive && state == UnitState.IDLE)
            {
                state = UnitState.Waiting;

                foreach (Cell cell in cellManager.GetAllCells()) // highlight available cells
                {
                    if (cell != null)
                    {
                        if (!CanRich(cell) && !wasMoved)
                        {
                            cell.GetComponent<Renderer>().material.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
                        }
                    }
                }

                if (unitsList.GetAllEnemies().Length > 0) // outline enemies unit can attack
                {
                    foreach (Unit unit in unitsList.GetAllEnemies())
                    {
                        if (CanAttack(unit.transform.position))
                        {
                            unit.outline.OutlineAsEnemy();
                            unit.ShowDamage(attackPower);
                            cellManager.GetCell(unit.transform.position).GetComponent<Renderer>().material.color = Color.white;
                        }
                    }
                }
            }
        }
        private bool CanRich(Cell cell)
        {
            if (Math.Sqrt(Math.Pow(cell.transform.position.x - transform.position.x, 2) +
                Math.Pow(cell.transform.position.y - transform.position.y, 2)) <= GetSpeed())
                return true;
            else
                return false;
        }

        void ToDie()
        {
            Destroy(gameObject);
        }
    }
}
