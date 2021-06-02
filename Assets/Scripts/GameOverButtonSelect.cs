using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtonSelect : MonoBehaviour
{
    public PlayerStats playerStats;

    public void TryAgain()
    {
        SceneManager.LoadScene("DynamicLevel");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void upgradeShop()
    {
        SceneManager.LoadScene("UpgradeShop");
    }
}
