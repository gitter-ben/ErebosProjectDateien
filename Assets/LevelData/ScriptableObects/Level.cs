using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level/Level")]
public class Level : ScriptableObject
{
    [Header("General")]
    public new string name;
    public string OpponentName;

    [Header("Rewards")]
    public int Gold;
    public int PlusLevels;

    [Header("Enemy")]
    public int damagePerAttack;
    public float HitCoolDownEnemy;
    public int EnemyHealth;

    [Header("Prefabs")]
    public GameObject Map;
    public GameObject gameManagerPrefab;
    public GameObject BackgroundPrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject canvasPrefab;
    public GameObject rewardCanvasPrefab;

    [Header("Other")]
    public LayerMask hitLayerPlayer;
    public LayerMask hitLayerEnemy;
    [Space]
    public Vector2 knockBackVelocityEnemy;
    public Vector2 knockBackVelocity;

    [Header("SpawnPositions")]
    public Vector3 playerSpawn;
    public Vector3 enemySpawn;
    public Vector3 canvasSpawn;
    public Vector3 mapSpawn;
    public Vector3 rewardCanvasSpawn;

    [Header("Cam")]
    public bool useClamp;
    public bool useClampX;
    public bool useClampY;
    public bool useClampZ;

    public float ClampXMin;
    public float ClampXMax;
    public float ClampYMin;
    public float ClampYMax;
    public float ClampZMin;
    public float ClampZMax;
    [Space]
    public bool useOffset;
    public bool useOffsetX;
    public bool useOffsetY;
    public bool useOffsetZ;

    public float OffsetX;
    public float OffsetY;
    public float OffsetZ;
}
