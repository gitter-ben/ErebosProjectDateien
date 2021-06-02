using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Opponent", menuName = "Opponent/Normal")]
public class Opponent : ScriptableObject
{
    public new string name;
    public string description;

    public Attack[] attacks;
}
