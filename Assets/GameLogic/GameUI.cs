using TMPro;
using UnityEngine;
using Assets.GameLogic.CellClasses;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.GameLogic
{
    class GameUI : MonoBehaviour
    {
        private bool buttonSwitched;
        private const int PORTALPROFIT = 50;
        private static int leftPlayerEnergy;
        private static int rightPlayerEnergy;
        [SerializeField] private TextMeshProUGUI leftEnergyT;
        [SerializeField] private TextMeshProUGUI rightEnergyT;
        [SerializeField] private TextMeshProUGUI mainButtonT;
        [SerializeField] private SpawnMenu spawnMenu;
        [SerializeField] private GameObject winScreen;
        [SerializeField] private TextMeshProUGUI winBoxT;

        private void Start()
        {
            GameEvents.GetInstance().OnWin += ShowWinScreen;
            leftPlayerEnergy = 100;
            rightPlayerEnergy = 100;
            leftEnergyT.text = leftPlayerEnergy.ToString();
            rightEnergyT.text = rightPlayerEnergy.ToString();
        }

        public void MainButtonPressed()
        {
            if(buttonSwitched)
            {
                GameEvents.GetInstance().CaptureButtonPressed();
            }
            else
            {
                GameEvents.GetInstance().PlayerChaged();
                if (TurnCounter.GetInstance().GetTurnsCount() > 0)
                {
                    AddEnergy();
                }
            }
        }

        public void AddEnergy()
        {
            if (TurnCounter.GetInstance().FirstPlayerTurn())
            {
                leftPlayerEnergy += CellManager.GetInstance().LeftPortalCount() * PORTALPROFIT;
            }
            else
            {
                rightPlayerEnergy += CellManager.GetInstance().RightPortalCount() * PORTALPROFIT;
            }
            UpdateEnergyT();
        }

        public void UpdateEnergyT()
        {
            leftEnergyT.text = leftPlayerEnergy.ToString();
            rightEnergyT.text = rightPlayerEnergy.ToString();
        }

        public int GetCurrentPlayerEnergy()
        {
            if (TurnCounter.GetInstance().FirstPlayerTurn())
            {
                return leftPlayerEnergy;
            }
            else
            {
                return rightPlayerEnergy;
            }
        }

        public void SpendEnergy(int amount)
        {
            if (TurnCounter.GetInstance().FirstPlayerTurn())
            {
                leftPlayerEnergy -= amount;
            }
            else
            {
                rightPlayerEnergy -= amount;
            }
            UpdateEnergyT();
        }

        public bool IsButtonSwiched()
        {
            return buttonSwitched;
        }

        public void SwitchMainButton()
        {
            if(buttonSwitched)
            {
                mainButtonT.SetText("Finish turn");
                buttonSwitched = false;
            }
            else
            {
                mainButtonT.SetText("Capture the portal");
                buttonSwitched = true;
            }
        }

        public void ShowInfoScreen()
        {
            spawnMenu.OpenOnlyInfo();
        }

        private void ShowWinScreen()
        {
            winBoxT.text = TurnCounter.GetInstance().GetCurrentPlayer().ToString() + " player win!";
            winScreen.SetActive(true);
        }

        public void ExitToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
