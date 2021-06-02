using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("General")]
    public List<Level> Levels;
    public Level bossFight;
    public PlayerStats playerStats;
    private Level level;


    private GameObject enemy;
    private GameObject player;
    private GameObject canvas;
    private GameObject healthBarPlayer;
    private GameObject healthBarEnemy;
    private GameObject map;
    private GameObject cam;
    private GameObject gameManager;
    private GameObject BackGround;
    private GameObject rewardCanvas;

    private Player playerScript;
    private Enemy enemyScript;
    private HealthBar healthBarScriptPlayer;
    private HealthBar healthBarScriptEnemy;
    private GameManagerScript gameManagerScript;
    private Parallax2 parallaxScript;

    private float timeStamp;
    private float rewardShowTime = 3;
    private bool doneInit = false;

    private bool bossFightBool = false;
    
    void Start()
    {
        if (playerStats.currentLevel != 11)
        {
            level = Levels[playerStats.currentLevel - 1];
        } else
        {
            level = bossFight;
            bossFightBool = true;
            
        }
        ShowReward();
        timeStamp = Time.time;
    }

    void ShowReward()
    {
        rewardCanvas = Instantiate<GameObject>(level.rewardCanvasPrefab, level.rewardCanvasSpawn, Quaternion.identity);
        rewardCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("Level: " + playerStats.currentLevel);

        if (bossFightBool)
        {
            rewardCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Belohnung:\nEin Platz im\nInneren Kreis");
        } else
        {
            rewardCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Belohnung:\n" + level.Gold + " Goldstücke\n" + level.PlusLevels + " Level");
        } 
        FindObjectOfType<rewardContainer>().goldReward = level.Gold;
        FindObjectOfType<rewardContainer>().levelReward = level.PlusLevels;
    }

    void Update()
    {
        if (!doneInit)
        {
            rewardCanvas.GetComponentInChildren<Slider>().value = (Time.time - timeStamp) / rewardShowTime;
        }
        if (Time.time - timeStamp >= rewardShowTime && !doneInit)
        {
            Destroy(rewardCanvas);
            INITALL();
            doneInit = true;
        }
    }

    void INITALL()
    {
        //FindObjectOfType<AudioManager>().Play("Music");
        cam = Camera.main.gameObject;
        SpawnMap();
        SpawnEnemy();
        SpawnPlayer();
        SpawnCanvas();
        AssignOpponent();
        AssignHealthBar();
        InitCam();
        InitGameManager();
        InitBackGround();
    }

    void AssignOpponent()
    {
        playerScript.enemy = enemyScript;
        enemyScript.player = playerScript;
        canvas.GetComponentInChildren<TextMeshProUGUI>().SetText(playerStats.name + " vs. " + level.OpponentName);
    }
    
    void AssignHealthBar()
    {
        playerScript.healthBar = healthBarScriptPlayer;
        enemyScript.healthBar = healthBarScriptEnemy;
    }

    void SpawnEnemy ()
    {
        enemy = Instantiate<GameObject>(level.enemyPrefab, level.enemySpawn, Quaternion.identity) as GameObject;
        enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.hitLayer = level.hitLayerEnemy;
        enemyScript.StartHealth = level.EnemyHealth;
        enemyScript.HitCoolDown = level.HitCoolDownEnemy;
        enemyScript.damagePerHit = level.damagePerAttack;
        enemyScript.knockBackVelocity = level.knockBackVelocityEnemy;
    }

    void SpawnPlayer()
    {
        player = Instantiate<GameObject>(level.playerPrefab, level.playerSpawn, Quaternion.identity) as GameObject;
        playerScript = player.GetComponent<Player>();
        playerScript.hitLayer = level.hitLayerPlayer;
        playerScript.damagePerHit = Mathf.RoundToInt(Mathf.Sqrt(playerStats.swordLevel) + 1F) * 5;

        //playerScript.armorAblationFaktor = (float)playerStats.armorLevel / 80f; //Quarter of the damage at Level 20
        playerScript.armorAblationFaktor = 1f - (0.75f * ((playerStats.armorLevel - 1f) / 19f));

        playerScript.HitCoolDown = playerStats.HitCoolDownPlayer;
        playerScript.StartHealth = playerStats.maxHealth;
        playerScript.knockBackVelocity = level.knockBackVelocity;
    }

    void SpawnCanvas()
    {
        canvas = Instantiate<GameObject>(level.canvasPrefab, level.canvasSpawn, Quaternion.identity) as GameObject;
        healthBarPlayer = canvas.transform.GetChild(1).gameObject;
        healthBarEnemy = canvas.transform.GetChild(2).gameObject;
        healthBarScriptPlayer = healthBarPlayer.GetComponent<HealthBar>();
        healthBarScriptEnemy = healthBarEnemy.GetComponent<HealthBar>();
    }

    void SpawnMap()
    {
        map = Instantiate<GameObject>(level.Map, level.mapSpawn, Quaternion.identity);
    }

    void InitCam()
    {
        FollowObject camScript = cam.AddComponent<FollowObject>();
        camScript.ObjectToFollow = player;

        camScript.useClamp = level.useClamp;
        camScript.useClampX = level.useClampX;
        camScript.useClampY = level.useClampY;
        camScript.useClampZ = level.useClampZ;
        camScript.ClampXMin = level.ClampXMin;
        camScript.ClampYMin = level.ClampYMin;
        camScript.ClampZMin = level.ClampZMin;
        camScript.ClampXMax = level.ClampXMax;
        camScript.ClampYMax = level.ClampYMax;
        camScript.ClampZMax = level.ClampZMax;

        camScript.useOffset = level.useOffset;
        camScript.useOffsetX = level.useOffsetX;
        camScript.useOffsetY = level.useOffsetY;
        camScript.useOffsetZ = level.useOffsetZ;
        camScript.OffsetX = level.OffsetX;
        camScript.OffsetY = level.OffsetY;
        camScript.OffsetZ = level.OffsetZ;
    }

    void InitGameManager()
    {
        gameManager = Instantiate<GameObject>(level.gameManagerPrefab, Vector3.zero, Quaternion.identity);
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        gameManagerScript.player = playerScript;
        gameManagerScript.enemy = enemyScript;
        gameManagerScript.isBossFight = bossFightBool;
    }

    void InitBackGround()
    {
        BackGround = Instantiate<GameObject>(level.BackgroundPrefab, Vector3.zero, Quaternion.identity);
        parallaxScript = BackGround.GetComponent<Parallax2>();
        parallaxScript.cam = Camera.main.gameObject;
    }
}
