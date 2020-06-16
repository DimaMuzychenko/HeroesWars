using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{

    public GameObject camera_GameObject;
    public Grid grid;

    Vector2 StartPosition;
    Vector2 DragStartPosition;
    Vector2 DragNewPosition;
    Vector2 Finger0Position;
    float DistanceBetweenFingers;
    bool isZooming;
    bool isScrolling;
    bool mousePresent;

    private void Awake()
    {
        mousePresent = Input.mousePresent;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isZooming && !isScrolling)
        {
            if (IsPointerOverUIObject())
            {

            }
            else
            {
                Vector3 inputPosition = grid.CellToWorld(grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
                Debug.Log("Apply input: " + inputPosition);
                GameEvents.GetInstance().CellClicked(inputPosition);
            }
        }

        if (mousePresent)
        {
            if(Input.GetMouseButton(1))
            {
                isScrolling = true;
                Vector2 NewPosition = GetWorldPosition();
                Vector2 PositionDifference = NewPosition - StartPosition;
                camera_GameObject.transform.Translate(-PositionDifference);
            }
            else
            {
                isScrolling = false;
            }
            StartPosition = GetWorldPosition();
            if(Input.mouseScrollDelta.y != 0)
            {
                if(Camera.main.orthographicSize <= 3.5f && Input.mouseScrollDelta.y < 0)
                {
                    isZooming = true;
                    Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
                }
                if (Camera.main.orthographicSize >= 1f && Input.mouseScrollDelta.y > 0)
                {
                    isZooming = true;
                    Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
                }
            }
            else
            {
                isZooming = false;
            }
        }
        else
        {
            if (Input.touchCount == 0 && isZooming)
            {
                isZooming = false;
            }

            if (Input.touchCount == 1)
            {
                if (!isZooming)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        isScrolling = true;
                        Vector2 NewPosition = GetWorldPosition();
                        Vector2 PositionDifference = NewPosition - StartPosition;
                        camera_GameObject.transform.Translate(-PositionDifference);
                    }
                    isScrolling = false;
                    StartPosition = GetWorldPosition();
                }
            }
            else if (Input.touchCount == 2)
            {
                if (Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    isZooming = true;

                    DragNewPosition = GetWorldPositionOfFinger(1);
                    Vector2 PositionDifference = DragNewPosition - DragStartPosition;

                    if (Vector2.Distance(DragNewPosition, Finger0Position) < DistanceBetweenFingers)
                        camera_GameObject.GetComponent<Camera>().orthographicSize += (PositionDifference.magnitude);

                    if (Vector2.Distance(DragNewPosition, Finger0Position) >= DistanceBetweenFingers)
                        camera_GameObject.GetComponent<Camera>().orthographicSize -= (PositionDifference.magnitude);

                    DistanceBetweenFingers = Vector2.Distance(DragNewPosition, Finger0Position);
                }
                DragStartPosition = GetWorldPositionOfFinger(1);
                Finger0Position = GetWorldPositionOfFinger(0);
            }
        }
    }

    private Vector2 GetWorldPosition()
    {
        return camera_GameObject.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
    }

    private Vector2 GetWorldPositionOfFinger(int FingerIndex)
    {
        return camera_GameObject.GetComponent<Camera>().ScreenToWorldPoint(Input.GetTouch(FingerIndex).position);
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
