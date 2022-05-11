using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parameter
{
    public static float TIME_LIMIT_INSTRUCTIONS_PHASE_EASY = 15;
    public static float TIME_LIMIT_INSTRUCTIONS_PHASE_MEDIUM = 12;
    public static float TIME_LIMIT_INSTRUCTIONS_PHASE_HARD = 8;
    public static float TIME_LIMIT_SINGLE_TASK_MAX_EASY = 15;
    public static float TIME_LIMIT_SINGLE_TASK_MAX_MEDIUM = 12;
    public static float TIME_LIMIT_SINGLE_TASK_MAX_HARD = 9;
    public static float TIME_LIMIT_SINGLE_TASK_MIN = 5;

    public static int CHEF_FACES_COUNT = 6;

    public static int INSTRUCTION_MIN_COUNT_EASY = 3;
    public static int INSTRUCTION_MIN_COUNT_MEDIUM = 4;
    public static int INSTRUCTION_MIN_COUNT_HARD = 5;
    public static int INSTRUCTION_MAX_COUNT_EASY = 4;
    public static int INSTRUCTION_MAX_COUNT_MEDIUM = 5;
    public static int INSTRUCTION_MAX_COUNT_HARD = 6;
    public static int INSTRUCTION_MAX_TOTAL_PLAYABLE_COUNT = 12;

    public static int TOTAL_LIFE_POINTS_MINIMUM = 3;

    public static float TIMER_BAR_WIDTH_DEFAULT = 1500f;

    public static string HEXCOLOR_WIN_GREEN = "A2FF00FF";
    public static string HEXCOLOR_LOSE_RED = "FF0F00";

    public static string[] LOSE_QUOTES = new[]
    {
        "GET OUT OF THE KITCHEN!",
        "MY GRAN COULD DO BETTER!",
        "TOO SLOW YOU DONKEY!"
    };
    
    public static string[] WIN_QUOTES = new[]
    {
        "NOT BAD!",
        "THAT YOUR BEST SHOT?",
        "GOOD JOB! BUT IT COULD BE BETTER!"
    };
}
