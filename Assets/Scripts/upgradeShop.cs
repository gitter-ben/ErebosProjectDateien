using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class upgradeShop : MonoBehaviour
{
    public PlayerStats playerStats;

    private float timeStamp;
    private bool moneyBool;
    private GameObject moneyNotifaction;
    public GameObject moneyNotifactionCanvas;
    public GameObject normalCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        showStats();
    }

    void NotEnoughMoney ()
    {
        timeStamp = Time.time;
        moneyNotifactionCanvas.GetComponent<Canvas>().enabled = true;
        normalCanvas.GetComponent<Canvas>().enabled = false;
    }

    private void OnGUI()
    {
        if (Time.time - timeStamp >= 1)
        {
            moneyNotifactionCanvas.GetComponent<Canvas>().enabled = false;
            normalCanvas.GetComponent<Canvas>().enabled = true;
        }
    }

    public void SwordPlus()
    {
        if (playerStats.swordLevel != 20)
        {
            if (playerStats.Gold >= 40)
            {
                playerStats.swordLevel++;
                playerStats.Gold -= 40;
                showStats();
            } else
            {
                NotEnoughMoney();
            }
        }
    }

    public void ArmorPlus()
    {
        if (playerStats.armorLevel != 20)
        {
            if (playerStats.Gold >= 40)
            {
                playerStats.armorLevel++;
                playerStats.Gold -= 40;
                showStats();
            }
            else
            {
                NotEnoughMoney();
            }
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Next()
    {
        SceneManager.LoadScene("DynamicLevel");
    }

    void showStats()
    {
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Level: " + playerStats.swordLevel);
        transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Level: " + playerStats.armorLevel);
    }
}
