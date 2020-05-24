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
        [SerializeField] private TextMeshProUGUI LightEnergyLableT;
        [SerializeField] private TextMeshProUGUI LightEnergyValueT;
        [SerializeField] private TextMeshProUGUI DarkEnergyLableT;
        [SerializeField] private TextMeshProUGUI DarkEnergyValueT;
        [SerializeField] private Button infoBT;
        [SerializeField] private Button[] actionBT;

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
        }

        private void Start()
        {
            playerControler = PlayerControler.GetInstance();
            GameEvents.GetInstance().OnWin += ShowWinScreen;
            GameEvents.GetInstance().OnPlayerChanged += RefreshButtons;
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
                DarkEnergyLableT.gameObject.SetActive(false);
                DarkEnergyValueT.gameObject.SetActive(false);
                LightEnergyLableT.gameObject.SetActive(true);
                LightEnergyValueT.gameObject.SetActive(true);

                infoBT.image.sprite = lightInfoBTSprite;

                foreach(Button button in actionBT)
                {
                    button.image.sprite = lightBTSprite;
                }
            }
            else
            {
                DarkEnergyLableT.gameObject.SetActive(true);
                DarkEnergyValueT.gameObject.SetActive(true);
                LightEnergyLableT.gameObject.SetActive(false);
                LightEnergyValueT.gameObject.SetActive(false);

                infoBT.image.sprite = darkInfoBTSprite;

                foreach (Button button in actionBT)
                {
                    button.image.sprite = darkBTSprite;
                }
            } 
            UpdateEnergyValue();
            userControls.SetActive(true); 
            playerChangingScreen.SetActive(false); 
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
                LightEnergyValueT.text = playerControler.GetEnergy().ToString();
            else
                DarkEnergyValueT.text = playerControler.GetEnergy().ToString();
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
            // finish with winScreen
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
            winBoxT.text = PlayerControler.GetInstance().GetCurrentPlayer().ToString() + "'s win!";
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
            for(int i = 1; i < actionBT.Length; i++)
            {
                actionBT[i].interactable = true;
            }
        }
    }
}
