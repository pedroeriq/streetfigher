using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float forwardJumpForce;
    
    public GameObject hadouken;
    public Transform pontoDeTiro;
    
    private bool forwardJump;
    private bool isJumping;
    private bool isAttacking;
    private Animator anim;
    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        ForwardJump();
        
        if (isAttacking == false)
        {
            StartCoroutine("Atacar");
        }
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);
        
        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        } 
        
        if (movement < 0)
        {
            if (!isJumping )
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping)
        {
            anim.SetInteger("transition", 0);
        }
    }
    void Jump()
    {
        if(Input.GetKey(KeyCode.W))
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }

    private void ForwardJump()
    {
        if(Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        {
            if (!forwardJump)
            {
                anim.SetInteger("transition", 3);
                rig.AddForce(new Vector2(0,forwardJumpForce), ForceMode2D.Impulse);
                forwardJump = true;
            }
        }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {
            if (!forwardJump)
            {
                anim.SetInteger("transition", 3);
                rig.AddForce(new Vector2(0,forwardJumpForce), ForceMode2D.Impulse);
                forwardJump = true;
            }
        }
    }
    private IEnumerator Atacar()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isAttacking = true;
            anim.SetBool("lpunch", true);
            yield return new WaitForSecondsRealtime(0.2f);
            isAttacking = false;
            anim.SetBool("lpunch", false);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            isAttacking = true;
            anim.SetBool("LMKick", true);
            yield return new WaitForSecondsRealtime(0.2f);
            isAttacking = false;
            anim.SetBool("LMKick", false);
        }
        if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.Y) && Input.GetKey(KeyCode.U))
        {
            isAttacking = true;
            anim.SetBool("Hadouken", true);
            yield return new WaitForSecondsRealtime(0.58f);
            isAttacking = false;
            anim.SetBool("Hadouken", false);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
   {
       if (collision.gameObject.layer == 3)
       {
           isJumping = false;
           forwardJump = false;
       }
   }
}