using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Assets.GameLogic.UnitClasses
{
    [ExecuteInEditMode]
    public class StatHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statHUD;
        [SerializeField] private Color noColor;
        [SerializeField] private Color friendColor;
        [SerializeField] private Color enemyColor;


        private void Awake()
        {
            SetHUDColor(noColor);
        }

        public void HighlightAsFriend()
        {
            SetHUDColor(friendColor);            
        }

        public void HighlightAsEnemy()
        {
            SetHUDColor(enemyColor);
        }

        public void RemoveHighlighting()
        {
            SetHUDColor(noColor);
        }


        void SetHUDColor(Color color)
        {
            statHUD.color = color;
        }

        public void SetHUDText(string text)
        {
            statHUD.text = text;
        }


    }
}
