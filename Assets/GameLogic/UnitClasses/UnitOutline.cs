using UnityEngine;
using TMPro;

namespace Assets.GameLogic.UnitClasses
{
    [ExecuteInEditMode]
    public class UnitOutline : MonoBehaviour
    {
        private Color friendColor = new Color(1f, 200/256f, 0f, 1f);
        private Color enemyColor = new Color(1f, 0f, 0f, 1f);
        private Color noColor = new Color(1f, 1f, 1f, 0f);     
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            UpdateStatusHUD(noColor);
        }

        public void OutlineAsFriend()
        {
            UpdateStatusHUD(friendColor);            
        }

        public void OutlineAsEnemy()
        {
            UpdateStatusHUD(enemyColor);            
        }

        public void RemoveOutline()
        {
            UpdateStatusHUD(noColor);
        }


        void UpdateStatusHUD(Color color)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetColor("_SolidOutline", color);
            spriteRenderer.SetPropertyBlock(mpb);            
        }
    }
}
