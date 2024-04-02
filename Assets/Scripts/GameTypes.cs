using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public enum PaddleSize
{
    Small,
    Medium,
    Big
}

public enum GameState
{
    Idle,
    LaunchSequence,
    LetsPong,
    GameWon
}