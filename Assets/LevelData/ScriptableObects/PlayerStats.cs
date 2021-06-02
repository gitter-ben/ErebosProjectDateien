using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Playerstats", menuName = "Playerstats")]
public class PlayerStats : ScriptableObject
{
    [Header("General")]
    public new string name;
    public int PlayerLevel;
    public int currentLevel;

    [Header("Combat")]
    public int maxHealth;

    public int swordLevel;
    public int armorLevel;
    public float HitCoolDownPlayer;

    [Header("Inventory")]
    public int Gold;
    public int WunschKristalle;
}
