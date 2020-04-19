using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.GameLogic
{
    public class Unit : MonoBehaviour
    {
        public UnitClasses.UnitOutline outline;
        public Sprite sprite;
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
        private TextMeshProUGUI statHUD;
        

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
            statHUD = GetComponentInChildren<TextMeshProUGUI>();            
            outline.RemoveOutline();
            attackPower = basicAttackPower;
            health = maxHealth;
            statHUD.text = health.ToString();
            MakeFriendlyStatHUD();
            isActive = false;
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
        
        public void MoveTo(Vector3 destination)
        {
            wasMoved = true;
            transform.position = destination;
            
        }

        public int GetAttackPower()
        {
            return attackPower;
        }

        public int GetSpeed()
        {
            return speed;
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
                Math.Pow(destination.y - transform.position.y, 2)) <= speed)
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

        void ToDie()
        {
            Destroy(gameObject);
        }
    }
}
