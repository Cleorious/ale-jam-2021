using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PlayerInstructionsData
{
    public List<Instruction> instructions;
    public int instructionIndex; //!note: starts at 0, increases when player has completed/failed their turn
    public int chefId;
}

public class Instruction
{
    // public Sprite instructionSprite;
    public InstructionType type;

    public Instruction(InstructionType instructionType)
    {
        this.type = instructionType;
    }
}

public enum InstructionType
{
    Bowl,
    Cleaver,
    Carrot,
    Cheese,
    Drumstick,
    Eggplant,
    Spork,
    Garlic,
    Knife,
    Meat,
    Milk,
    Mushroom,
    Salt,
    Tomato,
    Onion,
    
    COUNT
}

public class GameManager : MonoBehaviour
{
    public MainMenu mainMenu;
    public InstructionsPhase instructionsPhase;
    public GameplayPhase gameplayPhase;

    [HideInInspector]
    public SpriteAtlas spriteAtlas;

    bool initialized;
    
    
    //!gameplay data stuff
    public int currStage;
    List<PlayerInstructionsData> playerInstructionsDatas;
    [HideInInspector]
    public List<InstructionType> playableInstructionTypes;
    int currPlayerCount;

    
    void Start()
    {
        spriteAtlas = Resources.Load<SpriteAtlas>("Atlas/SpriteAtlas");
        
        SoundManager.Instance.Init(this);
        mainMenu.Init(this);
        instructionsPhase.Init(this);
        gameplayPhase.Init(this);
        
        
        initialized = true;
        
        ShowMainMenu();
    }

    void Update()
    {
        
    }

    public void ShowMainMenu()
    {
        SoundManager.Instance.PlayBGM(BGM.MainMenu);
        mainMenu.root.gameObject.SetActive(true);
    }

    public List<InstructionType> GeneratePlayableInstructions()
    {
        List<InstructionType> ret = new List<InstructionType>();
        
        int totalInstructionCount = (int)InstructionType.COUNT;
        for(int j = 0; j < Parameter.INSTRUCTION_MAX_TOTAL_PLAYABLE_COUNT; j++)
        {
            InstructionType instructionType = (InstructionType)Random.Range(0, (int)InstructionType.COUNT);
            
            while(ret.Contains(instructionType))
            {
                instructionType = (InstructionType)Random.Range(0, totalInstructionCount);
            }
            
            ret.Add(instructionType);
        }

        return ret;
    }

    public List<PlayerInstructionsData> GeneratePlayerInstructions(int playerCount)
    {
        float difficultyRatio = GetDifficultyRatio();
        int minInstructions;
        int maxInstructions;
        if(difficultyRatio < 0.3f)
        {
            minInstructions = Parameter.INSTRUCTION_MIN_COUNT_EASY;
            maxInstructions = Parameter.INSTRUCTION_MAX_COUNT_EASY;
        }
        else if (difficultyRatio < 0.8f)
        {
            minInstructions = Parameter.INSTRUCTION_MIN_COUNT_MEDIUM;
            maxInstructions = Parameter.INSTRUCTION_MAX_COUNT_MEDIUM;
        }
        else
        {
            minInstructions = Parameter.INSTRUCTION_MIN_COUNT_HARD;
            maxInstructions = Parameter.INSTRUCTION_MAX_COUNT_HARD;
        }
        
        List<PlayerInstructionsData> ret = new List<PlayerInstructionsData>();
        List<int> assignedChefIds = new List<int>();
        for(int i = 0; i < playerCount; i++)
        {
            PlayerInstructionsData playerInstructionsData = new PlayerInstructionsData();
            playerInstructionsData.instructions = new List<Instruction>();
            int instructionCount = Random.Range(minInstructions, maxInstructions);
            for(int j = 0; j < instructionCount; j++)
            {
                InstructionType instructionType = (InstructionType)Random.Range(0, (int)InstructionType.COUNT);
                while(!playableInstructionTypes.Contains(instructionType))
                {
                    instructionType = (InstructionType)Random.Range(0, (int)InstructionType.COUNT);
                }
                
                playerInstructionsData.instructions.Add(new Instruction(instructionType));
            }
            
            playerInstructionsData.chefId = Random.Range(0, Parameter.CHEF_FACES_COUNT);
            while(assignedChefIds.Contains(playerInstructionsData.chefId))
            {
                playerInstructionsData.chefId = Random.Range(0, Parameter.CHEF_FACES_COUNT);
            }
            
            assignedChefIds.Add(playerInstructionsData.chefId);
            ret.Add(playerInstructionsData);
        }

        return ret;
    }

    public float GetDifficultyRatio()
    {
        float ret = currStage / 5f;

        return ret;
    }

    public void PlayGame(int playerCount)
    {
        currStage = 1;
        currPlayerCount = playerCount;
        
        playableInstructionTypes = GeneratePlayableInstructions();
        playerInstructionsDatas = GeneratePlayerInstructions(currPlayerCount);

        instructionsPhase.LoadInstructions(playerInstructionsDatas);
    }

    public void NextStage()
    {
        currStage++;
        
        playableInstructionTypes = GeneratePlayableInstructions();
        playerInstructionsDatas = GeneratePlayerInstructions(currPlayerCount);

        instructionsPhase.LoadInstructions(playerInstructionsDatas);
    }

    public void StartGameplay()
    {
        SoundManager.Instance.PlayBGM(BGM.Gameplay);
        SoundManager.Instance.PlayBGM(BGM.Cooking, 0.1f, turnOffOthers: false);
        gameplayPhase.LoadStage(playerInstructionsDatas);
    }
    
    public Sprite GetInstructionSprite(InstructionType instructionType)
    {
        string assetName = "items_" + (int)instructionType;
        Sprite sprite = spriteAtlas.GetSprite(assetName);

        return sprite;
    }
    
    public Sprite GetChefSprite(int chefId)
    {
        string assetName = "ChefFace_" + chefId;
        Sprite sprite = spriteAtlas.GetSprite(assetName);

        return sprite;
    }

    public Sprite GetHeadChefSprite(ChefState chefState)
    {
        Sprite ret = spriteAtlas.GetSprite("HeadChef_Normal");
        switch(chefState)
        {
            case ChefState.Angry:
            case ChefState.VeryAngry:
                ret = spriteAtlas.GetSprite("HeadChef_Angry");
                break;
            default:
                break;
        }

        return ret;

    }
}


