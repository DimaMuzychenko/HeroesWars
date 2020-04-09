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
        public UnitType type;
        public string unitName;
        public int maxHealth;
        public int attackPower;
        public int speed;
        public int range;
        public int price;
        public string description;
        private int health;
        private bool isActive;
        private bool wasMoved;
        private TextMeshProUGUI statHUD;

        public enum UnitType
        {
            Warrior, Archer, Mage
        }

        private void Awake()
        {
            statHUD = GetComponentInChildren<TextMeshProUGUI>();            
            outline.RemoveOutline();
            health = maxHealth;
            statHUD.text = health.ToString();
            MakeFriendlyStatHUD();
            isActive = false;
        }

        public UnitType GetUnitType()
        {
            return type;
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
            transform.position = destination;
            wasMoved = true;
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

        void ToDie()
        {
            Destroy(gameObject);
        }
    }
}
