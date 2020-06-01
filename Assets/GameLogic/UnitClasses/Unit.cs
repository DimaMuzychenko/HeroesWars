using Assets.GameLogic.CellClasses;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.GameLogic
{
    public class Unit : MonoBehaviour
    {
        public UnitClasses.StatHUD statHUD;
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
        private CellManager cellManager;
        private UnitsList unitsList;

        // TODO: dieing animation
        
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
            pathFinder = PathFinder.GetInstance();            
            attackPower = basicAttackPower;
            health = maxHealth;
            isActive = false;
            state = UnitState.IDLE;
            statHUD.SetHUDText(health.ToString());
        }

        public Team GetUnitTeam()
        {
            return team;
        }

        public void ShowDamage(int damage)
        {
            statHUD.SetHUDText(health.ToString() + "-" + damage.ToString());
        }

        public void HideDamage()
        {
            statHUD.SetHUDText(health.ToString());
        }
        

        public int GetPrice()
        {
            return price;
        }

        public void Heal()
        {
            if (!wasMoved && state == UnitState.IDLE || state == UnitState.Waiting)
            {
                wasMoved = true;
                state = UnitState.Healing;
                animator.SetInteger("State", (int)state);
                health += 10;
                statHUD.SetHUDText(health.ToString());
            }
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

        public void MakeFriendlyStatHUD()
        {
            statHUD.HighlightAsFriend();
        }

        public int GetAttackPower()
        {
            return attackPower;
        }

        public void MakeEnemyStatHUD()
        {
            statHUD.HighlightAsEnemy();
        }

        public void MakeNeutralStatHUD()
        {
            statHUD.RemoveHighlighting();
        }

        public int GetSpeed()
        {
            return speed;
        }

        public void Attack(Vector3 targetPosition)
        {
            StartCoroutine(Attacking(targetPosition));
            Disactivate();
            statHUD.RemoveHighlighting();
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
                statHUD.SetHUDText(health.ToString());
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
            statHUD.RemoveHighlighting();
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
                    unit.statHUD.RemoveHighlighting();
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
                    foreach (Cell cell in cellManager.GetAllCells())
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
                    if (Vector3.Distance(unit.transform.position, transform.position) < GetRange() + 0.1f)
                    {
                        unit.statHUD.HighlightAsEnemy();
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
