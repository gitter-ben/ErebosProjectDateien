using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class spielFertig : MonoBehaviour
{
    public PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        playerStats.currentLevel = 1;
        playerStats.Gold = 0;
        playerStats.swordLevel = 1;
        playerStats.armorLevel = 1;
        playerStats.WunschKristalle = 0;
        playerStats.PlayerLevel = 1;
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
