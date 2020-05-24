using Assets.GameLogic.CellClasses;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

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
        [SerializeField] private float movementSpeed;
        [SerializeField] private int range;
        [SerializeField] private int price;
        [SerializeField] private string description;


        [SerializeField] private Animator animator;

        private PathFinder pathFinder;  
        private int attackPower;
        private int health;
        private bool isActive;
        private bool wasMoved;
        private TextMeshProUGUI statHUD;
        private CellManager cellManager;
        private UnitsList unitsList;
        
        public enum UnitState
        {
            IDLE, MovingRight, MovingLeft, AttackingLeft, AttackingRight, Healing, Capturig, Waiting
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
            pathFinder = PathFinder.GetInstance();
            outline.RemoveOutline();
            attackPower = basicAttackPower;
            health = maxHealth;
            statHUD.text = health.ToString();
            MakeFriendlyStatHUD();
            isActive = false;
            state = UnitState.IDLE;
        }

        public Team GetUnitTeam()
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
                state = UnitState.Healing;
                animator.SetInteger("State", (int)state);
                health += 10;
                statHUD.text = health.ToString();
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

        public void AnimationFinished()
        {
            state = UnitState.IDLE;
            animator.SetInteger("State", (int)state);
            Debug.Log("Animation was finished");
        }
        
        public void MoveTo(Vector3 destination)
        {
            if (isActive && !wasMoved && pathFinder.CanRich(destination))
            {
                //move
                var path = pathFinder.FindPath(destination);
                //transform.position = path[path.Length - 1].transform.position;
                //ShowActions();
                StartCoroutine(Moving(path));                
            }
            else
            {
                //cancel
                HideActions();
            }
            
        }

        private IEnumerator Moving(Cell[] path)
        {
            Debug.Log("Moving was started");
            wasMoved = true;
            var startPosition = transform.position;
            foreach (var point in path)
            {
                var destination = point.transform.position;                
                var progress = 0f;
                if(IsOnRightSide(destination))
                {
                    state = UnitState.MovingRight;
                }
                else
                {
                    state = UnitState.MovingLeft;
                }
                animator.SetInteger("State", (int)state);
                while (transform.position != destination)
                {
                    progress += Time.fixedDeltaTime * movementSpeed;
                    transform.position = Vector3.Lerp(startPosition, point.transform.position, progress);
                    yield return null;
                }
                startPosition = destination;
            }
            transform.position = new Vector3((float)Math.Round(transform.position.x, 2), (float)Math.Round(transform.position.y, 2), transform.position.z);  
            state = UnitState.IDLE;
            animator.SetInteger("State", (int)state);
            ShowActions();
            Debug.Log("Moving was stopped");
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
            StartCoroutine(Attacking(targetPosition));
            Disactivate();
            outline.RemoveOutline();
        }

        private IEnumerator Attacking(Vector3 target)
        {
            Debug.Log("Attacking was started");
            if(IsOnRightSide(target))
            {
                state = UnitState.AttackingRight;
            }
            else
            {
                state = UnitState.AttackingLeft;
            }
            animator.SetInteger("State", (int)state);
            yield return new WaitForSeconds(0.15f);
            Debug.Log(GetAttackPower() + " points of damage were applied");
            unitsList.GetUnit(target).ApplyDamage(GetAttackPower());
            yield return new WaitForSeconds(0.60f);
            state = UnitState.IDLE;
            Debug.Log("Attacking was stopped");
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

        private bool CloseEnemyExist()
        {
            if(unitsList.GetAllEnemies().Length > 0)
            {
                foreach(Unit enemy in unitsList.GetAllEnemies())
                {
                    if (pathFinder.CanAttack(enemy))
                        return true;
                }
            }
            return false;
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

                if(!wasMoved)
                {
                    var availableCells = pathFinder.GetAvailableCells();
                    Debug.Log("Available cells : " + availableCells.Count);
                    foreach (Cell cell in cellManager.GetAllCells()) // highlight available cells
                    {
                        if (cell != null)
                        {
                            if (!availableCells.Contains(cell))
                            {
                                cell.GetComponent<Renderer>().material.color = new Color(100 / 255f, 100 / 255f, 100 / 255f);
                            }
                        }
                    }
                }
                bool f = false;
                foreach (Unit unit in unitsList.GetAllEnemies())
                {
                    //if (pathFinder.CanAttack(unit.transform.position))
                    if (Vector3.Distance(unit.transform.position, transform.position) < GetRange() + 0.1f)
                    {
                        unit.outline.OutlineAsEnemy();
                        unit.ShowDamage(attackPower);
                        cellManager.GetCell(unit.transform.position).GetComponent<Renderer>().material.color = Color.white;
                        f = true;
                    }
                }

                if (wasMoved && !f)
                {
                    Disactivate();
                }
            }
        }        

        private bool IsOnRightSide(Vector3 target)
        {
            if (target.x - transform.position.x > 0)
                return true;
            return false;
        }

        void ToDie()
        {
            Destroy(gameObject);
        }
    }
}
