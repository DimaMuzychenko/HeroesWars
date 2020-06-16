using System;
using UnityEngine;

public class GameEvents
{
    private static GameEvents instance;


    public static GameEvents GetInstance()
    {
        if (instance == null)
            instance = new GameEvents();
        return instance;
    }

    public event Action<Vector3> OnCellClicked;
    public void CellClicked(Vector3 position)
    {
        if(OnCellClicked != null)
        {
            OnCellClicked(position);
        }
    }

    public event Action OnPlayerChanged;
    public void PlayerChanged()
    {
        if(OnPlayerChanged != null)
        {
            OnPlayerChanged();
        }
    }

    public event Action OnCaptureButtonPressed;
    public void CaptureButtonPressed()
    {
        if(OnCaptureButtonPressed != null)
        {
            OnCaptureButtonPressed();
        }
    }

    public event Action OnWin;
    public void ShowWinScreen()
    {
        if(OnWin != null)
        {
            OnWin();
        }
    }

    public event Action OnActionDone;
    public void ActionDone()
    {
        if(OnActionDone != null)
        {
            OnActionDone();
        }
    }

    public event Action OnUnitSpawned;
    public void UnitSpawned()
    {
        if(OnUnitSpawned != null)
        {
            OnUnitSpawned();
        }
    }
}
