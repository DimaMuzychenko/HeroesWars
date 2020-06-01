using TMPro;
using UnityEngine;
using Assets.GameLogic.CellClasses;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.GameLogic
{
    class GameUI : MonoBehaviour
    {
        private PlayerControler playerControler;
        private UnitSelection unitSelection;
        private CellManager cellManager;
        [SerializeField] private GameObject instruction;

        [SerializeField] private Sprite lightBTSprite;
        [SerializeField] private Sprite darkBTSprite;
        [SerializeField] private Sprite darkInfoBTSprite;
        [SerializeField] private Sprite lightInfoBTSprite;

        [SerializeField] private GameObject userControls;
        [SerializeField] private TextMeshProUGUI lightEnergyLableT;
        [SerializeField] private TextMeshProUGUI lightEnergyValueT;
        [SerializeField] private TextMeshProUGUI darkEnergyLableT;
        [SerializeField] private TextMeshProUGUI darkEnergyValueT;
        [SerializeField] private Button infoBT;
        [SerializeField] private Button pathMoveBT;
        [SerializeField] private Button healBT;
        [SerializeField] private Button captureBT;
        private Button[] actionBT;

        [SerializeField] private GameObject playerChangingScreen;
        [SerializeField] private GameObject lightsTurnLable;
        [SerializeField] private GameObject darksTurnLable;

        [SerializeField] private GameObject winScreen;
        [SerializeField] private TextMeshProUGUI winBoxT;
        [SerializeField] private GameObject lightLableT;
        [SerializeField] private GameObject darkLableT;
        [SerializeField] private GameObject lightButtonT;
        [SerializeField] private GameObject darkButtonT;


        private void Awake()
        {
            playerControler = PlayerControler.GetInstance();
            unitSelection = UnitSelection.GetInstance();
            cellManager = CellManager.GetInstance();
            actionBT = new Button[3] { pathMoveBT, healBT, captureBT };
        }

        private void Start()
        {
            playerControler = PlayerControler.GetInstance();
            GameEvents.GetInstance().OnWin += ShowWinScreen;
            GameEvents.GetInstance().OnPlayerChanged += RefreshButtons;
            GameEvents.GetInstance().OnActionDone += RefreshButtons;
            ShowChangingScreen();
            instruction.SetActive(true);
        }

        private void ShowChangingScreen()
        {
            userControls.SetActive(false);
            if (playerControler.FirstPlayerTurn())
            {
                lightsTurnLable.SetActive(true);
                darksTurnLable.SetActive(false);
            }
            else
            {
                lightsTurnLable.SetActive(false);
                darksTurnLable.SetActive(true);
            }
            playerChangingScreen.SetActive(true);
        }

        public void ResumeGame()
        {
            if (playerControler.FirstPlayerTurn())
            {
                darkEnergyLableT.gameObject.SetActive(false);
                darkEnergyValueT.gameObject.SetActive(false);
                lightEnergyLableT.gameObject.SetActive(true);
                lightEnergyValueT.gameObject.SetActive(true);

                infoBT.image.sprite = lightInfoBTSprite;

                foreach(Button button in actionBT)
                {
                    button.image.sprite = lightBTSprite;
                }
            }
            else
            {
                darkEnergyLableT.gameObject.SetActive(true);
                darkEnergyValueT.gameObject.SetActive(true);
                lightEnergyLableT.gameObject.SetActive(false);
                lightEnergyValueT.gameObject.SetActive(false);

                infoBT.image.sprite = darkInfoBTSprite;

                foreach (Button button in actionBT)
                {
                    button.image.sprite = darkBTSprite;
                }
            } 
            UpdateEnergyValue();
            userControls.SetActive(true); 
            playerChangingScreen.SetActive(false);
            RefreshButtons();
        }

        public void PassMove()
        {
            GameEvents.GetInstance().PlayerChanged();
            ShowChangingScreen();
        }

        public void UseHealing()
        {
            // TODO: switch healing interactable for each unit
            // and channel alpha of image of button
            actionBT[1].interactable = false;
            unitSelection.GetSelectedUnit().Heal();
        }

        public void UpdateEnergyValue()
        {
            if (playerControler.FirstPlayerTurn())
                lightEnergyValueT.text = playerControler.GetEnergy().ToString();
            else
                darkEnergyValueT.text = playerControler.GetEnergy().ToString();
        }

        public void Capture()
        {
            // TODO: switch capture interactable for each unit
            // and channel alpha of image of button
            actionBT[2].interactable = false;
            cellManager.CapturePortal(unitSelection.GetSelectedUnit().transform.position);
        }

        private void ShowWinScreen()
        {
            if(PlayerControler.GetInstance().FirstPlayerTurn())
            {
                lightLableT.SetActive(true);
                lightButtonT.SetActive(true);
                darkLableT.SetActive(false);
                darkButtonT.SetActive(false);
            }
            else
            {
                lightLableT.SetActive(false);
                lightButtonT.SetActive(false);
                darkLableT.SetActive(true);
                darkButtonT.SetActive(true);
            }
            winBoxT.text = PlayerControler.GetInstance().GetCurrentPlayer().ToString() + "s win!";
            winScreen.SetActive(true);
        }

        public void ExitToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void OpenInstruction()
        {
            instruction.SetActive(true);
        }

        public void RefreshButtons()
        {
            var unit = unitSelection.GetSelectedUnit();
            if(unit != null)
            {
                if(!unit.WasMoved())
                {
                    if((int)unit.GetUnitTeam() == (int)playerControler.GetCurrentPlayer())
                    {
                        healBT.interactable = true;
                    }
                    if(CellSelection.GetInstance().GetSelectedCell().type == Cell.CellType.LightPortal && unit.GetUnitTeam() == Unit.Team.Dark)
                    {
                        captureBT.interactable = true;
                    }
                    if (CellSelection.GetInstance().GetSelectedCell().type == Cell.CellType.DarkPortal && unit.GetUnitTeam() == Unit.Team.Light)
                    {
                        captureBT.interactable = true;
                    }
                    if(CellSelection.GetInstance().GetSelectedCell().type == Cell.CellType.Portal)
                    {
                        captureBT.interactable = true;
                    }
                    return;
                }
            }
            healBT.interactable = false;
            captureBT.interactable = false;
        }
    }
}
