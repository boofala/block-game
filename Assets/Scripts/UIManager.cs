using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public enum Player
    {
        Player1,
        Player2
    }

    public bool gameStart = true;
    public bool gameEnd = false;
    public GameObject gameState;

    //Game Score
    private Dictionary<string, int> gameScore = new Dictionary<string, int>
    {
        {"Player1", 0},
        {"Player2", 0},
        {"Player3", 0},
        {"Player4", 0}
    };
    private Canvas canvasGameObject;
    private GameObject textParent;
    private Text text;

    public void UpdateScore(Player loser)
    {
        GameSettings.scores[loser] -= 1;
    }

    // Game States
}
