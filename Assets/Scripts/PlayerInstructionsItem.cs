using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInstructionsItem : MonoBehaviour
{
    public Image chefImage;
    public TextMeshProUGUI playerText;
    public Transform itemsContainer;
    public List<Image> instructionImages;
    
    GameManager gameManager;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void SetInstructions(int playerNumber, PlayerInstructionsData instructionsData)
    {
        playerText.SetText("Player " + playerNumber);
        chefImage.sprite = gameManager.GetChefSprite(instructionsData.chefId);
        int instructionDataCount = instructionsData.instructions.Count;
        for(int i = 0; i < instructionDataCount; i++)
        {
            Instruction instruction = instructionsData.instructions[i];
            Sprite instructionSprite = gameManager.GetInstructionSprite(instruction.type);
            Image image;
            if(i < instructionImages.Count)
            {
                image = instructionImages[i];
                image.gameObject.SetActive(true);
            }
            else
            {
                image = Instantiate(instructionImages[0], itemsContainer);
                instructionImages.Add(image);
            }
            
            image.sprite = instructionSprite;
        }

        for(int i = instructionDataCount; i < instructionImages.Count; i++)
        {
            instructionImages[i].gameObject.SetActive(false);
        }

    }
}
