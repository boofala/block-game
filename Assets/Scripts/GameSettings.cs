using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static readonly int FRAME_RATE = 100;

    // GRID
    public static readonly int NUM_ROWS = 9;
    public static readonly int NUM_COLUMNS = 9;

    // MOVEMENT
    public static readonly int JUMP_DELAY = 100;
    public static readonly int JUMP_ROTATION_SPEED = 2;
    public static readonly int JUMP_WOBBLE_DEGREES = 5;
    public static readonly int STEP_DISTANCE = 1;
    public static readonly int JUMP_DISTANCE = 4;
    public static readonly int DEGREES_IN_STEP = 2;
}
