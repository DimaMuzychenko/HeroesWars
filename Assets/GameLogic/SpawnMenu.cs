﻿using UnityEngine;
using UnityEngine.UI;
using Assets.GameLogic;
using Assets.GameLogic.UnitClasses;
using TMPro;

public class SpawnMenu : MonoBehaviour
{
    public static bool isActive = false;
    public ToggleGroup toggles;    
    [SerializeField] private UnitControler unitControler;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private PrefabsList prefabsList;
    [SerializeField] private Button summonBT;
    [SerializeField] private Unit[] units;
    private Unit selectedUnit;
    private Image[] images = new Image[6];

    //selected unit's info    
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI unitNameT;
    [SerializeField] private TextMeshProUGUI healthInfoT;
    [SerializeField] private TextMeshProUGUI attackPowerInfoT;
    [SerializeField] private TextMeshProUGUI attackRangeInfoT;
    [SerializeField] private TextMeshProUGUI speedInfoT;
    [SerializeField] private TextMeshProUGUI priceInfoT;
    [SerializeField] private TextMeshProUGUI descriptionT;

    private void Awake()
    {
        for(int i = 0; i < toggles.GetComponentsInChildren<Toggle>().Length; i++)
        {
            foreach (Image image in toggles.GetComponentsInChildren<Toggle>()[i].GetComponentsInChildren<Image>())
            {
                if (image.gameObject.name == "Image")
                    images[i] = image;
            }
        }
    }

    private void OnEnable()
    {
        Debug.Log("Spawn menu is opened");
        isActive = true;
        UpdateTeamOfUnits();
        SelectUnit();
    }
    private void OnDisable()
    {
        Debug.Log("Spawn menu is closed");
        isActive = false;
    }

    public void SelectUnit()
    {
        for(int i = 0; i < toggles.GetComponentsInChildren<Toggle>().Length; i++)
        {
            if(toggles.GetComponentsInChildren<Toggle>()[i].isOn)
            {
                selectedUnit = units[i];
            }
        }
        UpdateInfo();
    }

    public void SpawnUnit()
    {
        unitControler.SpawnUnit(selectedUnit.GetUnitType());
        gameUI.SpendEnergy(selectedUnit.GetPrice());
        Close();
    }

    private void UpdateInfo()
    {
        object[] info = selectedUnit.GetUnitInfo();
        image.sprite = (Sprite)info[0];
        unitNameT.text = (string)info[1];
        healthInfoT.text = (string)info[2];
        attackPowerInfoT.text = (string)info[3];
        attackRangeInfoT.text = (string)info[4];
        speedInfoT.text = (string)info[5];
        priceInfoT.text = (string)info[6];
        descriptionT.text = (string)info[7];

        if (selectedUnit.GetPrice() > gameUI.GetCurrentPlayerEnergy())
        {
            summonBT.interactable = false;
            summonBT.GetComponentInChildren<TextMeshProUGUI>().color = new Color(200/256f, 200 / 256f, 200 / 256f, 0.5f);
        }
        else
        {
            summonBT.interactable = true;
            summonBT.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
        }
    }
    public void Open()
    {        
        summonBT.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void OpenOnlyInfo()
    {
        summonBT.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void UpdateTeamOfUnits()
    {
        if(TurnCounter.GetInstance().FirstPlayerTurn())
        {
            units = prefabsList.GetAllLeftUnits();
        }
        else
        {
            units = prefabsList.GetAllRightUnits();
        }
        for(int i = 0; i < 3; i++)
        {
            images[i].sprite = units[i].sprite;
        }
    }
}
