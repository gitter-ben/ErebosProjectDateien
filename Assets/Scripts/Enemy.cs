using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    public Player player;

    [Header("Movement")]
    [Range(0, 20)] public float moveSpeed = 11;
    [Range(0, 20)] public float initJumpBoost = 10;
    [Range(0, 1)] public float AirMoveFaktor = 0.5F;
    [Range(0, 1)] public float moveSmoothTimeAccelerate = 0.1F;
    [Range(0, 1)] public float moveSmoothTimeDeccelerate = 0.1F;

    public Transform GroundCheck;
    public LayerMask groundLayer;

    [Header("Combat")]
    public int StartHealth;
    public Vector3 localHitLocation;
    public int damagePerHit;
    public float HitCoolDown;
    public LayerMask hitLayer;

    [Header("UI")]
    public HealthBar healthBar;

    [Header("Animation")]
    public Transform IKeffectorLeft;
    public Transform IKeffectorRight;
    public GameObject BloodEffect;

    private Rigidbody2D rb;
    [HideInInspector] public int currentHealth;
    [HideInInspector] public Vector2 knockBackVelocity;

    private Vector2 move;

    private float direction = 1F;
    private float lastdirection = 1F;
    private float lastAttackTime = 0F;

    private Vector3 currentDirVelocity = Vector3.zero;
    private Vector3 currentVelocity;
    private Vector3 targetVelocity;

    private float AnimationTime = 0f;
    private bool AnimationIsPlaying = false;
    private float AnimationTimeTotal = 0.25f;
    private float AnimationDirection = 1;
    private Vector3 IKeffectorStartPosLeft;
    private Vector3 IKeffectorStartPosRight;
    private Vector3 NonHitAnimPosRight;
    private Vector3 NonHitAnimPosLeft;
    private float lastAttackTimeAnim;
    private bool x = false;
    private bool AnimationIsHit;
    private float lastJumpTime;

    void Start()
    {
        currentHealth = StartHealth;
        rb = GetComponent<Rigidbody2D>();
        healthBar.SetMaxHealth(StartHealth);
        healthBar.SetHealth(currentHealth);

        IKeffectorStartPosLeft = new Vector3(-1.4F, 0.5F, 0F);
        IKeffectorStartPosRight = new Vector3(1.4F, 0.5F, 0F);

        NonHitAnimPosLeft = new Vector3(-1.85F, 0.55F, 0F);
        NonHitAnimPosRight = new Vector3(1.85F, 0.55F, 0F);
    }

    void FixedUpdate()
    {
        CalcMoveAI();
        Move();
        Animate();
        //Debug.Log(IsGrounded());
    }

    void CalcMoveAIB()
    {
        Vector2 localMove = new Vector2(0, 0);
        Vector3 diff = player.transform.position - transform.position;
        float diffx = player.transform.position.x - transform.position.x;
        float dist = CalcDist(player.transform.position, transform.position);

        if (transform.position.x > player.transform.position.x)
        {
            if (diffx < -1.2f)
            {
                localMove.x = -1;
            }
            //localMove.x = -1;
        } else
        {
            if (diffx > 1.2f)
            {
                localMove.x = 1;
            }
        }
        //Attack();

        //if (transform.position.y < player.transform.position.y)
        //{
        //    Jump();
        //}

        //Debug.Log(dist);

        if (dist < 1.3f)
        {
            direction = (diffx > 0 ? 1 : -1);
            Attack();
        }

        //StartCoroutine(ReturnAfterDelay(localMove));
    }

    void CalcMoveAI()
    {
        float diffx = player.transform.position.x - transform.position.x;
        List<int> probabilityList = new List<int>();
        probabilityList.Add(1);
        for (int i = 0; i < 9999; i++)
        {
            probabilityList.Add(1);
        }

        if (UnityEngine.Random.Range(0f, 2.5f) < 0.1f)
        {
            Attack();
        }
        //Debug.Log(UnityEngine.Random.Range(0f, 1f));

        //ReturnDir(dir);
        if (UnityEngine.Random.Range(0f, 2.5f) < 0.1f)
        {
            direction = (diffx > 0 ? 1 : -1);
            move = new Vector2(direction, 0);
        }
    }

    IEnumerator ReturnMove(Vector2 input)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));
        move = input;
    }

    IEnumerator ReturnDir(float dir)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));
        direction = dir;
    }

    float CalcDist(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }

    void Move()
    {
        targetVelocity = (move.x * Vector3.right) * moveSpeed * (IsGrounded() ? 1 : AirMoveFaktor);
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref currentDirVelocity, targetVelocity.magnitude > currentVelocity.magnitude ? moveSmoothTimeAccelerate : moveSmoothTimeDeccelerate);

        if (targetVelocity == Vector3.zero)
        {

        } else
        {
            rb.velocity = currentVelocity + (rb.velocity.y * Vector3.up);
        }
        //direction = (rb.velocity.x > 0 ? 1 : -1);
    }

    void Animate()
    {
        if (direction == 1)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else if (direction == -1)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        if (AnimationIsPlaying)
        {
            //Debug.Log("Animation Is playing: " + AnimationTime.ToString());
            AnimationTime += Time.fixedDeltaTime;
            if (AnimationIsHit)
            {
                MoveSword(AnimationDirection, calcAnimationPos(AnimationTime, AnimationTimeTotal, transform.position
                          + (direction == 1 ? IKeffectorStartPosRight : IKeffectorStartPosLeft), player.transform.position + player.localHitLocation));
            }
            else
            {
                MoveSword(AnimationDirection, calcAnimationPos(AnimationTime, AnimationTimeTotal, transform.position
                    + (direction == 1 ? IKeffectorStartPosRight : IKeffectorStartPosLeft), transform.position +
                    (direction == 1 ? NonHitAnimPosRight : NonHitAnimPosLeft)));
            }
            if (AnimationTime >= AnimationTimeTotal)
            {
                AnimationTime = 0;
                AnimationIsPlaying = false;
            }
        }
        else
        {
            MoveSword(direction, transform.position + (direction == 1 ? IKeffectorStartPosRight : IKeffectorStartPosLeft));
        }
    }
    void MoveSword(float direction, Vector3 globalPosition)
    {
        if (direction == -1)
        {
            IKeffectorLeft.position = globalPosition;
            ShowArm(1);
        }
        else if (direction == 1)
        {
            IKeffectorRight.position = globalPosition;
            ShowArm(2);
        }
    }
    void ShowArm(int arm) // 1: Left  2: Right
    {
        if (arm == 1)
        {
            transform.GetChild(4).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(5).GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (arm == 2)
        {
            transform.GetChild(4).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(5).GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    public Vector3 calcAnimationPos(float Time, float TotalTime, Vector3 startPos, Vector3 endPos)
    {
        Vector3 dist = endPos - startPos;
        Vector3 distPerSec = dist / TotalTime;
        return startPos + distPerSec * Time;
    }

    bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, 0.48F);
        List<Collider2D> collidersList = new List<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))// || col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                collidersList.Add(col);
            }
        }

        return collidersList.Count > 0;
    }

    public void Jump()
    {
        if (IsGrounded() && Time.time - lastJumpTime >= 0.5f)
        {
            lastJumpTime = Time.time;
            rb.velocity += Vector2.up * initJumpBoost;
            FindObjectOfType<AudioManager>().Play("Jump");
        }
    }

    public void DoDamage(int damage, Vector2 hitOrigin)
    {
        currentHealth -= Convert.ToInt32(damage);
        //Debug.Log(damage * -1);
        Vector2 hitDirection = new Vector2(transform.position.x, transform.position.y) - hitOrigin;
        hitDirection.Normalize();

        rb.AddForce((new Vector2(hitDirection.x, 0) + Vector2.up)*5, ForceMode2D.Impulse);
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            FindObjectOfType<GameManagerScript>().EndGame(1);
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime >= HitCoolDown)
        {
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), hitDistance, hitLayer);
            Collider2D hit = Physics2D.OverlapCircle(transform.position, 1.3f, hitLayer);

            if (hit)
            {
                player.DoDamage(damagePerHit, new Vector2(transform.position.x, transform.position.y));
                //Debug.Log(damagePerHit);
                lastAttackTime = Time.time;
                Instantiate<GameObject>(BloodEffect, player.transform.position + player.localHitLocation, Quaternion.identity);
                AnimationIsPlaying = true;
                AnimationIsHit = true;
                AnimationDirection = direction;
                x = true;
            }
            lastAttackTime = Time.time;
            FindObjectOfType<AudioManager>().Play("Hit");
        }
        if (Time.time - lastAttackTimeAnim >= HitCoolDown && !x)
        {
            lastAttackTimeAnim = Time.time;
            AnimationIsPlaying = true;
            AnimationIsHit = false;
            AnimationDirection = direction;
            FindObjectOfType<AudioManager>().Play("Hit");
        }
        x = false;
    }
}
