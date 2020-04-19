using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public Grid grid;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(IsPointerOverUIObject()) // user clicked on UI
            {

            }
            else // user clicked on game object
            {
                Vector3 inputPosition = grid.CellToWorld(grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
                //Debug.Log(inputPosition);
                GameEvents.GetInstance().CellClicked(inputPosition);
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
