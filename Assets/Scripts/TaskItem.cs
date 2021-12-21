using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public Image instructionImage;
    public Button button;
    
    GameManager gameManager;
    InstructionType instructionType;

    public void Init(GameManager gameManager)
    {
        button.onClick.AddListener(OnButtonPressed);
        this.gameManager = gameManager;
    }

    public void SetInstructions(InstructionType instructionType)
    {
        this.instructionType = instructionType;
        instructionImage.sprite = gameManager.GetInstructionSprite(instructionType);
    }

    public void OnButtonPressed()
    {
        gameManager.gameplayPhase.OnInstructionPressed(instructionType);
    }
}
