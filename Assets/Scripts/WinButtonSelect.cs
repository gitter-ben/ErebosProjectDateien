using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinButtonSelect : MonoBehaviour
{
    public PlayerStats playerStats;

    private int goldReward;
    private int levelReward;

    public void Start()
    {
        playerStats.currentLevel++;
        goldReward = FindObjectOfType<rewardContainer>().goldReward;
        levelReward = FindObjectOfType<rewardContainer>().levelReward;
        playerStats.Gold += goldReward;
        playerStats.PlayerLevel += levelReward;

        gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("+" + goldReward.ToString() + " Goldstücke\n" + "+" + levelReward.ToString() + " Level");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("UpgradeShop");
    }
}
