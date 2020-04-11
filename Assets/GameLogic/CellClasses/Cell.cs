using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.GameLogic.CellClasses
{
    public class Cell : MonoBehaviour
    {        
        public CellType type;
        /*[SerializeField] private*/
        SpriteRenderer spriteRenderer;
        public enum CellType
        {
            Portal, LightPortal, DarkPortal, Grass, Send, Rock
        }
        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        public void OutlineCell(bool state)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(mpb);
            if (state)
                mpb.SetFloat("_Thickness", 20);
            else
                mpb.SetFloat("_Thickness", 0);
            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
