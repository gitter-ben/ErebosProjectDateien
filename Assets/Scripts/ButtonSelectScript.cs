using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSelectScript : MonoBehaviour
{
    public PlayerStats playerStats;

    private void Start()
    {
        //saveManager.Load();
    }

    public void Play() {
        SceneManager.LoadScene("DynamicLevel");
    }

    public void LevelSelect() {
        playerStats.currentLevel = 1;
        playerStats.Gold = 0;
        playerStats.swordLevel = 1;
        playerStats.armorLevel = 1;
        playerStats.WunschKristalle = 0;
        playerStats.PlayerLevel = 1;
        SceneManager.LoadScene("DynamicLevel");
    }

    public void Quit() {
        Application.Quit();
        //saveManager.Save();
    }
}
