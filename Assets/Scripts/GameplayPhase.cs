using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ChefState
{
    Normal,
    Angry,
    VeryAngry
}

public class GameplayPhase : MonoBehaviour
{
    public Transform root;
    public Image currChefImage;
    public Image timerBar;
    public Animator chefTimerAnim;
    public List<Image> lifeImages;
    public List<TaskItem> taskItems;

    public Animator correctOverlayAnim;
    public Animator incorrectOverlayAnim;

    [Header("End Game")]
    public Transform rootEndGame;
    public TextMeshProUGUI endGameText;
    public Button nextButton;
    public Image headChefImage;
    
    GameManager gameManager;
    bool initialized;

    bool isPlaying;

    //!note: gameplay variables
    int totalTurns;
    int turnCount;
    List<PlayerInstructionsData> playerInstructionsDatas;
    int currPlayerIndex;
    float timer;
    int currLifePoints;
    float totalTimer;
    ChefState currChefState;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        for(int i = 0; i < taskItems.Count; i++)
        {
            taskItems[i].Init(gameManager);
        }

        root.gameObject.SetActive(false);
        nextButton.onClick.AddListener(OnNextButtonPressed);

        initialized = true;
    }

    public void LoadStage(List<PlayerInstructionsData> playerInstructionsDatas)
    {
        this.playerInstructionsDatas = playerInstructionsDatas;
        currLifePoints = Mathf.Max(Parameter.TOTAL_LIFE_POINTS_MINIMUM, playerInstructionsDatas.Count);
        turnCount = 0;
        totalTurns = 0;
        
        for(int i = 0; i < lifeImages.Count; i++)
        {
            if(i < currLifePoints)
            {
                lifeImages[i].gameObject.SetActive(true);
            }
            else
            {
                lifeImages[i].gameObject.SetActive(false);
            }
        }
        
        List<InstructionType> playableInstructionTypes = gameManager.playableInstructionTypes;

        //! set the board
        for(int i = 0; i < taskItems.Count; i++)
        {
            taskItems[i].SetInstructions(playableInstructionTypes[i]);
        }
        
        for(int i = 0; i < playerInstructionsDatas.Count; i++)
        {
            PlayerInstructionsData instructionsData = playerInstructionsDatas[i];
            List<Instruction> instructions = instructionsData.instructions;
            for(int j = 0; j < instructions.Count; j++)
            {
                totalTurns += 1;
            }
        }

        currPlayerIndex = GetNextTurn();
        StartPlayerTurn();
        
        isPlaying = true;
        root.gameObject.SetActive(true);
        rootEndGame.gameObject.SetActive(false);
    }

    public void StartPlayerTurn()
    {
        turnCount++;
        PlayerInstructionsData instructionsData = playerInstructionsDatas[currPlayerIndex];
        currChefImage.sprite = gameManager.GetChefSprite(instructionsData.chefId);
        float ratio = Mathf.Min(((float)turnCount + 3f) / totalTurns, 1f);
        totalTimer = Mathf.Lerp(Parameter.TIME_LIMIT_SINGLE_TASK_MAX, Parameter.TIME_LIMIT_SINGLE_TASK_MIN, ratio);
        timer = totalTimer;
        clockPlayed = false;
        
        SetChefState(ChefState.Normal);
    }

    void SetChefState(ChefState chefState)
    {
        if(currChefState != chefState)
        {
            switch(chefState)
            {
            case ChefState.Normal:
                chefTimerAnim.SetTrigger("step1");
                break;
            case ChefState.Angry:
                chefTimerAnim.SetTrigger("step2");
                break;
            case ChefState.VeryAngry:
                chefTimerAnim.SetTrigger("step3");
                break;
            }

            currChefState = chefState;
        }
    }

    int GetNextTurn()
    {
        int playerCount = playerInstructionsDatas.Count;
        int nextPlayerIndex = Random.Range(0, playerCount);
        PlayerInstructionsData instructionsData = playerInstructionsDatas[nextPlayerIndex];
        while(instructionsData.instructionIndex >= instructionsData.instructions.Count)
        {
            nextPlayerIndex = Random.Range(0, playerCount);
            instructionsData = playerInstructionsDatas[nextPlayerIndex];
        }

        return nextPlayerIndex;
    }

    bool clockPlayed;
    public void Update()
    {
        if(initialized)
        {
            if(isPlaying)
            {
                timer -= Time.deltaTime;
                float currTimerRatio = timer / totalTimer;
                timerBar.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(0f, Parameter.TIMER_BAR_WIDTH_DEFAULT, currTimerRatio ), 52.1f);
    
                if(timer < 2.5f && !clockPlayed)
                {
                    clockPlayed = true;
                    SoundManager.Instance.PlaySfx(SFX.Clock);
                }

                if(currTimerRatio < 0.33)
                {
                    SetChefState(ChefState.VeryAngry);
                }
                else if(currTimerRatio < 0.66f)
                {
                    SetChefState(ChefState.Angry);
                }

                if(timer <= 0)
                {
                    TakeDamage();
                }
            }
        }
    }

    void TakeDamage()
    {
        bool gameEnded = false;
        bool turnDepleted = CheckTurnDepleted();
        //!reduce health, check if lose
        currLifePoints -= 1;
        int lifeIndex = Mathf.Max(currLifePoints, 0);
        if(currLifePoints <= 0 || turnDepleted)
        {
            gameEnded = true;
            lifeImages[0].gameObject.SetActive(false);
        }
        else
        {
            lifeImages[lifeIndex].gameObject.SetActive(false);
        }
        
        if(!gameEnded)
        {
            SoundManager.Instance.PlaySfx(SoundManager.Instance.GetRandomMaleSFX());
            currPlayerIndex = GetNextTurn();
            StartPlayerTurn();
        }
        else
        {
            isPlaying = false;

            //!TODO: end game here
            ShowEndGameScreen(false);
        }
        incorrectOverlayAnim.SetTrigger("flash");
        
    }

    

    public void ShowEndGameScreen(bool isWin)
    {
        SoundManager.Instance.StopAllBGM();
        rootEndGame.gameObject.SetActive(true);
        string text = isWin ? Parameter.WIN_QUOTES[Random.Range(0, Parameter.WIN_QUOTES.Length)] : Parameter.LOSE_QUOTES[Random.Range(0, Parameter.LOSE_QUOTES.Length)];
        endGameText.SetText(text);
        SoundManager.Instance.PlaySfx(isWin ? SFX.Win : SFX.Lose);
        headChefImage.sprite = gameManager.GetHeadChefSprite(isWin ? ChefState.Normal : ChefState.Angry);
    }

    bool CheckTurnDepleted()
    {
        return turnCount >= totalTurns;
    }

    public void OnInstructionPressed(InstructionType instructionType)
    {
        //! validate player answer
        PlayerInstructionsData instructionsData = playerInstructionsDatas[currPlayerIndex];
        InstructionType correctInstruction = instructionsData.instructions[instructionsData.instructionIndex].type;
        instructionsData.instructionIndex += 1;
        bool correct = correctInstruction == instructionType;
        if(correct)
        {
            //!TODO: show correct visuals here
            bool turnDepleted = CheckTurnDepleted();
            if(!turnDepleted)
            {
                SoundManager.Instance.PlaySfx(SFX.Correct);
                currPlayerIndex = GetNextTurn();
                StartPlayerTurn();
            }
            else
            {
                isPlaying = false;

                //!TODO: goto next level or end game here
                ShowEndGameScreen(true);
                
            }
            correctOverlayAnim.SetTrigger("flash");
        }
        else
        {
            TakeDamage();
        }
    }

    public void OnNextButtonPressed()
    {
        root.gameObject.SetActive(false);
        rootEndGame.gameObject.SetActive(false);
        gameManager.ShowMainMenu();
    }
}
