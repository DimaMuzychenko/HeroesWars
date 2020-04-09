using UnityEngine;
using UnityEngine.UI;
using Assets.GameLogic;
using Assets.GameLogic.UnitClasses;
using TMPro;

public class SpawnMenu : MonoBehaviour
{
    public static bool isActive = false;
    private Unit selectedUnit;

    [SerializeField] private UnitControler unitControler;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private PrefabsList prefabsList;
    [SerializeField] private Button summonBT;
    public ToggleGroup toggles;
    [SerializeField] private Unit[] units;

    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI typeInfoT;
    [SerializeField] private TextMeshProUGUI healthInfoT;
    [SerializeField] private TextMeshProUGUI attackPowerInfoT;
    [SerializeField] private TextMeshProUGUI attackRangeInfoT;
    [SerializeField] private TextMeshProUGUI speedInfoT;
    [SerializeField] private TextMeshProUGUI priceInfoT;
    [SerializeField] private TextMeshProUGUI descriptionT;

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
        image.sprite = selectedUnit.sprite;
        typeInfoT.text = selectedUnit.type.ToString();
        healthInfoT.text = selectedUnit.maxHealth.ToString();
        attackPowerInfoT.text = selectedUnit.attackPower.ToString();
        attackRangeInfoT.text = selectedUnit.range.ToString();
        speedInfoT.text = selectedUnit.speed.ToString();
        priceInfoT.text = selectedUnit.price.ToString();
        descriptionT.text = selectedUnit.description;

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
        for(int i = 0; i < toggles.GetComponentsInChildren<Image>().Length; i++)
        {
            toggles.GetComponentsInChildren<Image>()[i].sprite = units[i].sprite;
        }
    }
}
