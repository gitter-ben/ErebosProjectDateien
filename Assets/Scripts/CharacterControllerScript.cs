using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerScript : MonoBehaviour
{
      [Header("Movement")]
      [Range(0, 20)]public float moveSpeed = 11;
      [Range(0, 20)]public float initJumpBoost = 10;
      [Range(0, 1)]public float AirMoveFaktor = 0.5F;
      [Range(0, 1)]public float moveSmoothTimeAccelerate = 0.1F;
      [Range(0, 1)]public float moveSmoothTimeDeccelerate = 0.1F;

      [Header("GroundCheck")]
      public Transform GroundCheckCollider;
      public LayerMask groundLayer;

      private Rigidbody2D rb;
      private Vector2 move;
      private Vector3 velocity;
      private float velocityY;
      private bool isjumping;
      private float distToGround;

      private Vector3 currentDirVelocity = Vector3.zero;
      private Vector3 currentVelocity;
      private Vector3 targetVelocity;

      // Start is called before the first frame update
      void Awake()
      {
        rb = GetComponent<Rigidbody2D>();
        distToGround = GetComponent<Collider2D>().bounds.extents.y;

        //controls.Jump.performed += _ => Jump;;
      }

      bool IsGrounded() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheckCollider.position, 0.48F, groundLayer);
        if (colliders.Length > 0) {
          return true;
        } else {
          return false;
        }
      }

      void FixedUpdate() {
        MoveUpdate();
      }

      void MoveUpdate() {
        //Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        targetVelocity = (move.x * Vector3.right) * moveSpeed * (IsGrounded() ? 1 : AirMoveFaktor);
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref currentDirVelocity, targetVelocity.magnitude > currentVelocity.magnitude ? moveSmoothTimeAccelerate : moveSmoothTimeDeccelerate);

        rb.velocity = currentVelocity + rb.velocity.y * Vector3.up;
      }

      public void Movement(InputAction.CallbackContext ctx) {
        move = new Vector2(ctx.ReadValue<float>(), 0);
      }

     public void Jump(InputAction.CallbackContext ctx) {
        if (IsGrounded()) {
          rb.velocity += Vector2.up * initJumpBoost;
        }
    }
}
