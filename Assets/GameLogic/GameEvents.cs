using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private static GameEvents instance;

    private void Awake()
    {
        instance = this;
    }

    public static GameEvents GetInstance()
    {
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
    public void PlayerChaged()
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
}
