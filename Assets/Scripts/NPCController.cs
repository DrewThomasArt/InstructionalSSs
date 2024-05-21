using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidBody;
    [HideInInspector]
    public Animator animator;
    public float moveSpeed;
    Vector2 moveDir = new Vector2();
    Vector2 lastMoveDir;

    public bool canMove = true;
    //Make instance of this script to be able reference from other scripts!
    public static NPCController instance;
      void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
    }

      void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        moveDir.Normalize();

        bool isIdle = moveDir.x == 0 && moveDir.y == 0;

 if (isIdle){
    rigidBody.velocity = Vector2.zero;
    animator.SetBool("isWalking", false);
 } else {
    lastMoveDir = moveDir;
    rigidBody.velocity = moveDir * moveSpeed;
    animator.SetFloat("moveX", moveDir.x);
    animator.SetFloat("moveY", moveDir.y);
    animator.SetBool("isWalking", true);
 }
    }
}
