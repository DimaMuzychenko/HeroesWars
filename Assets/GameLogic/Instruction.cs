using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Instruction : MonoBehaviour
{
    public TextMeshProUGUI stepT;
    public GameObject previousBT;
    public GameObject nextBT;
    public GameObject fightBT;
    public GameObject[] instruction;
    private int currentStep = 1;

    private void OnEnable()
    {
        instruction[currentStep - 1].SetActive(false);
        currentStep = 1;
        instruction[currentStep - 1].SetActive(true);
        previousBT.SetActive(false);
        fightBT.SetActive(false);
        nextBT.SetActive(true);
        stepT.text = "Step " + currentStep.ToString();
    }

    public void ShowNextStep()
    {
        instruction[currentStep -1].SetActive(false);
        currentStep++;
        instruction[currentStep - 1].SetActive(true);
        if(currentStep == instruction.Length)
        {
            nextBT.SetActive(false);
            fightBT.SetActive(true);
        }
        previousBT.SetActive(true);
        stepT.text = "Step " + currentStep.ToString();
    }

    public void ShowPreviousStep()
    {
        instruction[currentStep - 1].SetActive(false);
        currentStep--;
        instruction[currentStep - 1].SetActive(true);
        if (currentStep == 1)
        {
            previousBT.SetActive(false);
        }
        fightBT.SetActive(false);
        nextBT.SetActive(true);
        stepT.text = "Step " + currentStep.ToString();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
