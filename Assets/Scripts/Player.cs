using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [Range(0, 20)] public float moveSpeed = 11;
    [Range(0, 20)] public float initJumpBoost = 10;
    [Range(0, 1)] public float AirMoveFaktor = 0.5F;
    [Range(0, 1)] public float moveSmoothTimeAccelerate = 0.1F;
    [Range(0, 1)] public float moveSmoothTimeDeccelerate = 0.1F;

    public Transform GroundCheck;
    public LayerMask groundLayer;

    [Header("Combat")]
    public Enemy enemy;
    public LayerMask hitLayer;
    public Vector3 localHitLocation;
    public float hitDistance;
    public int damagePerHit;
    [Range(0, 5)] public float HitCoolDown = 1;
    [Space]
    [Range(0, 200)] public int StartHealth = 100;
    public float armorAblationFaktor;

    [Header("Animation")]
    public Transform IKeffectorLeft;
    public Transform IKeffectorRight;
    public GameObject BloodEffect;

    public PolygonCollider2D leftCollider;
    public PolygonCollider2D rightCollider;

    [Header("UI")]
    public HealthBar healthBar;

    // Privates
    private Rigidbody2D rb;
    private Vector2 move;

    private float direction = 1F;
    private float lastdirection = 1F;
    private float lastAttackTime = 0F;
    [HideInInspector] public int currentHealth;
    [HideInInspector] public Vector2 knockBackVelocity;

    private Vector3 currentDirVelocity = Vector3.zero;
    private Vector3 currentVelocity;
    private Vector3 targetVelocity;

    private float AnimationTime = 0f;
    private bool AnimationIsPlaying = false;
    private float AnimationTimeTotal = 0.1f;
    private float AnimationDirection = 1;
    private bool AnimationIsHit = false;
    private float lastAttackTimeAnim = 0f;
    private Vector3 IKeffectorStartPosLeft;
    private Vector3 IKeffectorStartPosRight;
    private Vector3 NonHitAnimPosLeft;
    private Vector3 NonHitAnimPosRight;
    private bool x;
    private float lastJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        IKeffectorStartPosLeft = new Vector3(-1.4F, 0.5F, 0F);
        IKeffectorStartPosRight = new Vector3(1.4F, 0.5F, 0F);
        NonHitAnimPosLeft = new Vector3(-1.75F, 0.45F, 0F);
        NonHitAnimPosRight = new Vector3(1.75F, 0.45F, 0F);
        currentHealth = StartHealth;
        healthBar.SetMaxHealth(StartHealth);
        healthBar.SetHealth(currentHealth);
    }

    void FixedUpdate()
    {
        Move();
        Animate();
        //CheckPos();
    }

    void CheckPos()
    {
        if (transform.position.y < 20)
        {
            currentHealth = 0;
        }
    }

    void Move()
    {
        targetVelocity = (move.x * Vector3.right) * moveSpeed * (IsGrounded() ? 1 : AirMoveFaktor);
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref currentDirVelocity, targetVelocity.magnitude > currentVelocity.magnitude ? moveSmoothTimeAccelerate : moveSmoothTimeDeccelerate);

        rb.velocity = currentVelocity + (rb.velocity.y * Vector3.up);
    }

    void Animate()
    {
        if (direction == 1)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        } else if (direction == -1)
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
                          + (direction == 1 ? IKeffectorStartPosRight : IKeffectorStartPosLeft), enemy.transform.position + enemy.localHitLocation));
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
        } else
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
        } else if (direction == 1)
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
        } else if (arm == 2)
        {
            transform.GetChild(4).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(5).GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, 0.28F);
        List<Collider2D> collidersList = new List<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground") || col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collidersList.Add(col);
            }
        }

        return collidersList.Count > 0;
    }

    public Vector3 calcAnimationPos(float Time, float TotalTime, Vector3 startPos, Vector3 endPos)
    {
        Vector3 dist = endPos - startPos;
        Vector3 distPerSec = dist / TotalTime;
        return startPos + distPerSec * Time;
    }

    public void DoDamage(int damage, Vector2 hitOrigin)
    {
        //Debug.Log("Before: " + currentHealth.ToString());
        currentHealth -= (int)(damage * armorAblationFaktor);
        Debug.Log(armorAblationFaktor);
        //Debug.Log(currentHealth);
        healthBar.SetHealth(currentHealth);
        Vector2 hitDirection = new Vector2(transform.position.x, transform.position.y) - hitOrigin;
        hitDirection.Normalize();

        rb.AddForce((new Vector2(hitDirection.x, 0) + Vector2.up) * 5, ForceMode2D.Impulse);
        if (currentHealth <= 0)
        {
            FindObjectOfType<GameManagerScript>().EndGame(0);
        }
    }

    public void Movement(InputAction.CallbackContext ctx)
    {
        move = new Vector2(ctx.ReadValue<float>(), 0);
        direction = move.x;
        if (direction == 0)
        {
            direction = lastdirection;
        }
        lastdirection = direction;
    }
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (IsGrounded() && Time.time - lastJumpTime >= 0.5f)
        {
            lastJumpTime = Time.time;
            rb.velocity += Vector2.up * initJumpBoost;
            FindObjectOfType<AudioManager>().Play("Jump");
        }
    }
    public void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (Time.time - lastAttackTime >= HitCoolDown)
            {
                //RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), hitDistance, hitLayer);
                Collider2D hit = Physics2D.OverlapCircle(transform.position, 1.3f, hitLayer);

                if (hit)
                {
                    enemy.DoDamage(damagePerHit, new Vector2(transform.position.x, transform.position.y));
                    lastAttackTime = Time.time;
                    Instantiate<GameObject>(BloodEffect, enemy.transform.position + enemy.localHitLocation, Quaternion.identity);
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
}
