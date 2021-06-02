using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [Header("Fighters")]
    public Enemy enemy;
    public Player player;

    [HideInInspector]public bool isBossFight;

    private bool gameHasEnded = false;

    void Start()
    {
        //enemy.EnableMove();
        //player.EnableMove();
    }

    void Update()
    {

    }

    public void EndGame(int gameCondition)  // 1: Win 0: Lose
    {
        Debug.Log(isBossFight);
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            if (isBossFight && gameCondition == 1)
            {
                SceneManager.LoadScene("AbsoluteWin");
            }
            else
            {
                if (gameCondition == 1)
                {
                    SceneManager.LoadScene("Win");
                }
                else if (gameCondition == 0)
                {
                    SceneManager.LoadScene("GameOver");
                }
            }
        }
    }
}
