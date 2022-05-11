using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionsPhase : MonoBehaviour
{
    public Transform root;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI stageText;

    public Transform uiItemsContainer;
    public List<PlayerInstructionsItem> playerInstructionsItems;
    
    GameManager gameManager;
    bool initialized;

    bool isShowing;

    float timer;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        for(int i = 0; i < playerInstructionsItems.Count; i++)
        {
            playerInstructionsItems[i].Init(gameManager);
        }
        
        root.gameObject.SetActive(false);
        initialized = true;
    }

    public void LoadInstructions(List<PlayerInstructionsData> playerInstructionsDatas)
    {
        int playerCount = playerInstructionsDatas.Count;
        
        for(int i = 0; i < playerCount; i++)
        {
            PlayerInstructionsData playerInstructionsData = playerInstructionsDatas[i];            
            PlayerInstructionsItem item;
            if(i < playerInstructionsItems.Count)
            {
                item = playerInstructionsItems[i];
                item.gameObject.SetActive(true);
            }
            else
            {
                item = Instantiate(playerInstructionsItems[0], uiItemsContainer);
                item.Init(gameManager);
                playerInstructionsItems.Add(item);
            }
            
            item.SetInstructions(i + 1, playerInstructionsData);
            
        }
        
        for(int i = playerCount; i < playerInstructionsItems.Count; i++)
        {
            playerInstructionsItems[i].gameObject.SetActive(false);
        }

        isShowing = true;

        float difficultyRatio = gameManager.GetDifficultyRatio();
        if(difficultyRatio < 0.3f)
        {
            timer = Parameter.TIME_LIMIT_INSTRUCTIONS_PHASE_EASY;
        }
        else if (difficultyRatio < 0.8f)
        {
            timer = Parameter.TIME_LIMIT_INSTRUCTIONS_PHASE_MEDIUM;
        }
        else
        {
            timer = Parameter.TIME_LIMIT_INSTRUCTIONS_PHASE_HARD;
        }
        
        timerText.SetText(Mathf.RoundToInt(timer).ToString());
        stageText.SetText("Stage " + gameManager.currStage);
        root.gameObject.SetActive(true);
        SoundManager.Instance.PlayBGM(BGM.Instructions);
    }

    public void StartGameplay()
    {
        gameManager.StartGameplay();
        root.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
        if(initialized)
        {
            if(isShowing)
            {
                //!TODO: update timers etc here
                timer -= Time.deltaTime;
                
                timerText.SetText(Mathf.RoundToInt(timer).ToString());

                if(timer <= 0)
                {
                    isShowing = false;
                    StartGameplay();
                }
            }
        }
    }
}
